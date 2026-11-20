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
        public async Task<IActionResult> Index()
        {
            var solicitudes = await _context.solicituds
                .Include(s => s.id_clienteNavigation)
                    .ThenInclude(c => c.id_clienteNavigation)             // cliente -> usuario
                .Include(s => s.id_domiciliarioNavigation)
                    .ThenInclude(d => d.id_domiciliarioNavigation)        // domiciliario -> usuario
                .Include(s => s.id_estadoNavigation)
                .Include(s => s.id_metodoPagoNavigation)
                .Include(s => s.id_establecimientoRecogidaNavigation)
                .Include(s => s.id_direccionRecogidaExternaNavigation)
                .AsNoTracking()
                .ToListAsync();

            return View(solicitudes);
        }

        // GET: Solicitudes/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Solicitudes/Create
        public IActionResult Create()
        {
            ViewData["id_cliente"] = new SelectList(_context.cliente_direccions, "id_cliente", "id_cliente");
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente");
            ViewData["id_direccionRecogidaExterna"] = new SelectList(_context.direccions, "id_direccion", "id_direccion");
            ViewData["id_domiciliario"] = new SelectList(_context.domiciliarios, "id_domiciliario", "id_domiciliario");
            ViewData["id_establecimientoRecogida"] = new SelectList(_context.establecimientos, "id_establecimiento", "id_establecimiento");
            ViewData["id_estado"] = new SelectList(_context.estadosolicituds, "id_estado", "id_estado");
            ViewData["id_metodoPago"] = new SelectList(_context.metodopagos, "id_metodoPago", "id_metodoPago");
            return View();
        }

        // POST: Solicitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_solicitud,fechaCreacion,horaCreacion,descripcion,montoTotal,id_estado,id_metodoPago,id_cliente,id_domiciliario,fechaAsignacion,horaAsignacion,id_direccionEntrega,id_establecimientoRecogida,id_direccionRecogidaExterna")] solicitud solicitud)
        {
            await ValidarReglasAsync(solicitud);    // Create

            if (ModelState.IsValid)
            {
                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_cliente"] = new SelectList(_context.cliente_direccions, "id_cliente", "id_cliente", solicitud.id_cliente);
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
            ViewData["id_cliente"] = new SelectList(_context.cliente_direccions, "id_cliente", "id_cliente", solicitud.id_cliente);
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", solicitud.id_cliente);
            ViewData["id_direccionRecogidaExterna"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", solicitud.id_direccionRecogidaExterna);
            ViewData["id_domiciliario"] = new SelectList(_context.domiciliarios, "id_domiciliario", "id_domiciliario", solicitud.id_domiciliario);
            ViewData["id_establecimientoRecogida"] = new SelectList(_context.establecimientos, "id_establecimiento", "id_establecimiento", solicitud.id_establecimientoRecogida);
            ViewData["id_estado"] = new SelectList(_context.estadosolicituds, "id_estado", "id_estado", solicitud.id_estado);
            ViewData["id_metodoPago"] = new SelectList(_context.metodopagos, "id_metodoPago", "id_metodoPago", solicitud.id_metodoPago);
            return View(solicitud);
        }

        // POST: Solicitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_solicitud,fechaCreacion,horaCreacion,descripcion,montoTotal,id_estado,id_metodoPago,id_cliente,id_domiciliario,fechaAsignacion,horaAsignacion,id_direccionEntrega,id_establecimientoRecogida,id_direccionRecogidaExterna")] solicitud solicitud)
        {
            if (id != solicitud.id_solicitud)
            {
                return NotFound();
            }

            await ValidarReglasAsync(solicitud);   // Edit

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!solicitudExists(solicitud.id_solicitud))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_cliente"] = new SelectList(_context.cliente_direccions, "id_cliente", "id_cliente", solicitud.id_cliente);
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
    }
}
