using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ISO810_ComprasProject.Data;
using ISO810_ComprasProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace ISO810_ComprasProject.Controllers
{
    [Authorize]
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
            ViewBag.Articulos = _context.Articulo
                .Select(a => new { a.ArticuloId, a.CostoUnitario })
                .ToList(); 

            ViewData["UnidadMedidaId"] = new SelectList(_context.UnidadMedida, "UnidadMedidaId", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompraId,Fecha,ArticuloId,Cantidad,Monto,UnidadMedidaId,Estado")] OrdenCompras ordenCompras)
        {
            if (ModelState.IsValid)
            {

                _context.Add(ordenCompras);
                await _context.SaveChangesAsync();
                if (ordenCompras.Estado == EstadoCompra.Completada)
                {
                    // crear la transacción
                    var transaccion = new Transaccion
                    {
                        Descripcion = $"Orden #{ordenCompras.CompraId} completada",
                        Fecha = DateTime.Now,
                        Monto = (decimal)ordenCompras.Monto,
                        AsientoId = null,
                        FechaAsiento = null
                    };

                    ordenCompras.Transaccion = transaccion;
                }
             
                _context.Update(ordenCompras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

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
        public async Task<IActionResult> Edit(int id, [Bind("CompraId,Fecha,ArticuloId,Cantidad,Monto,UnidadMedidaId,Estado")] OrdenCompras ordenCompras)
        {
            if (id != ordenCompras.CompraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var ordenComprasOld = _context.OrdenCompra.AsNoTracking().FirstOrDefault(d=>d.CompraId == ordenCompras.CompraId);

                if (ordenComprasOld.Estado == EstadoCompra.Completada)
                {
                    TempData["ErrorMessage"] = "No es posible editar la orden de compra en estado completado. Ya fue creada la transaccion.";
                }

                try
                {
                    if (ordenCompras.Estado == EstadoCompra.Completada)
                    {
                        // crear la transacción
                        var transaccion = new Transaccion
                        {
                            Descripcion = $"Orden #{ordenCompras.CompraId} completada",
                            Fecha = DateTime.Now,
                            Monto = (decimal)ordenCompras.Monto,
                            AsientoId = null,
                            FechaAsiento = null
                        };

                        ordenCompras.Transaccion = transaccion;
                    }

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
            try
            {
                var ordenCompras = await _context.OrdenCompra.FindAsync(id);

                if (ordenCompras.Estado == EstadoCompra.Completada)
                {
                    TempData["ErrorMessage"] = "No es posible elimnar la orden de compra en estado completado. Ya fue creada la transaccion.";
                    return RedirectToAction(nameof(Index));
                }

                if (ordenCompras != null)
                {
                    _context.OrdenCompra.Remove(ordenCompras);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("REFERENCE constraint"))
                {
                    TempData["ErrorMessage"] = "No se puede eliminar esta Orden de Compra porque tiene registros relacionados.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar esta Orden de Compra.";
                }
                return RedirectToAction(nameof(Index));
            }

        }

        private bool OrdenComprasExists(int id)
        {
            return _context.OrdenCompra.Any(e => e.CompraId == id);
        }
    }
}
