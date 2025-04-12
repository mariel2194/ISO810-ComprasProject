using ISO810_ComprasProject.Data;
using ISO810_ComprasProject.Models;
using ISO810_ComprasProject.Services;
using ISO810_ComprasProject.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;

namespace ISO810_ComprasProject.Controllers
{
    [Authorize]
    public class TransaccionesController : Controller
    {
        private readonly ComprasDBContext _context;
        private readonly ContabilidadAuthService _authService;
        private readonly ContabilidadApiService _apiService;
        private readonly ContabilidadConfig _config;


        public TransaccionesController(ComprasDBContext context,
            ContabilidadAuthService authService,
            ContabilidadApiService apiService,
            ContabilidadConfig config)
        {
            _context = context;
            _authService = authService;
            _apiService = apiService;
            _config = config;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index()
        {
            var transacciones = await _context.Transaccion
                //.Include(t => t.)
                //.ThenInclude(o => o.Articulo)
                //.Include(t => t.OrdenCompra.Proveedor)
                .ToListAsync();

            return View(new TransaccionFiltroViewModel
            {
                Transacciones = transacciones.OrderByDescending(x => x.TransaccionId).ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta)
        {
            var query = _context.Transaccion.AsQueryable();

            if (desde.HasValue)
                query = query.Where(t => t.Fecha >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(t => t.Fecha <= hasta.Value);

            var resultado = await query.ToListAsync();

            return View(new TransaccionFiltroViewModel
            {
                Desde = desde,
                Hasta = hasta,
                Transacciones = resultado.OrderByDescending(x=>x.TransaccionId).ToList()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Contabilizar(string ids)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ids))
                    return BadRequest();

                var idList = ids.Split(',').Select(int.Parse).ToList();

                var transactions = _context.Transaccion.Where(d => idList.Contains(d.TransaccionId) && (d.AsientoId == null || d.AsientoId == 0)).ToList();

                if (transactions.Any())
                {
                    var authService = new ContabilidadAuthService(new HttpClient(), _config);
                    var (token, sistemaAuxiliarId) = await authService.ObtenerTokenAsync(_config.Usuario, _config.Clave);

                    var apiService = new ContabilidadApiService(new HttpClient(), _config);

                    var detalles = new List<object>();

                    var totalMonto = transactions.Sum(t => t.Monto);

                    var movimientosDebito = transactions.Select(t => new
                    {
                        cuentaId = 80, //Compras de mercancias
                        tipoMovimiento = "DB",
                        montoAsiento = t.Monto
                    }).ToList();

                    movimientosDebito.Add(new
                    {
                        cuentaId = 4, //Cuenta corriente del banco X
                        tipoMovimiento = "CR",
                        montoAsiento = totalMonto
                    });

                    // Armar el asiento contable único
                    var asiento = new
                    {
                        descripcion = "Compras correspondientes a " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                        sistemaAuxiliarId,
                        fechaAsiento = DateTime.Now,
                        detalles = movimientosDebito
                    };

                    // Enviar a la API
                    var (idAsiento, fechaAsiento) = apiService.EnviarAsientoAsync(token, asiento).Result;

                    if (idAsiento != 0)
                    {
                        transactions.ForEach(d => d.AsientoId = idAsiento);
                        transactions.ForEach(d => d.FechaAsiento = fechaAsiento);

                        _context.SaveChangesAsync();

                        TempData["ProcesoMensaje"] = "Las transacciones fueron contabilizadas y enviadas correctamente al modulo contable.";
                    }
                    else
                    {
                        TempData["ProcesoMensaje"] = "Error al contabilizar las transacciones";
                    }
                }
                else 
                {
                    TempData["ProcesoMensaje"] = "No hay transacciones pendientes para contabilizar";
                }
 
            }
            catch (Exception ex)
            {
                TempData["ProcesoMensaje"] = "Error al contabilizar las transacciones: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
