using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    /// <summary>
    /// Interconecta la vistas de proyectos con los servicios de las bases de datos
    /// </summary>
    public class ProyectoController : Controller
    {
        private readonly IRepositorioProyecto repositorioProyecto;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ProyectoController(IRepositorioProyecto repositorioProyecto, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioUsuario = repositorioUsuario;
        }

        /// <summary>
        /// Renderiza una vista con todos los proyectos pertenecientes al usuario
        /// </summary>
        /// <returns>retorna una vista con proyectos</returns>
        public async Task<IActionResult> Index()
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            IEnumerable<Proyecto> proyectos = await repositorioProyecto.ObtenerProyectos(UsuarioID);
            return View(proyectos);
        }
    }
}
