using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly IServiceCita _serviceCita;

        public CitaController(IServiceCita serviceCita)
        {
            _serviceCita = serviceCita;
        }

        // Valida el encargado recibido contra los encargados de la BD;
        // si no es válido o no se envió, usa el primero registrado.
        private static int? ResolverEncargado(List<(int IdUsuario, string Descripcion)> encargados, int? idEncargado)
        {
            if (encargados.Count == 0) return null;

            if (idEncargado.HasValue && encargados.Any(e => e.IdUsuario == idEncargado.Value))
                return idEncargado.Value;

            return encargados[0].IdUsuario;
        }

        public async Task<IActionResult> Index(int? idEncargado)
        {
            var encargados = await _serviceCita.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargados, idEncargado);

            ViewBag.Encargados = encargados
                .Select(e => new SelectListItem(e.Descripcion, e.IdUsuario.ToString(), e.IdUsuario == idValido))
                .ToList();
            ViewBag.IdEncargado = idValido;

            if (idValido is null)
                return View(new List<CitaDTO>());

            var lista = await _serviceCita.ListAsync(idValido.Value);
            return View(lista);
        }

        public async Task<IActionResult> Details(int id, int? idEncargado)
        {
            var encargados = await _serviceCita.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargados, idEncargado);
            if (idValido is null) return NotFound();

            var cita = await _serviceCita.FindByIdAsync(id, idValido.Value);
            if (cita is null) return NotFound();

            ViewBag.IdEncargado = idValido;
            return View(cita);
        }
    }
}