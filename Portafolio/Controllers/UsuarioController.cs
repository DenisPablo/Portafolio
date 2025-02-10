using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;

namespace Portafolio.Controllers
{
    public class UsuarioController : Controller
    {

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

        return RedirectToAction("Index", "Proyecto");
    }

   }
}
