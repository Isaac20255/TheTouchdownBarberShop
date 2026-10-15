using Microsoft.AspNetCore.Mvc;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IServiceFactura _serviceFactura;

        public FacturaController(IServiceFactura serviceFactura)
        {
            _serviceFactura = serviceFactura;
        }

        // GET: /Factura
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Recibir mensaje enviado desde otra acción mediante TempData
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Facturas";
            ViewBag.Titulo = "Historial de Facturación";

            var collection = await _serviceFactura.ListAsync();
            return View(collection);
        }

        // GET: /Factura/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var factura = await _serviceFactura.FindByIdAsync(id.Value);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }
    }
}