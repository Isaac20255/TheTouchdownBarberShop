using Microsoft.AspNetCore.Mvc;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IServiceProducto _service;
        private readonly IWebHostEnvironment _env;

        public ProductoController(IServiceProducto service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var lista = await _service.ListAsync();
            return View(lista);
        }

        // GET: Producto/Details
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _service.FindByIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // GET: Producto/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = await _service.ListCategoriasAsync();
            return View(new ProductoDTO { Activo = true });
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoDTO dto)
        {
            ModelState.Remove("CategoriaNombre");

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _service.ListCategoriasAsync();
                return View(dto);
            }

            try
            {
                await _service.AddAsync(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo guardar el producto: " + (ex.InnerException?.Message ?? ex.Message));
                ViewBag.Categorias = await _service.ListCategoriasAsync();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Producto/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _service.FindByIdAsync(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = await _service.ListCategoriasAsync();
            return View(producto);
        }

        // POST: Producto/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoDTO dto, IFormFile? imagenArchivo)
        {
            if (id != dto.IdProducto) return BadRequest();

            ModelState.Remove("CategoriaNombre");

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _service.ListCategoriasAsync();
                return View(dto);
            }

            // Si se seleccionó una imagen nueva, se guarda y se reemplaza el nombre.
            // Si no, dto.Imagen conserva el nombre actual que llega por el campo hidden.
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                var extension = Path.GetExtension(imagenArchivo.FileName).ToLowerInvariant();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
                {
                    ModelState.AddModelError(string.Empty, "Formato de imagen no permitido. Use JPG, PNG o WEBP.");
                    ViewBag.Categorias = await _service.ListCategoriasAsync();
                    return View(dto);
                }

                var nombreArchivo = Guid.NewGuid() + extension;
                var ruta = Path.Combine(_env.WebRootPath, "images", nombreArchivo);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagenArchivo.CopyToAsync(stream);
                }

                dto.Imagen = nombreArchivo;
            }

            try
            {
                await _service.UpdateAsync(dto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar el producto: " + (ex.InnerException?.Message ?? ex.Message));
                ViewBag.Categorias = await _service.ListCategoriasAsync();
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Producto/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _service.FindByIdAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // POST: Producto/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (Exception)
            {
                var producto = await _service.FindByIdAsync(id);
                if (producto == null) return NotFound();

                ModelState.AddModelError(string.Empty,
                    "No se puede eliminar el producto porque está asociado a una factura.");
                return View(producto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}