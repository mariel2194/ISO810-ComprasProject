using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISO810_ComprasProject.Data;
using ISO810_ComprasProject.Models;

namespace ISO810_ComprasProject.Controllers
{
    public class OrdenComprasController : Controller
    {
        private readonly ComprasDBContext _context;

        public OrdenComprasController(ComprasDBContext context)
        {
            _context = context;
        }

        // GET: OrdenCompras
        public async Task<IActionResult> Index()
        {
            var comprasDBContext = _context.OrdenCompra.Include(o => o.Articulo).Include(o => o.UnidadMedida);
            return View(await comprasDBContext.ToListAsync());
        }

        // GET: OrdenCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompras = await _context.OrdenCompra
                .Include(o => o.Articulo)
                .Include(o => o.UnidadMedida)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (ordenCompras == null)
            {
                return NotFound();
            }

            return View(ordenCompras);
        }

        // GET: OrdenCompras/Create
        public IActionResult Create()
        {
            ViewData["ArticuloId"] = new SelectList(_context.Articulo, "ArticuloId", "Descripcion");
            ViewData["UnidadMedidaId"] = new SelectList(_context.UnidadMedida, "UnidadMedidaId", "Descripcion");
            return View();
        }

        // POST: OrdenCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompraId,Fecha,ArticuloId,Cantidad,UnidadMedidaId,Estado")] OrdenCompras ordenCompras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordenCompras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticuloId"] = new SelectList(_context.Articulo, "ArticuloId", "Descripcion", ordenCompras.ArticuloId);
            ViewData["UnidadMedidaId"] = new SelectList(_context.UnidadMedida, "UnidadMedidaId", "Descripcion", ordenCompras.UnidadMedidaId);
            return View(ordenCompras);
        }

        // GET: OrdenCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompras = await _context.OrdenCompra.FindAsync(id);
            if (ordenCompras == null)
            {
                return NotFound();
            }
            ViewData["ArticuloId"] = new SelectList(_context.Articulo, "ArticuloId", "Descripcion", ordenCompras.ArticuloId);
            ViewData["UnidadMedidaId"] = new SelectList(_context.UnidadMedida, "UnidadMedidaId", "Descripcion", ordenCompras.UnidadMedidaId);
            return View(ordenCompras);
        }

        // POST: OrdenCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompraId,Fecha,ArticuloId,Cantidad,UnidadMedidaId,Estado")] OrdenCompras ordenCompras)
        {
            if (id != ordenCompras.CompraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordenCompras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenComprasExists(ordenCompras.CompraId))
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
            ViewData["ArticuloId"] = new SelectList(_context.Articulo, "ArticuloId", "Descripcion", ordenCompras.ArticuloId);
            ViewData["UnidadMedidaId"] = new SelectList(_context.UnidadMedida, "UnidadMedidaId", "Descripcion", ordenCompras.UnidadMedidaId);
            return View(ordenCompras);
        }

        // GET: OrdenCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompras = await _context.OrdenCompra
                .Include(o => o.Articulo)
                .Include(o => o.UnidadMedida)
                .FirstOrDefaultAsync(m => m.CompraId == id);
            if (ordenCompras == null)
            {
                return NotFound();
            }

            return View(ordenCompras);
        }

        // POST: OrdenCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenCompras = await _context.OrdenCompra.FindAsync(id);
            if (ordenCompras != null)
            {
                _context.OrdenCompra.Remove(ordenCompras);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenComprasExists(int id)
        {
            return _context.OrdenCompra.Any(e => e.CompraId == id);
        }
    }
}
