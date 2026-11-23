using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DomiciliosApp.Models;

namespace DomiciliosApp.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly DomiciliosContext _context;

        public SolicitudesController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: Solicitudes
        [HttpGet]
        public async Task<IActionResult> Index(int? idEstado, int? idMetodoPago)
        {
            var query = _context.solicituds
                .AsNoTracking()
                .Include(s => s.id_clienteNavigation)
                    .ThenInclude(c => c.id_clienteNavigation)
                .Include(s => s.id_domiciliarioNavigation)
                    .ThenInclude(d => d!.id_domiciliarioNavigation)
                .Include(s => s.id_estadoNavigation)
                .Include(s => s.id_metodoPagoNavigation)
                .Include(s => s.id_establecimientoRecogidaNavigation)
                .Include(s => s.id_direccionRecogidaExternaNavigation)
                .AsQueryable();

            if (idEstado.HasValue)
                query = query.Where(s => s.id_estado == idEstado.Value);

            if (idMetodoPago.HasValue)
                query = query.Where(s => s.id_metodoPago == idMetodoPago.Value);

            ViewBag.IdEstado = idEstado;
            ViewBag.IdMetodoPago = idMetodoPago;

            return View(await query.OrderByDescending(s => s.id_solicitud).ToListAsync());
        }

        // GET: Solicitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var solicitud = await _context.solicituds
                .Include(s => s.cliente_direccion)
                .Include(s => s.id_clienteNavigation)
                .Include(s => s.id_direccionRecogidaExternaNavigation)
                .Include(s => s.id_domiciliarioNavigation)
                .Include(s => s.id_establecimientoRecogidaNavigation)
                .Include(s => s.id_estadoNavigation)
                .Include(s => s.id_metodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.id_solicitud == id);

            if (solicitud == null) return NotFound();

            // Función fn_minutos_asignacion y tabla de auditoría del trigger trg_solicitud_log_au
            ViewBag.MinutosAsignacion = await DbSqlHelper.ObtenerMinutosAsignacionAsync(_context, solicitud.id_solicitud);
            ViewBag.HistorialEstados = await _context.log_estado_solicituds
                .AsNoTracking()
                .Where(l => l.id_solicitud == solicitud.id_solicitud)
                .OrderBy(l => l.id)
                .ToListAsync();

            return View(solicitud);
        }

        // GET: Solicitudes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Solicitudes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("descripcion,montoTotal,id_metodoPago,id_cliente,id_direccionEntrega,id_establecimientoRecogida,id_direccionRecogidaExterna")] solicitud solicitud)
        {
            // El procedimiento sp_crear_solicitud asigna fechaCreacion, horaCreacion e id_estado = 1
            ModelState.Remove(nameof(solicitud.fechaCreacion));
            ModelState.Remove(nameof(solicitud.horaCreacion));
            ModelState.Remove(nameof(solicitud.id_estado));

            await ValidarReglasAsync(solicitud);

            if (ModelState.IsValid)
            {
                try
                {
                    var nuevoId = await DbSqlHelper.CrearSolicitudAsync(_context, solicitud);
                    TempData["Exito"] = $"Solicitud #{nuevoId} creada correctamente.";
                    return RedirectToAction(nameof(Details), new { id = nuevoId });
                }
                catch (Exception ex)
                {
                    var msg = DbSqlHelper.ExtraerMensajeNegocio(ex);
                    if (msg != null)
                        ModelState.AddModelError(string.Empty, msg);
                    else
                        throw;
                }
            }

            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", solicitud.id_cliente);
            ViewData["id_direccionRecogidaExterna"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", solicitud.id_direccionRecogidaExterna);
            ViewData["id_domiciliario"] = new SelectList(_context.domiciliarios, "id_domiciliario", "id_domiciliario", solicitud.id_domiciliario);
            ViewData["id_establecimientoRecogida"] = new SelectList(_context.establecimientos, "id_establecimiento", "id_establecimiento", solicitud.id_establecimientoRecogida);
            ViewData["id_estado"] = new SelectList(_context.estadosolicituds, "id_estado", "id_estado", solicitud.id_estado);
            ViewData["id_metodoPago"] = new SelectList(_context.metodopagos, "id_metodoPago", "id_metodoPago", solicitud.id_metodoPago);
            return View(solicitud);
        }

        // GET: Solicitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.solicituds.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", solicitud.id_cliente);
            ViewData["id_direccionRecogidaExterna"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", solicitud.id_direccionRecogidaExterna);
            ViewData["id_domiciliario"] = new SelectList(_context.domiciliarios, "id_domiciliario", "id_domiciliario", solicitud.id_domiciliario);
            ViewData["id_establecimientoRecogida"] = new SelectList(_context.establecimientos, "id_establecimiento", "id_establecimiento", solicitud.id_establecimientoRecogida);
            ViewData["id_estado"] = new SelectList(_context.estadosolicituds, "id_estado", "id_estado", solicitud.id_estado);
            ViewData["id_metodoPago"] = new SelectList(_context.metodopagos, "id_metodoPago", "id_metodoPago", solicitud.id_metodoPago);
            return View(solicitud);
        }

        // POST: Solicitudes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_solicitud,fechaCreacion,horaCreacion,descripcion,montoTotal,id_estado,id_metodoPago,id_cliente,id_domiciliario,fechaAsignacion,horaAsignacion,id_direccionEntrega,id_establecimientoRecogida,id_direccionRecogidaExterna")] solicitud solicitud)
        {
            if (id != solicitud.id_solicitud)
            {
                return NotFound();
            }

            await ValidarReglasAsync(solicitud);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var msg = DbSqlHelper.ExtraerMensajeNegocio(ex);
                    if (msg != null)
                    {
                        ModelState.AddModelError(string.Empty, msg);
                    }
                    else if (ex is DbUpdateConcurrencyException && !solicitudExists(solicitud.id_solicitud))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", solicitud.id_cliente);
            ViewData["id_direccionRecogidaExterna"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", solicitud.id_direccionRecogidaExterna);
            ViewData["id_domiciliario"] = new SelectList(_context.domiciliarios, "id_domiciliario", "id_domiciliario", solicitud.id_domiciliario);
            ViewData["id_establecimientoRecogida"] = new SelectList(_context.establecimientos, "id_establecimiento", "id_establecimiento", solicitud.id_establecimientoRecogida);
            ViewData["id_estado"] = new SelectList(_context.estadosolicituds, "id_estado", "id_estado", solicitud.id_estado);
            ViewData["id_metodoPago"] = new SelectList(_context.metodopagos, "id_metodoPago", "id_metodoPago", solicitud.id_metodoPago);
            return View(solicitud);
        }

        // GET: Solicitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.solicituds
                .Include(s => s.cliente_direccion)
                .Include(s => s.id_clienteNavigation)
                .Include(s => s.id_direccionRecogidaExternaNavigation)
                .Include(s => s.id_domiciliarioNavigation)
                .Include(s => s.id_establecimientoRecogidaNavigation)
                .Include(s => s.id_estadoNavigation)
                .Include(s => s.id_metodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.id_solicitud == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: Solicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.solicituds.FindAsync(id);
            if (solicitud != null)
            {
                _context.solicituds.Remove(solicitud);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task ValidarReglasAsync(solicitud s)
        {
            // La dirección de entrega debe estar registrada para ese cliente (clave foránea compuesta)
            var existe = await _context.cliente_direccions
                .AnyAsync(cd => cd.id_cliente == s.id_cliente && cd.id_direccion == s.id_direccionEntrega);
            if (!existe)
                ModelState.AddModelError(nameof(s.id_direccionEntrega),
                    "La dirección de entrega no está registrada para el cliente seleccionado.");
        }

        private bool solicitudExists(int id)
        {
            return _context.solicituds.Any(e => e.id_solicitud == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(int id, int id_domiciliario)
        {
            try
            {
                await DbSqlHelper.AsignarDomiciliarioAsync(_context, id, id_domiciliario);
                TempData["Exito"] = "Domiciliario asignado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = DbSqlHelper.ExtraerMensajeNegocio(ex) ?? "No se pudo asignar el domiciliario.";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id)
        {
            try
            {
                await DbSqlHelper.FinalizarSolicitudAsync(_context, id);
                TempData["Exito"] = "Solicitud finalizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = DbSqlHelper.ExtraerMensajeNegocio(ex) ?? "No se pudo finalizar la solicitud.";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                await DbSqlHelper.CancelarSolicitudAsync(_context, id);
                TempData["Exito"] = "Solicitud cancelada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = DbSqlHelper.ExtraerMensajeNegocio(ex) ?? "No se pudo cancelar la solicitud.";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarEntrega(int id)
        {
            try
            {
                await DbSqlHelper.IniciarEntregaAsync(_context, id);
                TempData["Exito"] = "Entrega iniciada (En Curso).";
            }
            catch (Exception ex)
            {
                TempData["Error"] = DbSqlHelper.ExtraerMensajeNegocio(ex) ?? "No se pudo iniciar la entrega.";
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}