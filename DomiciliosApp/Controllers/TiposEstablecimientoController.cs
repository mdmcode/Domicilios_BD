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
    public class TiposEstablecimientoController : Controller
    {
        private readonly DomiciliosContext _context;

        public TiposEstablecimientoController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: TiposEstablecimiento
        public async Task<IActionResult> Index()
        {
            return View(await _context.tipoestablecimientos.ToListAsync());
        }

        // GET: TiposEstablecimiento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoestablecimiento = await _context.tipoestablecimientos
                .FirstOrDefaultAsync(m => m.id_tipoEstablecimiento == id);
            if (tipoestablecimiento == null)
            {
                return NotFound();
            }

            return View(tipoestablecimiento);
        }

        // GET: TiposEstablecimiento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposEstablecimiento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_tipoEstablecimiento,nombre")] tipoestablecimiento tipoestablecimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoestablecimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoestablecimiento);
        }

        // GET: TiposEstablecimiento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoestablecimiento = await _context.tipoestablecimientos.FindAsync(id);
            if (tipoestablecimiento == null)
            {
                return NotFound();
            }
            return View(tipoestablecimiento);
        }

        // POST: TiposEstablecimiento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_tipoEstablecimiento,nombre")] tipoestablecimiento tipoestablecimiento)
        {
            if (id != tipoestablecimiento.id_tipoEstablecimiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoestablecimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tipoestablecimientoExists(tipoestablecimiento.id_tipoEstablecimiento))
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
            return View(tipoestablecimiento);
        }

        // GET: TiposEstablecimiento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoestablecimiento = await _context.tipoestablecimientos
                .FirstOrDefaultAsync(m => m.id_tipoEstablecimiento == id);
            if (tipoestablecimiento == null)
            {
                return NotFound();
            }

            return View(tipoestablecimiento);
        }

        // POST: TiposEstablecimiento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoestablecimiento = await _context.tipoestablecimientos.FindAsync(id);
            if (tipoestablecimiento != null)
            {
                _context.tipoestablecimientos.Remove(tipoestablecimiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tipoestablecimientoExists(int id)
        {
            return _context.tipoestablecimientos.Any(e => e.id_tipoEstablecimiento == id);
        }
    }
}
