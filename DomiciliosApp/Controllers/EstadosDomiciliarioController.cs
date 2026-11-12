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
    public class EstadosDomiciliarioController : Controller
    {
        private readonly DomiciliosContext _context;

        public EstadosDomiciliarioController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: EstadosDomiciliario
        public async Task<IActionResult> Index()
        {
            return View(await _context.estadodomiciliarios.ToListAsync());
        }

        // GET: EstadosDomiciliario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodomiciliario = await _context.estadodomiciliarios
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estadodomiciliario == null)
            {
                return NotFound();
            }

            return View(estadodomiciliario);
        }

        // GET: EstadosDomiciliario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosDomiciliario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_estado,nombre")] estadodomiciliario estadodomiciliario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadodomiciliario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadodomiciliario);
        }

        // GET: EstadosDomiciliario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodomiciliario = await _context.estadodomiciliarios.FindAsync(id);
            if (estadodomiciliario == null)
            {
                return NotFound();
            }
            return View(estadodomiciliario);
        }

        // POST: EstadosDomiciliario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_estado,nombre")] estadodomiciliario estadodomiciliario)
        {
            if (id != estadodomiciliario.id_estado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadodomiciliario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!estadodomiciliarioExists(estadodomiciliario.id_estado))
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
            return View(estadodomiciliario);
        }

        // GET: EstadosDomiciliario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodomiciliario = await _context.estadodomiciliarios
                .FirstOrDefaultAsync(m => m.id_estado == id);
            if (estadodomiciliario == null)
            {
                return NotFound();
            }

            return View(estadodomiciliario);
        }

        // POST: EstadosDomiciliario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadodomiciliario = await _context.estadodomiciliarios.FindAsync(id);
            if (estadodomiciliario != null)
            {
                _context.estadodomiciliarios.Remove(estadodomiciliario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool estadodomiciliarioExists(int id)
        {
            return _context.estadodomiciliarios.Any(e => e.id_estado == id);
        }
    }
}
