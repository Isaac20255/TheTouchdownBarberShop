using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _serviceProducto;

        public ProductoController(IServiceProducto serviceProducto)
        {
            _serviceProducto = serviceProducto;
        }

        // GET: /Producto
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Recibir mensaje enviado desde otra acción mediante TempData
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Productos";
            ViewBag.Titulo = "Lista de Productos";

            var collection = await _serviceProducto.ListAsync();
            return View(collection);
        }

        // GET: /Producto/Enviar
        public IActionResult Enviar()
        {
            TempData["Mensaje"] = Web.Util.SweetAlertHelper.Mensaje("Enviar", "Mensaje enviado", Web.Util.SweetAlertMessageType.success);
            return RedirectToAction("Index");
        }

        // GET: /Producto/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var producto = await _serviceProducto.FindByIdAsync(id.Value);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: /Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: /Producto/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: /Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: /Producto/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: /Producto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
