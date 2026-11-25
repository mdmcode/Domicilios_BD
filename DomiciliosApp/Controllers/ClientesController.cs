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
    public class ClientesController : Controller
    {
        private readonly DomiciliosContext _context;

        public ClientesController(DomiciliosContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.clientes
                .Include(c => c.id_clienteNavigation)   // cliente -> usuario
                .AsNoTracking()
                .ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes
                .Include(c => c.id_clienteNavigation)
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewBag.TotalGastado = await DbSqlHelper.ObtenerTotalClienteAsync(_context, cliente.id_cliente);

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View(new UsuarioCrearVM { Tipo = UsuarioCrearVM.TipoCliente });
        }

        // POST: Clientes/Create
        // Crea el usuario (datos personales) y su fila en cliente, junto con las direcciones elegidas.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCrearVM vm)
        {
            vm.Tipo = UsuarioCrearVM.TipoCliente;   // esta pantalla solo crea clientes

            if (ModelState.IsValid)
            {
                try
                {
                    await UsuarioServicio.CrearAsync(_context, vm);
                    TempData["Exito"] = "Cliente creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    var msg = DbSqlHelper.ExtraerMensajeNegocio(ex);
                    if (msg == null) throw;
                    ModelState.AddModelError(string.Empty, msg);
                }
            }
            return View(vm);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["id_cliente"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario", cliente.id_cliente);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_cliente")] cliente cliente)
        {
            if (id != cliente.id_cliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!clienteExists(cliente.id_cliente))
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
            ViewData["id_cliente"] = new SelectList(_context.usuarios, "id_usuario", "id_usuario", cliente.id_cliente);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes
                .Include(c => c.id_clienteNavigation)
                .FirstOrDefaultAsync(m => m.id_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool clienteExists(int id)
        {
            return _context.clientes.Any(e => e.id_cliente == id);
        }
    }
}
