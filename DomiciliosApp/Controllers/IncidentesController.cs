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
    public class IncidentesController : Controller
    {
        private readonly DomiciliosContext _context;

        public IncidentesController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: Incidentes
        public async Task<IActionResult> Index()
        {
            var domiciliosContext = _context.incidentes.Include(i => i.id_solicitudNavigation).Include(i => i.tipoIncidenteNavigation);
            return View(await domiciliosContext.ToListAsync());
        }

        // GET: Incidentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidente = await _context.incidentes
                .Include(i => i.id_solicitudNavigation)
                .Include(i => i.tipoIncidenteNavigation)
                .FirstOrDefaultAsync(m => m.id_incidente == id);
            if (incidente == null)
            {
                return NotFound();
            }

            return View(incidente);
        }

        // GET: Incidentes/Create
        public IActionResult Create()
        {
            ViewData["id_solicitud"] = new SelectList(_context.solicituds, "id_solicitud", "id_solicitud");
            ViewData["tipoIncidente"] = new SelectList(_context.tipoincidentes, "id_tipoIncidente", "id_tipoIncidente");
            return View();
        }

        // POST: Incidentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tipoIncidente,descripcion,id_solicitud")] incidente incidente)
        {
            // El procedimiento sp_registrar_incidente asigna fecha y hora actuales automáticamente
            ModelState.Remove(nameof(incidente.fecha));
            ModelState.Remove(nameof(incidente.hora));

            if (ModelState.IsValid)
            {
                try
                {
                    await DbSqlHelper.RegistrarIncidenteAsync(
                        _context,
                        incidente.id_solicitud,
                        incidente.tipoIncidente,
                        incidente.descripcion);

                    TempData["Exito"] = "Incidente registrado correctamente.";
                    return RedirectToAction(nameof(Index));
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

            ViewData["id_solicitud"] = new SelectList(_context.solicituds, "id_solicitud", "id_solicitud", incidente.id_solicitud);
            ViewData["tipoIncidente"] = new SelectList(_context.tipoincidentes, "id_tipoIncidente", "id_tipoIncidente", incidente.tipoIncidente);
            return View(incidente);
        }

        // GET: Incidentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidente = await _context.incidentes.FindAsync(id);
            if (incidente == null)
            {
                return NotFound();
            }
            ViewData["id_solicitud"] = new SelectList(_context.solicituds, "id_solicitud", "id_solicitud", incidente.id_solicitud);
            ViewData["tipoIncidente"] = new SelectList(_context.tipoincidentes, "id_tipoIncidente", "id_tipoIncidente", incidente.tipoIncidente);
            return View(incidente);
        }

        // POST: Incidentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_incidente,tipoIncidente,descripcion,fecha,hora,id_solicitud")] incidente incidente)
        {
            if (id != incidente.id_incidente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!incidenteExists(incidente.id_incidente))
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
            ViewData["id_solicitud"] = new SelectList(_context.solicituds, "id_solicitud", "id_solicitud", incidente.id_solicitud);
            ViewData["tipoIncidente"] = new SelectList(_context.tipoincidentes, "id_tipoIncidente", "id_tipoIncidente", incidente.tipoIncidente);
            return View(incidente);
        }

        // GET: Incidentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidente = await _context.incidentes
                .Include(i => i.id_solicitudNavigation)
                .Include(i => i.tipoIncidenteNavigation)
                .FirstOrDefaultAsync(m => m.id_incidente == id);
            if (incidente == null)
            {
                return NotFound();
            }

            return View(incidente);
        }

        // POST: Incidentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidente = await _context.incidentes.FindAsync(id);
            if (incidente != null)
            {
                _context.incidentes.Remove(incidente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool incidenteExists(int id)
        {
            return _context.incidentes.Any(e => e.id_incidente == id);
        }
    }
}
