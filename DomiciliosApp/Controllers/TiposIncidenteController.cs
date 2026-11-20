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
    public class TiposIncidenteController : Controller
    {
        private readonly DomiciliosContext _context;

        public TiposIncidenteController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: TiposIncidente
        public async Task<IActionResult> Index()
        {
            return View(await _context.tipoincidentes.ToListAsync());
        }

        // GET: TiposIncidente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoincidente = await _context.tipoincidentes
                .FirstOrDefaultAsync(m => m.id_tipoIncidente == id);
            if (tipoincidente == null)
            {
                return NotFound();
            }

            return View(tipoincidente);
        }

        // GET: TiposIncidente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposIncidente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_tipoIncidente,nombre")] tipoincidente tipoincidente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoincidente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoincidente);
        }

        // GET: TiposIncidente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoincidente = await _context.tipoincidentes.FindAsync(id);
            if (tipoincidente == null)
            {
                return NotFound();
            }
            return View(tipoincidente);
        }

        // POST: TiposIncidente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_tipoIncidente,nombre")] tipoincidente tipoincidente)
        {
            if (id != tipoincidente.id_tipoIncidente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoincidente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tipoincidenteExists(tipoincidente.id_tipoIncidente))
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
            return View(tipoincidente);
        }

        // GET: TiposIncidente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoincidente = await _context.tipoincidentes
                .FirstOrDefaultAsync(m => m.id_tipoIncidente == id);
            if (tipoincidente == null)
            {
                return NotFound();
            }

            return View(tipoincidente);
        }

        // POST: TiposIncidente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoincidente = await _context.tipoincidentes.FindAsync(id);
            if (tipoincidente != null)
            {
                _context.tipoincidentes.Remove(tipoincidente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tipoincidenteExists(int id)
        {
            return _context.tipoincidentes.Any(e => e.id_tipoIncidente == id);
        }
    }
}
