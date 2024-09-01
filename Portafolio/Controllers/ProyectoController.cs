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
        private readonly ICloudinaryService cloudinaryService;
        private readonly IRepositorioImagenProyecto repositorioImagenProyecto;

        public ProyectoController(IRepositorioProyecto repositorioProyecto, IRepositorioUsuario repositorioUsuario, ICloudinaryService cloudinaryService, IRepositorioImagenProyecto repositorioImagenProyecto)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioUsuario = repositorioUsuario;
            this.cloudinaryService = cloudinaryService;
            this.repositorioImagenProyecto = repositorioImagenProyecto;
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

        public IActionResult Crear() 
        {
            Proyecto proyecto = new Proyecto();
            return View("CrearEditar", proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proyecto proyecto, IEnumerable<IFormFile> imagenes) 
        {
            if (!ModelState.IsValid) 
            {
                return View("CrearEditar", proyecto);
            }

            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            proyecto.UsuarioID = UsuarioID;

            if (imagenes != null && imagenes.Any())
            {
            
            }
        }
    }
}
