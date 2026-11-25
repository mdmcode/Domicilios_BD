using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        // Las vistas arman los nombres con Db.Clientes() / Db.Direcciones(), no necesitan Include.
        public async Task<IActionResult> Index()
        {
            return View(await _context.cliente_direccions.AsNoTracking().ToListAsync());
        }

        // GET: ClientesDirecciones/Details?id_cliente=32&id_direccion=45
        public async Task<IActionResult> Details(int? id_cliente, int? id_direccion)
        {
            if (id_cliente == null || id_direccion == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id_cliente == id_cliente && m.id_direccion == id_direccion);
            if (cliente_direccion == null)
            {
                return NotFound();
            }

            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClientesDirecciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_cliente,id_direccion")] cliente_direccion cliente_direccion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(cliente_direccion);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Dirección asignada al cliente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Muestra el motivo real que devuelve MySQL (llave foránea, duplicado, etc.)
                    ModelState.AddModelError(string.Empty,
                        "No se pudo guardar: " + (ex.InnerException?.Message ?? ex.Message));
                }
            }
            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Edit?id_cliente=32&id_direccion=45
        // Esta tabla solo tiene las dos claves, así que no hay nada que modificar:
        // la vista muestra la información y explica que se elimina y se crea de nuevo.
        public async Task<IActionResult> Edit(int? id_cliente, int? id_direccion)
        {
            if (id_cliente == null || id_direccion == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id_cliente == id_cliente && m.id_direccion == id_direccion);
            if (cliente_direccion == null)
            {
                return NotFound();
            }

            return View(cliente_direccion);
        }

        // GET: ClientesDirecciones/Delete?id_cliente=32&id_direccion=45
        public async Task<IActionResult> Delete(int? id_cliente, int? id_direccion)
        {
            if (id_cliente == null || id_direccion == null)
            {
                return NotFound();
            }

            var cliente_direccion = await _context.cliente_direccions
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.id_cliente == id_cliente && m.id_direccion == id_direccion);
            if (cliente_direccion == null)
            {
                return NotFound();
            }

            return View(cliente_direccion);
        }

        // POST: ClientesDirecciones/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id_cliente, int id_direccion)
        {
            var cliente_direccion = await _context.cliente_direccions.FindAsync(id_cliente, id_direccion);
            if (cliente_direccion != null)
            {
                try
                {
                    _context.cliente_direccions.Remove(cliente_direccion);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Dirección eliminada del cliente.";
                }
                catch (DbUpdateException)
                {
                    // La llave foránea de solicitud apunta a este par (cliente, dirección)
                    TempData["Error"] = "No se puede eliminar: esa dirección ya se usó como dirección de entrega en una solicitud de este cliente.";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}