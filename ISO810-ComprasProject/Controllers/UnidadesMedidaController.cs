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
    public class UnidadesMedidaController : Controller
    {
        private readonly ComprasDBContext _context;

        public UnidadesMedidaController(ComprasDBContext context)
        {
            _context = context;
        }

        // GET: UnidadesMedida
        public async Task<IActionResult> Index()
        {
            return View(await _context.UnidadMedida.ToListAsync());
        }

        // GET: UnidadesMedida/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadesMedida = await _context.UnidadMedida
                .FirstOrDefaultAsync(m => m.UnidadMedidaId == id);
            if (unidadesMedida == null)
            {
                return NotFound();
            }

            return View(unidadesMedida);
        }

        // GET: UnidadesMedida/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UnidadesMedida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnidadMedidaId,Descripcion,Activo")] UnidadesMedida unidadesMedida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unidadesMedida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(unidadesMedida);
        }

        // GET: UnidadesMedida/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadesMedida = await _context.UnidadMedida.FindAsync(id);
            if (unidadesMedida == null)
            {
                return NotFound();
            }
            return View(unidadesMedida);
        }

        // POST: UnidadesMedida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnidadMedidaId,Descripcion,Activo")] UnidadesMedida unidadesMedida)
        {
            if (id != unidadesMedida.UnidadMedidaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidadesMedida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadesMedidaExists(unidadesMedida.UnidadMedidaId))
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
            return View(unidadesMedida);
        }

        // GET: UnidadesMedida/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidadesMedida = await _context.UnidadMedida
                .FirstOrDefaultAsync(m => m.UnidadMedidaId == id);
            if (unidadesMedida == null)
            {
                return NotFound();
            }

            return View(unidadesMedida);
        }

        // POST: UnidadesMedida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unidadesMedida = await _context.UnidadMedida.FindAsync(id);
            if (unidadesMedida != null)
            {
                _context.UnidadMedida.Remove(unidadesMedida);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnidadesMedidaExists(int id)
        {
            return _context.UnidadMedida.Any(e => e.UnidadMedidaId == id);
        }
    }
}
