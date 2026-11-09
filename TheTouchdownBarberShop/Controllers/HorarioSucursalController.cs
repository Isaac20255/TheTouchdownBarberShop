using Microsoft.AspNetCore.Mvc;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Web.Controllers
{
    public class HorarioSucursalController : Controller
    {
        private readonly IServiceHorarioSucursal _service;

        public HorarioSucursalController(IServiceHorarioSucursal service)
        {
            _service = service;
        }

        // GET: HorarioSucursal?idSucursal=1
        public async Task<IActionResult> Index(int idSucursal)
        {
            ViewBag.IdSucursal = idSucursal;
            var lista = await _service.ListBySucursalAsync(idSucursal);
            return View(lista);
        }

        // GET: HorarioSucursal/Details
        public async Task<IActionResult> Details(int id)
        {
            var horario = await _service.FindByIdAsync(id);
            if (horario == null) return NotFound();
            return View(horario);
        }

        // GET: HorarioSucursal/Create?idSucursal=1
        [HttpGet]
        public IActionResult Create(int idSucursal)
        {
            var dto = new HorarioSucursalDTO { IdSucursal = idSucursal };
            return View(dto);
        }

        // POST: HorarioSucursal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HorarioSucursalDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _service.AddAsync(dto);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo guardar el horario: " + (ex.InnerException?.Message ?? ex.Message));
                return View(dto);
            }

            return RedirectToAction(nameof(Index), new { idSucursal = dto.IdSucursal });
        }
    }
}