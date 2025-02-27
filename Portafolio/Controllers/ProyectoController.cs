using Microsoft.AspNetCore.Authorization;
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
        private readonly ICloudinaryService cloudinaryService;
        private readonly IRepositorioImagenProyecto repositorioImagenProyecto;
        private readonly IProyectoUtilidades proyectoUtilidades;
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioTecnologia repositorioTecnologia;
        private readonly IRepositorioTecnologiaUsada repositorioTecnologiaUsada;
        private readonly IServicioUsuario servicioUsuario;

        public ProyectoController(IRepositorioProyecto repositorioProyecto, ICloudinaryService cloudinaryService,
            IRepositorioImagenProyecto repositorioImagenProyecto, IProyectoUtilidades proyectoUtilidades, IRepositorioCategoria repositorioCategoria, IRepositorioTecnologia repositorioTecnologia,
            IRepositorioTecnologiaUsada repositorioTecnologiaUsada, IServicioUsuario servicioUsuario)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.cloudinaryService = cloudinaryService;
            this.repositorioImagenProyecto = repositorioImagenProyecto;
            this.proyectoUtilidades = proyectoUtilidades;
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioTecnologia = repositorioTecnologia;
            this.repositorioTecnologiaUsada = repositorioTecnologiaUsada;
            this.servicioUsuario = servicioUsuario;
        }

        /// <summary>
        /// Renderiza una vista con todos los proyectos pertenecientes al usuario
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            int UsuarioID = servicioUsuario.ObtenerUsuarioId();
            IEnumerable<Proyecto> proyectos = await repositorioProyecto.ObtenerProyectos(UsuarioID);
            return View(proyectos);
        }

        /// <summary>
        /// Renderiza una vista para crear un nuevo proyecto en la base de datos
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Crear() 
        {
                Proyecto proyecto = new Proyecto();
                var UsuarioID = servicioUsuario.ObtenerUsuarioId();
                var categorias = await repositorioCategoria.ObtenerCategoriasActivas(UsuarioID);
                var tecnologias = await repositorioTecnologia.ObtenerTecnologiasActivas(UsuarioID);
                
                ViewBag.Tecnologias = tecnologias;
                ViewBag.TecnologiasUsadas = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(proyecto.ProyectoID, UsuarioID);
                ViewBag.Categorias = new SelectList(categorias, "CategoriaID", "Nombre");
                return View("CrearEditar", proyecto);
        }
        /// <summary>
        /// Carga en la base de datos un nuevo proyecto
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="imagenes"></param>
        /// <param name="tecnologiasSeleccionadas"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Crear(Proyecto proyecto, IEnumerable<IFormFile> imagenes, int[] tecnologiasSeleccionadas) 
        {
            var UsuarioID = servicioUsuario.ObtenerUsuarioId();

            // Se carga el proyecto
            proyecto.UsuarioID = UsuarioID;
            proyecto.Descripcion = proyectoUtilidades.LimpiarInputHTML(proyecto.Descripcion);
            proyecto.FechaPubli = DateTime.Now;

            if (!ModelState.IsValid) 
            {
              return View("CrearEditar", proyecto);
            }
           
            var ProyectoID  = await repositorioProyecto.Crear(proyecto);

            //Se cargan las tecnologias seleccionadas.
            await GuardarTecnologiaUsada(tecnologiasSeleccionadas, ProyectoID, UsuarioID);

            //Se cargan las imagenes
            await CargarImagenes(imagenes, ProyectoID, UsuarioID);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Muestra la vista para editar un proyecto existente.
        /// </summary>
        /// <param name="ProyectoID">Identificador del proyecto a editar.</param>
        /// <returns>Una vista para editar la tecnología si existe, o una vista de error si no se encuentra.</returns>
        public async Task<IActionResult> Editar(int ProyectoID)
        {
            var UsuarioID = servicioUsuario.ObtenerUsuarioId();
            Proyecto proyecto = await repositorioProyecto.ObtenerProyectoPorID(ProyectoID, UsuarioID);
            int categoriaSeleccionada = proyecto.CategoriaID;
            var categorias = await repositorioCategoria.ObtenerCategoriasActivas(UsuarioID);

            if (proyecto == null)
            {
                return View("Error404");
            }

            ViewBag.Tecnologias = await repositorioTecnologia.ObtenerTecnologiasActivas(UsuarioID);
            ViewBag.TecnologiasUsadas = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(ProyectoID, UsuarioID);
            ViewBag.Categorias = new SelectList(categorias, "CategoriaID", "Nombre", categoriaSeleccionada);
            ViewBag.Imagenes = await repositorioImagenProyecto.ObtenerImagenesProyecto(ProyectoID, UsuarioID);
            return View("CrearEditar", proyecto);
        }

        /// <summary>
        /// Actualiza un proyecto existente en el sistema.
        /// </summary>
        /// <param name="proyecto">Instancia de la clase proyecto con la información modificada.</param>
        /// <returns>Redirige a la lista de proyectos si la edición es exitosa, o muestra una vista de error si el modelo es inválido.</returns>
        
        [HttpPost]
        public async Task<IActionResult> Editar(Proyecto proyecto, int[] tecnologiasSeleccionadas, IEnumerable<IFormFile> imagenes, string[] publicIDs)
        {
            var UsuarioID = servicioUsuario.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                return View(proyecto);
            }

            await ActualizarTecnologiasUsadas(tecnologiasSeleccionadas, proyecto.ProyectoID, UsuarioID);

            await CargarImagenes(imagenes, proyecto.ProyectoID, UsuarioID);

            if (publicIDs != null) {
                foreach(var publicID in publicIDs) 
                {
                    await BorrarImagen(publicID,UsuarioID);
                }
            }

            await repositorioProyecto.EditarProyecto(proyecto);

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Muestra una vista de confirmación para eliminar un proyecto.
        /// </summary>
        /// <param name="ProyectoID">Identificador del proyecto a eliminar.</param>
        /// <returns>Una vista de confirmación si el proyecto existe, o una vista de error si no se encuentra.</returns>
        public async Task<IActionResult> ConfirmarEliminar(int ProyectoID)
        {
            int UsuarioID = servicioUsuario.ObtenerUsuarioId();
            Proyecto proyecto = await repositorioProyecto.ObtenerProyectoPorID(ProyectoID, UsuarioID); 

            if (proyecto == null)
            {
                return View("Error404");
            }

            return View("_Partials/_Confirmar", proyecto);
        }

        /// <summary>
        /// Elimina un proyecto del sistema.
        /// </summary>
        /// <param name="ProyectoID">Identificador del proyecto a eliminar.</param>
        /// <returns>Redirige a la lista de proyectos si la eliminación es exitosa, o a una vista de error si no se encuentra.</returns>
        [HttpPost]
        public async Task<IActionResult> Eliminar(int ProyectoID)
        {
            int UsuarioID = servicioUsuario.ObtenerUsuarioId();
            Proyecto proyecto = await repositorioProyecto.ObtenerProyectoPorID(ProyectoID, UsuarioID);

            if (proyecto == null)
            {
                return View("Error404");
            }

            await repositorioProyecto.EliminarProyecto(ProyectoID, UsuarioID);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Elimina una imagen del servicio de cloudnary y de la base de datos
        /// </summary>
        /// <param name="publicID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        [HttpPost]
        private async Task BorrarImagen(string publicID, int UsuarioID)
        {
            try
            {
                await cloudinaryService.ElimanarImagenAsync(publicID);
                await repositorioImagenProyecto.EliminarImagenProyecto(publicID, UsuarioID);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Verifica si una categoría con un nombre específico ya existe para el usuario.
        /// </summary>
        /// <param name="nombre">Nombre de la categoría a verificar.</param>
        /// <returns>Un valor booleano en formato JSON que indica si la categoría ya existe.</returns>
        
        /*
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
        */

        /// <summary>
        /// Se encarga de registrar las imagenes en la base de datos y subirlas a Clodinary
        /// </summary>
        /// <param name="imagenes"></param>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        private async Task CargarImagenes(IEnumerable<IFormFile> imagenes, int ProyectoID, int UsuarioID)
        {
            if (imagenes == null || !imagenes.Any())
            {
                return;
            }

            int orden = 0;
            var tareas = new List<Task>(); // Lista para manejar tareas en paralelo

            foreach (var imagen in imagenes)
            {
                if (imagen != null && imagen.Length > 0) // Validación manual
                {
                    tareas.Add(ProcesarImagen(imagen, UsuarioID, ProyectoID, orden++));
                }
            }

            await Task.WhenAll(tareas); // Ejecutar todas las cargas en paralelo
        }

        /// <summary>
        /// Se encarga de actualizar las tecnologias al momento de editar un proyecto
        /// </summary>
        /// <param name="tecnologiasSeleccionadas"></param>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        private async Task ActualizarTecnologiasUsadas(int[] tecnologiasSeleccionadas, int ProyectoID, int UsuarioID) {

            await LimpiarTecnologias(ProyectoID, UsuarioID);
            await GuardarTecnologiaUsada(tecnologiasSeleccionadas, ProyectoID, UsuarioID);
        }

        /// <summary>
        /// Se encarga de procesar las imagenes para luego ser cargadas en la base de datos y cloudinary, este metodo se llama en el metodo CargarImagenes 
        /// </summary>
        /// <param name="imagen"></param>
        /// <param name="usuarioID"></param>
        /// <param name="ProyectoID"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        private async Task ProcesarImagen(IFormFile imagen, int usuarioID, int ProyectoID, int orden)
        {
            try
            {
                using var stream = imagen.OpenReadStream();
                var imagenSubida = await cloudinaryService.SubirImagenAsyc(stream, imagen.FileName);

                var imagenASubir = new ImagenProyecto
                {
                    URL = imagenSubida.Url.ToString(),
                    Estado = true,
                    Orden = orden,
                    UsuarioID = usuarioID,
                    PublicID = imagenSubida.PublicId,
                    ProyectoID = ProyectoID,
                };

                await repositorioImagenProyecto.Crear(imagenASubir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir imagen {imagen.FileName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Guarda las tecnologias del formulario en la base de datos.
        /// </summary>
        /// <param name="tecnologiasSeleccionadas"></param>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        private async Task GuardarTecnologiaUsada(int[] tecnologiasSeleccionadas, int ProyectoID, int UsuarioID) 
        {
            foreach (var TecnologiaID in tecnologiasSeleccionadas)
            {
                await repositorioTecnologiaUsada.Crear(ProyectoID, TecnologiaID, UsuarioID);
            }
        }

        /// <summary>
        /// Borra todas las tecnologias de un proyecto
        /// </summary>
        /// <param name="ProyectoID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        private async Task LimpiarTecnologias(int ProyectoID,int UsuarioID) {
            var tecnologiasPrevias = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(ProyectoID, UsuarioID);

            foreach (var tecnologia in tecnologiasPrevias)
            {
                await repositorioTecnologiaUsada.EliminarTecnologia(tecnologia.TecnologiaID, ProyectoID, UsuarioID);
            }
        }
    }
}
