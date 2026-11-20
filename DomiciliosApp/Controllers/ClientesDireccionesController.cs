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
    public class ClientesDireccionesController : Controller
    {
        private readonly DomiciliosContext _context;

        public ClientesDireccionesController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: ClientesDirecciones
        public async Task<IActionResult> Index()
        {
            var domiciliosContext = _context.cliente_direccions.Include(c => c.id_clienteNavigation).Include(c => c.id_direccionNavigation);
            return View(await domiciliosContext.ToListAsync());
        }

        // GET: ClientesDirecciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions
                .Include(c => c.id_clienteNavigation)
                .Include(c => c.id_direccionNavigation)
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente_direccion == null)
            {
                return NotFound();
            }

            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Create
        public IActionResult Create()
        {
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente");
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion");
            return View();
        }

        // POST: ClientesDirecciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_cliente,id_direccion")] cliente_direccion cliente_direccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente_direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", cliente_direccion.id_cliente);
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", cliente_direccion.id_direccion);
            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions.FindAsync(id);
            if (cliente_direccion == null)
            {
                return NotFound();
            }
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", cliente_direccion.id_cliente);
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", cliente_direccion.id_direccion);
            return View(cliente_direccion);
        }

        // POST: ClientesDirecciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_cliente,id_direccion")] cliente_direccion cliente_direccion)
        {
            if (id != cliente_direccion.id_cliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente_direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cliente_direccionExists(cliente_direccion.id_cliente))
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
            ViewData["id_cliente"] = new SelectList(_context.clientes, "id_cliente", "id_cliente", cliente_direccion.id_cliente);
            ViewData["id_direccion"] = new SelectList(_context.direccions, "id_direccion", "id_direccion", cliente_direccion.id_direccion);
            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions
                .Include(c => c.id_clienteNavigation)
                .Include(c => c.id_direccionNavigation)
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente_direccion == null)
            {
                return NotFound();
            }

            return View(cliente_direccion);
        }

        // POST: ClientesDirecciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente_direccion = await _context.cliente_direccions.FindAsync(id);
            if (cliente_direccion != null)
            {
                _context.cliente_direccions.Remove(cliente_direccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cliente_direccionExists(int id)
        {
            return _context.cliente_direccions.Any(e => e.id_cliente == id);
        }
    }
}
