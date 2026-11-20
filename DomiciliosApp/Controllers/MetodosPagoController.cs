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
    public class MetodosPagoController : Controller
    {
        private readonly DomiciliosContext _context;

        public MetodosPagoController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: MetodosPago
        public async Task<IActionResult> Index()
        {
            return View(await _context.metodopagos.ToListAsync());
        }

        // GET: MetodosPago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodopago = await _context.metodopagos
                .FirstOrDefaultAsync(m => m.id_metodoPago == id);
            if (metodopago == null)
            {
                return NotFound();
            }

            return View(metodopago);
        }

        // GET: MetodosPago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MetodosPago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_metodoPago,nombre")] metodopago metodopago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metodopago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metodopago);
        }

        // GET: MetodosPago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodopago = await _context.metodopagos.FindAsync(id);
            if (metodopago == null)
            {
                return NotFound();
            }
            return View(metodopago);
        }

        // POST: MetodosPago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_metodoPago,nombre")] metodopago metodopago)
        {
            if (id != metodopago.id_metodoPago)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(metodopago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!metodopagoExists(metodopago.id_metodoPago))
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
            return View(metodopago);
        }

        // GET: MetodosPago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodopago = await _context.metodopagos
                .FirstOrDefaultAsync(m => m.id_metodoPago == id);
            if (metodopago == null)
            {
                return NotFound();
            }

            return View(metodopago);
        }

        // POST: MetodosPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var metodopago = await _context.metodopagos.FindAsync(id);
            if (metodopago != null)
            {
                _context.metodopagos.Remove(metodopago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool metodopagoExists(int id)
        {
            return _context.metodopagos.Any(e => e.id_metodoPago == id);
        }
    }
}
