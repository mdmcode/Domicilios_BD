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
    public class MediosTransporteController : Controller
    {
        private readonly DomiciliosContext _context;

        public MediosTransporteController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: MediosTransporte
        public async Task<IActionResult> Index()
        {
            return View(await _context.mediodetransportes.ToListAsync());
        }

        // GET: MediosTransporte/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediodetransporte = await _context.mediodetransportes
                .FirstOrDefaultAsync(m => m.id_transporte == id);
            if (mediodetransporte == null)
            {
                return NotFound();
            }

            return View(mediodetransporte);
        }

        // GET: MediosTransporte/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MediosTransporte/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_transporte,nombre")] mediodetransporte mediodetransporte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mediodetransporte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mediodetransporte);
        }

        // GET: MediosTransporte/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediodetransporte = await _context.mediodetransportes.FindAsync(id);
            if (mediodetransporte == null)
            {
                return NotFound();
            }
            return View(mediodetransporte);
        }

        // POST: MediosTransporte/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_transporte,nombre")] mediodetransporte mediodetransporte)
        {
            if (id != mediodetransporte.id_transporte)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mediodetransporte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mediodetransporteExists(mediodetransporte.id_transporte))
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
            return View(mediodetransporte);
        }

        // GET: MediosTransporte/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mediodetransporte = await _context.mediodetransportes
                .FirstOrDefaultAsync(m => m.id_transporte == id);
            if (mediodetransporte == null)
            {
                return NotFound();
            }

            return View(mediodetransporte);
        }

        // POST: MediosTransporte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediodetransporte = await _context.mediodetransportes.FindAsync(id);
            if (mediodetransporte != null)
            {
                _context.mediodetransportes.Remove(mediodetransporte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mediodetransporteExists(int id)
        {
            return _context.mediodetransportes.Any(e => e.id_transporte == id);
        }
    }
}
