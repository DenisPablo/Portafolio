using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    /// <summary>
    /// Se encarga de realizar la primera presentacion de la pagina.
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IRepositorioProyecto repositorioProyecto;
        private readonly IRepositorioTecnologiaUsada repositorioTecnologiaUsada;
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioImagenProyecto repositorioImagenProyecto;
        private readonly IRepositorioUsuario repositorioUsuario;

        public HomeController(IRepositorioProyecto repositorioProyecto, IRepositorioTecnologiaUsada repositorioTecnologiaUsada, IRepositorioCategoria repositorioCategoria, IRepositorioImagenProyecto repositorioImagenProyecto,IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioTecnologiaUsada = repositorioTecnologiaUsada;
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioImagenProyecto = repositorioImagenProyecto;
            this.repositorioUsuario = repositorioUsuario;
        }

        public async Task<IActionResult> Index() 
        {
            var ultimosProyectos = await repositorioProyecto.ObtenerUltimosProyectos();
            var descripcion = await repositorioUsuario.ObtenerDescripcionVista();
            List<ProyectoViewModel> ultimosProyectosViewModel = [];
            IEnumerable<ImagenProyecto> imagenes = [];


            foreach(var proyecto in ultimosProyectos) 
            {
                var tecnologiasUsadas = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(proyecto.ProyectoID, proyecto.UsuarioID);
                var categoria = await repositorioCategoria.ObtenerCategoriasPorID(proyecto.CategoriaID, proyecto.UsuarioID);

                ProyectoViewModel ultimoProyecto = new(proyecto.ProyectoID, proyecto.Titulo, proyecto.Descripcion, tecnologiasUsadas, proyecto.Antiguedad, categoria.Nombre, imagenes);
                ultimosProyectosViewModel.Add(ultimoProyecto);
            }

            ViewBag.Descripcion = descripcion.Descripcion;
            return View(ultimosProyectosViewModel);
        }

        public async Task<IActionResult> Proyectos()
        {
            List<ProyectoViewModel> proyectosViewModel = [];
            var proyectos = await repositorioProyecto.ObtenerProyectosVisitante();

            foreach (var proyecto in proyectos)
            {
                IEnumerable<Tecnologia> tecnologias = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(proyecto.ProyectoID, proyecto.UsuarioID);
                
                var categoria = await repositorioCategoria.ObtenerCategoriasPorID(proyecto.CategoriaID, proyecto.UsuarioID);
                IEnumerable<ImagenProyecto> Imagenes = [];

                ProyectoViewModel proyectoViewModel = new(proyecto.ProyectoID,proyecto.Titulo, proyecto.Descripcion, tecnologias, proyecto.Antiguedad, categoria.Nombre, Imagenes);

                proyectosViewModel.Add(proyectoViewModel);
              
            }


            return View(proyectosViewModel);
        }

        [Route("Proyectos/Detalles/{ProyectoID}")]
        public async Task<IActionResult> Detalles(int ProyectoID) 
        {
            var proyecto = await repositorioProyecto.ObtenerProyectoDetalle(ProyectoID);
            var categoria = await repositorioCategoria.ObtenerCategoriasPorID(proyecto.CategoriaID, proyecto.UsuarioID);
            var tecnologiasUsadas = await repositorioTecnologiaUsada.ObtenerTecnologiasProyectoDetalles(ProyectoID);
            var imagenes = await repositorioImagenProyecto.ObtenerImagenesProyecto(proyecto.ProyectoID, proyecto.UsuarioID);

            ProyectoViewModel proyectoViewModel = new(proyecto.ProyectoID, proyecto.Titulo, proyecto.Descripcion, tecnologiasUsadas, proyecto.Antiguedad, categoria.Nombre, imagenes);

            return View(proyectoViewModel);
        }
    }
}
