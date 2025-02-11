using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;

namespace Portafolio.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> userManager;

        public UsuarioController(UserManager<Usuario> userManager) 
        {
            this.userManager = userManager;
        }


        public IActionResult Registro()
        {  
            return View(); 
        }

    [HttpPost]
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
                return RedirectToAction("Index", "Proyecto");
        }

        foreach(var error in resultado.Errors) 
        {
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(modelo);
    }

   }
}
