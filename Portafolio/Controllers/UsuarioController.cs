using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;

namespace Portafolio.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {  
            return View(); 
        }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Registro(RegistroViewModel modelo)
    {

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var usuario = new Usuario() { EmailNormalizado = modelo.Email };
        var resultado = await userManager.CreateAsync(usuario, modelo.Password);

        if (resultado.Succeeded)
        {
                await signInManager.SignInAsync(usuario, isPersistent: true);
                return RedirectToAction("Index", "Proyecto");
        }

        foreach(var error in resultado.Errors) 
        {
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(modelo);
    }

        [HttpPost]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(IniciarSesionViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, isPersistent: true, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Proyecto");
            }
            ModelState.AddModelError(string.Empty, "Inicio de sesion fallido");
            return View(modelo);
        }
    }
}
