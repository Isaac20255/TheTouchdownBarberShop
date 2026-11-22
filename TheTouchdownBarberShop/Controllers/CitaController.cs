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

        private bool EsStaff =>
            User.Identity?.IsAuthenticated == true &&
            (User.IsInRole("Administrador") || User.IsInRole("Encargado"));

        private static int? ResolverEncargado(List<(int IdUsuario, string Descripcion)> encargados, int? idEncargado)
        {
            if (encargados.Count == 0) return null;

            if (idEncargado.HasValue && encargados.Any(e => e.IdUsuario == idEncargado.Value))
                return idEncargado.Value;

            return encargados[0].IdUsuario;
        }

        public async Task<IActionResult> Index(int? idEncargado)
        {
            if (!EsStaff)
            {
                ViewBag.Encargados = new List<SelectListItem>();
                ViewBag.IdEncargado = null;

                if (User.Identity?.IsAuthenticated != true)
                    return View(new List<CitaDTO>());

                var encargados = await _serviceCita.ListEncargadosAsync();
                var todas = new List<CitaDTO>();
                foreach (var enc in encargados)
                {
                    var citasEnc = await _serviceCita.ListAsync(enc.IdUsuario);
                    todas.AddRange(citasEnc);
                }

                var nombreUsuario = User.Identity?.Name;
                var citasCliente = todas
                    .Where(c => !string.IsNullOrWhiteSpace(nombreUsuario) &&
                                string.Equals(c.NombreCliente, nombreUsuario, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return View(citasCliente);
            }

            var encargadosStaff = await _serviceCita.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargadosStaff, idEncargado);

            ViewBag.Encargados = encargadosStaff
                .Select(e => new SelectListItem(e.Descripcion, e.IdUsuario.ToString(), e.IdUsuario == idValido))
                .ToList();
            ViewBag.IdEncargado = idValido;

            if (idValido is null)
                return View(new List<CitaDTO>());

            var lista = await _serviceCita.ListAsync(idValido.Value);
            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Filtrar(int? idEncargado, string? cliente, DateTime? fecha)
        {
            if (!EsStaff)
                return Forbid();

            var encargadosStaff = await _serviceCita.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargadosStaff, idEncargado);
            if (idValido is null)
                return Json(Array.Empty<object>());

            var filtradas = await _serviceCita.ListAsync(idValido.Value, cliente, fecha);

            var resultado = filtradas.Select(c => new
            {
                c.IdCita,
                fechaHora = c.FechaHora.ToString("dd/MM/yyyy HH:mm"),
                c.NombreCliente,
                c.NombreBarbero,
                c.Estado,
                c.Sucursal
            });

            return Json(resultado);
        }

        public async Task<IActionResult> Details(int id, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var encargados = await _serviceCita.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargados, idEncargado);
            if (idValido is null)
                return NotFound();

            var cita = await _serviceCita.FindByIdAsync(id, idValido.Value);
            if (cita is null)
                return NotFound();

            ViewBag.IdEncargado = idValido;
            return View(cita);
        }
    }
}