using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTouchdownBarberShop.Application.DTO;
using TheTouchdownBarberShop.Application.Services.Interfaces;
namespace TheTouchdownBarberShop.Controllers
{
    public class SucursalController : Controller
    {
        private readonly IServiceSucursal _serviceSucursal;
        private readonly IRepositoryUsuario _usuarioRepository;
        private readonly IServiceHorarioSucursal _serviceHorario;

        public SucursalController(IServiceSucursal serviceSucursal, IRepositoryUsuario usuarioRepository, IServiceHorarioSucursal serviceHorario)
        {
            _serviceSucursal = serviceSucursal;
            _usuarioRepository = usuarioRepository;
            _serviceHorario = serviceHorario;
        }

        // GET: /Sucursal
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }
            if (TempData.ContainsKey("Error"))
            {
                ViewBag.ErrorMessage = TempData["Error"];
            }
            ViewData["Title"] = "Sucursales";
            ViewBag.Titulo = "Lista de Sucursales";
            var collection = await _serviceSucursal.ListAsync();

            foreach (var sucursal in collection)
            {
                sucursal.EstaAbierta = await _serviceHorario.EstaAbiertaAsync(sucursal.IdSucursal);
            }

            return View(collection);
        }

        // GET: /Sucursal/Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sucursal = await _serviceSucursal.FindByIdAsync(id.Value);
            if (sucursal == null)
            {
                return NotFound();
            }

            var estaAbierta = await _serviceHorario.EstaAbiertaAsync(id.Value);
            if (!estaAbierta)
            {
                TempData["Error"] = "La sucursal está cerrada. Solo se puede acceder durante su horario de atención.";
                return RedirectToAction(nameof(Index));
            }

            return View(sucursal);
        }

        // GET: /Sucursal/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarAdministradoresAsync();
            return View();
        }

        // POST: /Sucursal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SucursalDTO sucursalDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceSucursal.AddAsync(sucursalDto);
                TempData["Mensaje"] = "Sucursal creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarAdministradoresAsync(sucursalDto.AdministradorId);
            return View(sucursalDto);
        }

        // GET: /Sucursal/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sucursal = await _serviceSucursal.FindByIdAsync(id.Value);
            if (sucursal == null)
            {
                return NotFound();
            }

            await CargarAdministradoresAsync(sucursal.AdministradorId);
            return View(sucursal);
        }

        // POST: /Sucursal/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SucursalDTO sucursalDto)
        {
            if (id != sucursalDto.IdSucursal)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _serviceSucursal.UpdateAsync(sucursalDto);
                TempData["Mensaje"] = "Sucursal actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarAdministradoresAsync(sucursalDto.AdministradorId);
            return View(sucursalDto);
        }

        // GET: /Sucursal/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var sucursal = await _serviceSucursal.FindByIdAsync(id.Value);
            if (sucursal == null)
            {
                return NotFound();
            }
            return View(sucursal);
        }

        // POST: /Sucursal/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceSucursal.DeleteAsync(id);
            TempData["Mensaje"] = "Sucursal eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarAdministradoresAsync(int? seleccionadoId = null)
        {
            var usuarios = await _usuarioRepository.ListAsync();
            var encargados = usuarios.Where(u => u.IdRolNavigation?.Nombre == "Encargado").ToList();
            ViewBag.Administradores = new SelectList(encargados, "IdUsuario", "Nombre", seleccionadoId);
        }
    }
}