using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        // GET: /Usuario
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Recibir mensaje enviado desde otra acción mediante TempData
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Usuarios";
            ViewBag.Titulo = "Lista de Usuarios (Isaac Acuña Araya)";

            var collection = await _serviceUsuario.ListAsync();
            return View(collection);
        }

        // GET: /Usuario/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var usuario = await _serviceUsuario.FindByIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalle de Usuario";
            return View(usuario);
        }

        // GET: /Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Usuario/Create
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

        // GET: /Usuario/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: /Usuario/Edit/5
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

        // GET: /Usuario/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: /Usuario/Delete/5
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