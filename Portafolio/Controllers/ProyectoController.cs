using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IProyectoUtilidades proyectoUtilidades;
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioTecnologia repositorioTecnologia;

        public ProyectoController(IRepositorioProyecto repositorioProyecto, IRepositorioUsuario repositorioUsuario, ICloudinaryService cloudinaryService,
            IRepositorioImagenProyecto repositorioImagenProyecto, IProyectoUtilidades proyectoUtilidades, IRepositorioCategoria repositorioCategoria, IRepositorioTecnologia repositorioTecnologia)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioUsuario = repositorioUsuario;
            this.cloudinaryService = cloudinaryService;
            this.repositorioImagenProyecto = repositorioImagenProyecto;
            this.proyectoUtilidades = proyectoUtilidades;
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioTecnologia = repositorioTecnologia;
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

        public async Task<IActionResult> Crear() 
        {
            Proyecto proyecto = new Proyecto();
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var categorias = await repositorioCategoria.ObtenerCategorias(UsuarioID);
            var tecnologias = await repositorioTecnologia.ObtenerTecnologias(UsuarioID);

            ViewBag.Tecnologias = tecnologias;
            ViewBag.Categorias = new SelectList(categorias, "CategoriaID", "Nombre");
            return View("CrearEditar", proyecto);   
        }

        
        [HttpPost]
        public async Task<IActionResult> Crear(Proyecto proyecto, IEnumerable<IFormFile> imagenes, int[] tecnologiasSeleccionadas) 
        {

            if (!ModelState.IsValid) 
            {
              return View(proyecto);
            }

            proyecto.UsuarioID = await repositorioUsuario.ObtenerUsuario();
            proyecto.Descripcion = proyectoUtilidades.LimpiarInputHTML(proyecto.Descripcion);
            proyecto.FechaPubli = new DateTime();

            return View();
        }
        
    }
}
