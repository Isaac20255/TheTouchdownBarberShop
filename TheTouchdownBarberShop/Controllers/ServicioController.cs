using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServiceServicio _serviceServicio;

        public ServicioController(IServiceServicio serviceServicio)
        {
            _serviceServicio = serviceServicio;
        }

        // GET: /Servicio
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            ViewData["Title"] = "Servicios";
            ViewBag.Titulo = "Mantenimiento de Servicios";

            var collection = await _serviceServicio.ListAsync();
            return View(collection);
        }

        // GET: /Servicio/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var servicio = await _serviceServicio.FindByIdAsync(id.Value);

            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // GET: /Servicio/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categorias = await _serviceServicio.ListCategoriasAsync();
            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre");
            return View();
        }

        // POST: /Servicio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicioDTO servicioDto, IFormFile? imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                if (imagenArchivo != null && imagenArchivo.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await imagenArchivo.CopyToAsync(stream);
                    }

                    servicioDto.Imagen = nombreArchivo;
                }

                await _serviceServicio.AddAsync(servicioDto);
                TempData["Mensaje"] = "Servicio creado exitosamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _serviceServicio.ListCategoriasAsync();
            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", servicioDto.IdCategoria);
            return View(servicioDto);
        }

        // GET: /Servicio/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var servicio = await _serviceServicio.FindByIdAsync(id.Value);
            if (servicio == null)
            {
                return NotFound();
            }

            var categorias = await _serviceServicio.ListCategoriasAsync();
            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", servicio.IdCategoria);
            return View(servicio);
        }

        // POST: /Servicio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicioDTO servicioDto, IFormFile? imagenArchivo)
        {
            if (id != servicioDto.IdServicio)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (imagenArchivo != null && imagenArchivo.Length > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagenArchivo.FileName);
                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await imagenArchivo.CopyToAsync(stream);
                    }

                    servicioDto.Imagen = nombreArchivo;
                }

                await _serviceServicio.UpdateAsync(servicioDto);
                TempData["Mensaje"] = "Servicio actualizado exitosamente.";
                TempData["Tipo"] = "success";
                return RedirectToAction(nameof(Index));
            }

            var categorias = await _serviceServicio.ListCategoriasAsync();
            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", servicioDto.IdCategoria);
            return View(servicioDto);
        }

        // GET: /Servicio/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var servicio = await _serviceServicio.FindByIdAsync(id.Value);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: /Servicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceServicio.DeleteAsync(id);
            TempData["Mensaje"] = "Servicio eliminado exitosamente.";
            TempData["Tipo"] = "error";
            return RedirectToAction(nameof(Index));
        }
    }
}