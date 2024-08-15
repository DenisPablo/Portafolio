using Microsoft.AspNetCore.Mvc;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    public class TecnologiaController : Controller
    {
        private readonly IRepositorioTecnologia repositorioTecnologia;
        private readonly IRepositorioUsuario repositorioUsuario;

        public TecnologiaController(IRepositorioTecnologia repositorioTecnologia, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioTecnologia = repositorioTecnologia;
            this.repositorioUsuario = repositorioUsuario;
        }

        public async Task<IActionResult> Index()
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var tecnologias = await repositorioTecnologia.ObtenerTecnologias(UsuarioID);

            return View(tecnologias);
        }
    }
}
