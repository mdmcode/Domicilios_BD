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
    public class DomiciliariosController : Controller
    {
        private readonly DomiciliosContext _context;

        public DomiciliariosController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: Domiciliarios
        public async Task<IActionResult> Index()
        {
            var domiciliarios = await _context.domiciliarios
                .Include(d => d.id_domiciliarioNavigation)   // domiciliario -> usuario
                .Include(d => d.id_estadoNavigation)         // estadodomiciliario
                .Include(d => d.id_transporteNavigation)     // mediodetransporte
                .AsNoTracking()
                .ToListAsync();
            return View(domiciliarios);
        }
        
        // GET: Domiciliarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domiciliario = await _context.domiciliarios
                .Include(d => d.id_domiciliarioNavigation)
                .Include(d => d.id_estadoNavigation)
                .Include(d => d.id_transporteNavigation)
                .FirstOrDefaultAsync(m => m.id_domiciliario == id);
            if (domiciliario == null)
            {
                return NotFound();
            }

            return View(domiciliario);
        }

        // GET: Domiciliarios/Create
        public IActionResult Create()
        {
            ViewData["id_domiciliario"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario");
            ViewData["id_estado"] = new SelectList(_context.estadodomiciliarios, "id_estado", "id_estado");
            ViewData["id_transporte"] = new SelectList(_context.mediodetransportes, "id_transporte", "id_transporte");
            return View();
        }

        // POST: Domiciliarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_domiciliario,codigo,id_transporte,id_estado")] domiciliario domiciliario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(domiciliario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_domiciliario"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario", domiciliario.id_domiciliario);
            ViewData["id_estado"] = new SelectList(_context.estadodomiciliarios, "id_estado", "id_estado", domiciliario.id_estado);
            ViewData["id_transporte"] = new SelectList(_context.mediodetransportes, "id_transporte", "id_transporte", domiciliario.id_transporte);
            return View(domiciliario);
        }

        // GET: Domiciliarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domiciliario = await _context.domiciliarios.FindAsync(id);
            if (domiciliario == null)
            {
                return NotFound();
            }
            ViewData["id_domiciliario"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario", domiciliario.id_domiciliario);
            ViewData["id_estado"] = new SelectList(_context.estadodomiciliarios, "id_estado", "id_estado", domiciliario.id_estado);
            ViewData["id_transporte"] = new SelectList(_context.mediodetransportes, "id_transporte", "id_transporte", domiciliario.id_transporte);
            return View(domiciliario);
        }

        // POST: Domiciliarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_domiciliario,codigo,id_transporte,id_estado")] domiciliario domiciliario)
        {
            if (id != domiciliario.id_domiciliario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domiciliario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!domiciliarioExists(domiciliario.id_domiciliario))
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
            ViewData["id_domiciliario"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario", domiciliario.id_domiciliario);
            ViewData["id_estado"] = new SelectList(_context.estadodomiciliarios, "id_estado", "id_estado", domiciliario.id_estado);
            ViewData["id_transporte"] = new SelectList(_context.mediodetransportes, "id_transporte", "id_transporte", domiciliario.id_transporte);
            return View(domiciliario);
        }

        // GET: Domiciliarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domiciliario = await _context.domiciliarios
                .Include(d => d.id_domiciliarioNavigation)
                .Include(d => d.id_estadoNavigation)
                .Include(d => d.id_transporteNavigation)
                .FirstOrDefaultAsync(m => m.id_domiciliario == id);
            if (domiciliario == null)
            {
                return NotFound();
            }

            return View(domiciliario);
        }

        // POST: Domiciliarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var domiciliario = await _context.domiciliarios.FindAsync(id);
            if (domiciliario != null)
            {
                _context.domiciliarios.Remove(domiciliario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool domiciliarioExists(int id)
        {
            return _context.domiciliarios.Any(e => e.id_domiciliario == id);
        }
    }
}
