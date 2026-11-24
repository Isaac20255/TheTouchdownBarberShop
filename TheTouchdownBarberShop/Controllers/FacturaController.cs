using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TheTouchdownBarberShop.Application.DTO;
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
            var encargados = await _serviceFactura.ListEncargadosAsync();
            return ResolverEncargado(encargados, idEncargado);
        }

        private async Task CargarCatalogosAsync(int idEncargado)
        {
            ViewBag.IdEncargado = idEncargado;
            ViewBag.Clientes = await _serviceFactura.ListClientesAsync();
            ViewBag.Productos = await _serviceFactura.ListProductosAsync();
            ViewBag.Servicios = await _serviceFactura.ListServiciosAsync();
        }

        // Los datos de solo lectura del encabezado no viajan en el formulario: se vuelven a cargar.
        private async Task RestaurarEncabezadoAsync(FacturaDTO dto, int idEncargado)
        {
            var encabezado = await _serviceFactura.PrepararNuevaAsync(idEncargado);
            if (encabezado is not null)
            {
                dto.IdSucursal = encabezado.IdSucursal;
                dto.SucursalNombre = encabezado.SucursalNombre;
                dto.IdEncargado = encabezado.IdEncargado;
                dto.EncargadoNombre = encabezado.EncargadoNombre;
                dto.FechaEmision = encabezado.FechaEmision;
                dto.PorcentajeImpuesto = encabezado.PorcentajeImpuesto;
                dto.Estado = encabezado.Estado;
            }

            await CargarCatalogosAsync(idEncargado);
        }

        // GET: /Factura
        [HttpGet]
        public async Task<IActionResult> Index(int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            ViewData["Title"] = "Facturas";

            var encargados = await _serviceFactura.ListEncargadosAsync();
            var idValido = ResolverEncargado(encargados, idEncargado);

            ViewBag.Encargados = encargados
                .Select(e => new SelectListItem(e.Descripcion, e.IdUsuario.ToString(), e.IdUsuario == idValido))
                .ToList();
            ViewBag.IdEncargado = idValido;

            if (idValido is null)
                return View(new List<FacturaDTO>());

            var collection = await _serviceFactura.ListAsync(idValido.Value);
            return View(collection);
        }

        // GET: /Factura/Filtrar (filtro por cliente y/o fecha sin recargar la página)
        [HttpGet]
        public async Task<IActionResult> Filtrar(int? idEncargado, string? cliente, DateTime? fecha)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return Json(Array.Empty<object>());

            var filtradas = await _serviceFactura.ListAsync(idValido.Value, cliente, fecha);

            var resultado = filtradas.Select(f => new
            {
                f.IdFactura,
                fecha = f.FechaEmision.ToString("dd/MM/yyyy HH:mm"),
                f.ClienteNombre,
                f.Estado,
                f.Total,
                f.SucursalNombre
            });

            return Json(resultado);
        }

        // GET: /Factura/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            if (id == null)
                return BadRequest();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            var factura = await _serviceFactura.FindByIdAsync(id.Value, idValido.Value);
            if (factura == null)
                return NotFound();

            ViewBag.IdEncargado = idValido;
            return View(factura);
        }

        // GET: /Factura/Registrar            (factura nueva)
        // GET: /Factura/Registrar?idProforma=5 (factura desde una proforma)
        [HttpGet]
        public async Task<IActionResult> Registrar(int? idEncargado, int? idProforma)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            FacturaDTO? dto = idProforma.HasValue
                ? await _serviceFactura.PrepararDesdeProformaAsync(idProforma.Value, idValido.Value)
                : await _serviceFactura.PrepararNuevaAsync(idValido.Value);

            if (dto is null)
                return NotFound();

            await CargarCatalogosAsync(idValido.Value);
            return View(dto);
        }

        // POST: /Factura/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(FacturaDTO dto, int? idEncargado)
        {
            if (!EsStaff)
                return Forbid();

            var idValido = await ResolverEncargadoAsync(idEncargado);
            if (idValido is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await RestaurarEncabezadoAsync(dto, idValido.Value);
                return View(dto);
            }

            var idFactura = await _serviceFactura.RegistrarAsync(dto, idValido.Value);
            if (idFactura is null)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la factura. Verifique el cliente y los productos o servicios seleccionados.");
                await RestaurarEncabezadoAsync(dto, idValido.Value);
                return View(dto);
            }

            TempData["Success"] = "La factura se registró correctamente.";
            return RedirectToAction(nameof(Details), new { id = idFactura.Value, idEncargado = idValido.Value });
        }
    }
}