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
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorId,TipoDocumento,NumeroDocumento,NombreComercial,Activo")] Proveedores proveedores)
        {
            if (proveedores.TipoDocumento == "Cédula" && !validaCedula(proveedores.Cedula))
            {
                ModelState.AddModelError("NumeroDocumento", "La cédula ingresada no es válida.");
            }
            else if (proveedores.TipoDocumento == "RNC" && !esUnRNCValido(proveedores.RNC))
            {
                ModelState.AddModelError("NumeroDocumento", "El RNC ingresado no es válido.");
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
public async Task<IActionResult> Edit(int id, [Bind("ProveedorId,TipoDocumento,NumeroDocumento,NombreComercial,Activo")] Proveedores proveedores)
{
    if (id != proveedores.ProveedorId)
    {
        return NotFound();
    }

    if (proveedores.TipoDocumento == "Cédula" && !validaCedula(proveedores.Cedula))
    {
        ModelState.AddModelError("NumeroDocumento", "La cédula ingresada no es válida.");
    }
    else if (proveedores.TipoDocumento == "RNC" && !esUnRNCValido(proveedores.RNC))
    {
        ModelState.AddModelError("NumeroDocumento", "El RNC ingresado no es válido.");
    }

    if (!ModelState["NumeroDocumento"].Errors.Any())
    {
        if (ModelState.IsValid)
        {
            try
            {

                proveedores.Cedula = "0";
                proveedores.RNC = "0";
                _context.Update(proveedores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedoresExists(proveedores.ProveedorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        else if ((ModelState.ContainsKey("Cedula") && ModelState["Cedula"].Errors.Any()) || (ModelState.ContainsKey("RNC") && ModelState["RNC"].Errors.Any()))
        {

            try
            {
                proveedores.Cedula = "0";
                proveedores.RNC = "0";
                _context.Update(proveedores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedoresExists(proveedores.ProveedorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }

    ViewBag.TipoDocumento = new SelectList(new[] { "Cédula", "RNC" }, proveedores.TipoDocumento);
    return View(proveedores);
}

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

    int vnTotal = 0;

    int[] digitoMult = new int[8] { 7, 9, 8, 6, 5, 4, 3, 2 };

    string vcRNC = pRNC.Replace("-", "").Replace(" ", "");

    string vDigito = vcRNC.Substring(8, 1);

    if (vcRNC.Length.Equals(9))

        if (!"145".Contains(vcRNC.Substring(0, 1)))

            return false;



    for (int vDig = 1; vDig <= 8; vDig++)

    {

        int vCalculo = Int32.Parse(vcRNC.Substring(vDig - 1, 1)) * digitoMult[vDig - 1];

        vnTotal += vCalculo;

    }



    if (vnTotal % 11 == 0 && vDigito == "1" || vnTotal % 11 == 1 && vDigito == "1" ||

        (11 - (vnTotal % 11)).Equals(vDigito))

        return true;

    else

        return false;

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
