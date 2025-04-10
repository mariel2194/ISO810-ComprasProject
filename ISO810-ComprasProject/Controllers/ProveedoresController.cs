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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace ISO810_ComprasProject.Controllers
{
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly ComprasDBContext _context;

        public ProveedoresController(ComprasDBContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedor.ToListAsync());
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            ViewBag.TipoDocumento = new SelectList(new[] { "Cédula", "RNC" });
            return View(new Proveedores());
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorId,TipoDocumento,Cedula,RNC,NombreComercial,Activo")] Proveedores proveedores)
        {

            if (proveedores.TipoDocumento == "Cédula")
            {
                proveedores.RNC = "N/A";
                if (!validaCedula(proveedores.Cedula) || proveedores.Cedula.Length == 0)
                {
                    ModelState.AddModelError("Cedula", "Por digite una cédula válida ");
                }
            }
            else if (!esUnRNCValido(proveedores.RNC) || proveedores.RNC.Length == 0)
            {
                proveedores.Cedula = "N/A";
                if (!esUnRNCValido(proveedores.RNC))
                {
                    ModelState.AddModelError("RNC", "Por digite digite un RNC válido.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(proveedores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TipoDocumento = new SelectList(new[] { "Cédula", "RNC" }, proveedores.TipoDocumento);
            return View(proveedores);
        }


        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.Proveedor.FindAsync(id);
            if (proveedores == null)
            {
                return NotFound();
            }
            ViewBag.TipoDocumento = new SelectList(new[] { "Cédula", "RNC" }, proveedores.TipoDocumento);
            return View(proveedores);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedores = await _context.Proveedor
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedores == null)
            {
                return NotFound();
            }

            return View(proveedores);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedores = await _context.Proveedor.FindAsync(id);
            if (proveedores != null)
            {
                _context.Proveedor.Remove(proveedores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedoresExists(int id)
        {
            return _context.Proveedor.Any(e => e.ProveedorId == id);
        }

        private bool esUnRNCValido(string pRNC)
        {
            if (string.IsNullOrWhiteSpace(pRNC)) return false;

            int vnTotal = 0;
            int[] digitoMult = new int[8] { 7, 9, 8, 6, 5, 4, 3, 2 };

            string vcRNC = pRNC.Replace("-", "").Replace(" ", "");

            if (vcRNC.Length != 9) return false;
            if (vcRNC[0] != '1' && vcRNC[0] != '4' && vcRNC[0] != '5') return false;

            if (!int.TryParse(vcRNC.Substring(8, 1), out int vDigito)) return false;

            for (int vDig = 0; vDig < 8; vDig++)
            {
                if (!int.TryParse(vcRNC[vDig].ToString(), out int vNumero)) return false;
                vnTotal += vNumero * digitoMult[vDig];
            }

            int residuo = vnTotal % 11;
            int digitoCalculado = (residuo == 0 || residuo == 1) ? 0 : (11 - residuo);

            return vDigito == digitoCalculado;
        }



        private static bool validaCedula(string pCedula)

        {
            int vnTotal = 0;
            string vcCedula = pCedula.Replace("-", "");
            int pLongCed = vcCedula.Trim().Length;
            int[] digitoMult = new int[11] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };
            if (pLongCed < 11 || pLongCed > 11)
                return false;
            for (int vDig = 1; vDig <= pLongCed; vDig++)
            {
                int vCalculo = Int32.Parse(vcCedula.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];
                if (vCalculo < 10)
                    vnTotal += vCalculo;
                else
                    vnTotal += Int32.Parse(vCalculo.ToString().Substring(0, 1)) + Int32.Parse(vCalculo.ToString().Substring(1, 1));
            }
            if (vnTotal % 10 == 0)
                return true;
            else
                return false;
        }


    }
}
