using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using System.Diagnostics;

namespace Portafolio.Controllers
{
    /// <summary>
    /// Se encarga de realizar la primera presentacion de la pagina.
    /// </summary>
    public class HomeController : Controller
    {

        public IActionResult Index() 
        {
            return View();
        }
    }
}
