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
    public class EstadosSolicitudController : Controller
    {
        private readonly DomiciliosContext _context;

        public EstadosSolicitudController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: EstadosSolicitud
        public async Task<IActionResult> Index()
        {
            return View(await _context.estadosolicituds.ToListAsync());
        }

        // GET: EstadosSolicitud/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosolicitud = await _context.estadosolicituds
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estadosolicitud == null)
            {
                return NotFound();
            }

            return View(estadosolicitud);
        }

        // GET: EstadosSolicitud/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosSolicitud/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_estado,nombre")] estadosolicitud estadosolicitud)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadosolicitud);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadosolicitud);
        }

        // GET: EstadosSolicitud/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosolicitud = await _context.estadosolicituds.FindAsync(id);
            if (estadosolicitud == null)
            {
                return NotFound();
            }
            return View(estadosolicitud);
        }

        // POST: EstadosSolicitud/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_estado,nombre")] estadosolicitud estadosolicitud)
        {
            if (id != estadosolicitud.id_estado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadosolicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!estadosolicitudExists(estadosolicitud.id_estado))
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
            return View(estadosolicitud);
        }

        // GET: EstadosSolicitud/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosolicitud = await _context.estadosolicituds
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estadosolicitud == null)
            {
                return NotFound();
            }

            return View(estadosolicitud);
        }

        // POST: EstadosSolicitud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadosolicitud = await _context.estadosolicituds.FindAsync(id);
            if (estadosolicitud != null)
            {
                _context.estadosolicituds.Remove(estadosolicitud);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool estadosolicitudExists(int id)
        {
            return _context.estadosolicituds.Any(e => e.id_estado == id);
        }
    }
}
