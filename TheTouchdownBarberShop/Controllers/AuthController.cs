using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheTouchdownBarberShop.Application.Services.Interfaces;
using TheTouchdownBarberShop.Models;

namespace TheTouchdownBarberShop.Controllers;

public class AuthController : Controller
{
    private readonly IServiceUsuario _serviceUsuario;

    public AuthController(IServiceUsuario serviceUsuario)
    {
        _serviceUsuario = serviceUsuario;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var usuario = await _serviceUsuario.ValidarCredencialesAsync(model.Correo, model.Contrasenia);

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo ?? string.Empty),
            new Claim(ClaimTypes.Role, usuario.NombreRol)
        };

        if (usuario.IdSucursal.HasValue)
        {
            claims.Add(new Claim("IdSucursal", usuario.IdSucursal.Value.ToString()));
        }

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identity));

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}