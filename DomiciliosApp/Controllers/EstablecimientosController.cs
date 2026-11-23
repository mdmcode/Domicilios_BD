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
    public class EstablecimientosController : Controller
    {
        private readonly DomiciliosContext _context;

        public EstablecimientosController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: Establecimientos
        public async Task<IActionResult> Index(int? idTipoEstablecimiento)
        {
            var query = _context.establecimientos
                .AsNoTracking()
                .Include(e => e.id_tipoEstablecimientoNavigation)
                .Include(e => e.id_direccionNavigation)
                .AsQueryable();

            if (idTipoEstablecimiento.HasValue)
                query = query.Where(e => e.id_tipoEstablecimiento == idTipoEstablecimiento.Value);

            ViewData["TiposEstablecimiento"] = await _context.TiposEstablecimientoAsync(idTipoEstablecimiento);
            ViewData["FiltroTipo"] = idTipoEstablecimiento;

            return View(await query.ToListAsync());
        }

        // GET: Establecimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.establecimientos
                .Include(e => e.id_direccionNavigation)
                .Include(e => e.id_tipoEstablecimientoNavigation)
                .FirstOrDefaultAsync(m => m.id_establecimiento == id);
            if (establecimiento == null)
            {
                return NotFound();
            }

            return View(establecimiento);
        }

        // GET: Establecimientos/Create
        public IActionResult Create()
        {
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion");
            ViewData["id_tipoEstablecimiento"] = new SelectList(_context.tipoestablecimientos, "id_tipoEstablecimiento", "id_tipoEstablecimiento");
            return View();
        }

        // POST: Establecimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_establecimiento,codigo,nombre,id_tipoEstablecimiento,id_direccion,telefono,email")] establecimiento establecimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(establecimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", establecimiento.id_direccion);
            ViewData["id_tipoEstablecimiento"] = new SelectList(_context.tipoestablecimientos, "id_tipoEstablecimiento", "id_tipoEstablecimiento", establecimiento.id_tipoEstablecimiento);
            return View(establecimiento);
        }

        // GET: Establecimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.establecimientos.FindAsync(id);
            if (establecimiento == null)
            {
                return NotFound();
            }
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", establecimiento.id_direccion);
            ViewData["id_tipoEstablecimiento"] = new SelectList(_context.tipoestablecimientos, "id_tipoEstablecimiento", "id_tipoEstablecimiento", establecimiento.id_tipoEstablecimiento);
            return View(establecimiento);
        }

        // POST: Establecimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_establecimiento,codigo,nombre,id_tipoEstablecimiento,id_direccion,telefono,email")] establecimiento establecimiento)
        {
            if (id != establecimiento.id_establecimiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(establecimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!establecimientoExists(establecimiento.id_establecimiento))
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
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", establecimiento.id_direccion);
            ViewData["id_tipoEstablecimiento"] = new SelectList(_context.tipoestablecimientos, "id_tipoEstablecimiento", "id_tipoEstablecimiento", establecimiento.id_tipoEstablecimiento);
            return View(establecimiento);
        }

        // GET: Establecimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.establecimientos
                .Include(e => e.id_direccionNavigation)
                .Include(e => e.id_tipoEstablecimientoNavigation)
                .FirstOrDefaultAsync(m => m.id_establecimiento == id);
            if (establecimiento == null)
            {
                return NotFound();
            }

            return View(establecimiento);
        }

        // POST: Establecimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var establecimiento = await _context.establecimientos.FindAsync(id);
            if (establecimiento != null)
            {
                _context.establecimientos.Remove(establecimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool establecimientoExists(int id)
        {
            return _context.establecimientos.Any(e => e.id_establecimiento == id);
        }
    }
}
