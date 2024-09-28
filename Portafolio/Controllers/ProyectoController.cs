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
        private readonly IRepositorioTecnologiaUsada repositorioTecnologiaUsada;

        public ProyectoController(IRepositorioProyecto repositorioProyecto, IRepositorioUsuario repositorioUsuario, ICloudinaryService cloudinaryService,
            IRepositorioImagenProyecto repositorioImagenProyecto, IProyectoUtilidades proyectoUtilidades, IRepositorioCategoria repositorioCategoria, IRepositorioTecnologia repositorioTecnologia,
            IRepositorioTecnologiaUsada repositorioTecnologiaUsada)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioUsuario = repositorioUsuario;
            this.cloudinaryService = cloudinaryService;
            this.repositorioImagenProyecto = repositorioImagenProyecto;
            this.proyectoUtilidades = proyectoUtilidades;
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioTecnologia = repositorioTecnologia;
            this.repositorioTecnologiaUsada = repositorioTecnologiaUsada;
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
            // Se carga el proyecto
            proyecto.UsuarioID = await repositorioUsuario.ObtenerUsuario();
            proyecto.Descripcion = proyectoUtilidades.LimpiarInputHTML(proyecto.Descripcion);
            proyecto.FechaPubli = DateTime.Now;
            var ProyectoID  = await repositorioProyecto.Crear(proyecto);

            //Se cargan las tecnologias seleccionadas.
            foreach (var TecnologiaID in tecnologiasSeleccionadas) 
            {
                var UsuarioID = await repositorioUsuario.ObtenerUsuario();
                await repositorioTecnologiaUsada.Crear(ProyectoID, TecnologiaID, UsuarioID);
            }

            //Se cargan las imagenes del proyecto.
            int orden = 0;
            foreach (var imagen in imagenes)
            {
                if (imagen != null && imagen.Length > 0)
                {

                    using (var stream = imagen.OpenReadStream())
                    {
                        var imagenSubida = await cloudinaryService.SubirImagenAsyc(stream, imagen.FileName);
                        var UsuarioID = await repositorioUsuario.ObtenerUsuario();
                        ImagenProyecto imagenASubir = new ImagenProyecto
                        {
                            URL = imagenSubida.Url.ToString(),
                            Estado = true,
                            Orden = orden++,
                            UsuarioID = UsuarioID,
                            PublicID = imagenSubida.PublicId,
                            ProyectoID = ProyectoID,
                        };
                        await repositorioImagenProyecto.Crear(imagenASubir);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifica si una categoría con un nombre específico ya existe para el usuario.
        /// </summary>
        /// <param name="nombre">Nombre de la categoría a verificar.</param>
        /// <returns>Un valor booleano en formato JSON que indica si la categoría ya existe.</returns>
        [HttpGet]
        public async Task<IActionResult> VerificarExistenciaProyecto(string titulo)
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var yaExisteProyecto = await repositorioProyecto.ExisteProyecto(titulo, UsuarioID);

            if (yaExisteProyecto)
            {
                return Json($"El nombre ya existe");
            }

            return Json(true);
        }

    }
}
