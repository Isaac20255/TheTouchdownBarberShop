using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTouchdownBarberShop.Application.DTOs;
using TheTouchdownBarberShop.Application.Services.Interfaces;

namespace TheTouchdownBarberShop.Web.Controllers
{
    public class CitaController : Controller
    {
        private readonly IServiceCita _serviceCita;

        // Preguntas que se hacen al cliente al registrar la reserva (mínimo 3 respondidas).
        private static readonly string[] PreguntasCliente =
        {
            "¿Qué estilo de corte desea?",
            "¿Tiene alguna alergia o sensibilidad en la piel?",
            "¿Es su primera visita a la barbería?",
            "¿Desea algún producto adicional?"
        };

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

        private async Task<int?> ResolverEncargadoAsync(int? idEncargado)
        {
            var encargados = await _serviceCita.ListEncargadosAsync();
            return ResolverEncargado(encargados, idEncargado);
        }

        // Agrega al diccionario todas las preguntas del formulario, conservando las respuestas existentes.
        private static void PrepararRespuestas(CitaDTO dto)
        {
            foreach (var pregunta in PreguntasCliente)
            {
                if (!dto.Respuestas.ContainsKey(pregunta))
                    dto.Respuestas[pregunta] = null;
            }
        }

        // Los campos de solo lectura del DTO no llegan en el formulario y no deben invalidar el ModelState.
        private void LimpiarModelState()
        {
            ModelState.Remove(nameof(CitaDTO.NombreCliente));
            ModelState.Remove(nameof(CitaDTO.NombreBarbero));
            ModelState.Remove(nameof(CitaDTO.Estado));
            ModelState.Remove(nameof(CitaDTO.Sucursal));
        }

        private async Task CargarCombosAsync(CitaDTO dto, int idEncargado)
        {
            var clientes = await _serviceCita.ListClientesAsync();
            var barberos = await _serviceCita.ListBarberosAsync();
            var paquetes = await _serviceCita.ListPaquetesAsync();
            var servicios = await _serviceCita.ListServiciosAsync();

            ViewBag.IdEncargado = idEncargado;

            ViewBag.Clientes = clientes
                .Select(c => new SelectListItem(c.Descripcion, c.Id.ToString(), c.Id == dto.IdCliente))
                .ToList();

            ViewBag.Barberos = barberos
                .Select(b => new SelectListItem(b.Descripcion, b.Id.ToString(), b.Id == dto.IdBarbero))
                .ToList();

            ViewBag.Paquetes = paquetes
                .Select(p => new SelectListItem(p.Descripcion, p.Id.ToString(), p.Id == dto.IdPaquete))
                .ToList();

            ViewBag.Servicios = servicios
                .Select(s => new SelectListItem(s.Descripcion, s.Id.ToString(), dto.IdsServicios.Contains(s.Id)))
                .ToList();
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

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            var cita = await _serviceCita.FindByIdAsync(id, idValido.Value);
            if (cita is null)
                return NotFound();

            ViewBag.IdEncargado = idValido;
            return View(cita);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            var dto = new CitaDTO { FechaHora = DateTime.Today.AddHours(DateTime.Now.Hour + 1) };
            PrepararRespuestas(dto);

            await CargarCombosAsync(dto, idValido.Value);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaDTO dto, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            LimpiarModelState();

            if (!ModelState.IsValid)
            {
                PrepararRespuestas(dto);
                await CargarCombosAsync(dto, idValido.Value);
                return View(dto);
            }

            var idNueva = await _serviceCita.AddAsync(dto, idValido.Value);
            if (idNueva is null)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la cita. Verifique que los servicios seleccionados sean válidos.");
                PrepararRespuestas(dto);
                await CargarCombosAsync(dto, idValido.Value);
                return View(dto);
            }

            TempData["Success"] = "La cita se registró correctamente.";
            return RedirectToAction(nameof(Details), new { id = idNueva.Value, idEncargado = idValido.Value });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            var dto = await _serviceCita.FindByIdAsync(id, idValido.Value);
            if (dto is null)
                return NotFound();

            PrepararRespuestas(dto);
            await CargarCombosAsync(dto, idValido.Value);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitaDTO dto, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            if (id != dto.IdCita)
                return BadRequest();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            LimpiarModelState();

            if (!ModelState.IsValid)
            {
                PrepararRespuestas(dto);
                await CargarCombosAsync(dto, idValido.Value);
                return View(dto);
            }

            var actualizada = await _serviceCita.UpdateAsync(dto, idValido.Value);
            if (!actualizada)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar la cita. Verifique que exista y que los servicios seleccionados sean válidos.");
                PrepararRespuestas(dto);
                await CargarCombosAsync(dto, idValido.Value);
                return View(dto);
            }

            TempData["Success"] = "La cita se actualizó correctamente.";
            return RedirectToAction(nameof(Details), new { id = dto.IdCita, idEncargado = idValido.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            try
            {
                var eliminada = await _serviceCita.DeleteAsync(id, idValido.Value);
                if (!eliminada)
                    return NotFound();

                TempData["Success"] = "La cita se eliminó correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                // La cita tiene una factura asociada.
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id, idEncargado = idValido.Value });
            }

            return RedirectToAction(nameof(Index), new { idEncargado = idValido.Value });
        }
    }
}