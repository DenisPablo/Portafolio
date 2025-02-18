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

        public HomeController(IRepositorioProyecto repositorioProyecto, IRepositorioTecnologiaUsada repositorioTecnologiaUsada, IRepositorioCategoria repositorioCategoria)
        {
            this.repositorioProyecto = repositorioProyecto;
            this.repositorioTecnologiaUsada = repositorioTecnologiaUsada;
            this.repositorioCategoria = repositorioCategoria;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public async Task<IActionResult> Proyectos()
        {
            List<ProyectoViewModel> proyectosViewModel = [];
            var proyectos = await repositorioProyecto.ObtenerProyectosVisitante();

            foreach (var proyecto in proyectos)
            {
                IEnumerable<Tecnologia> tecnologias = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(proyecto.ProyectoID, proyecto.UsuarioID);
                
                var categoria = await repositorioCategoria.ObtenerCategoriasPorID(proyecto.CategoriaID, proyecto.UsuarioID);


                ProyectoViewModel proyectoViewModel = new(proyecto.ProyectoID,proyecto.Titulo, proyecto.Descripcion, tecnologias, proyecto.Antiguedad, categoria.Nombre);

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

            ProyectoViewModel proyectoViewModel = new(proyecto.ProyectoID, proyecto.Titulo, proyecto.Descripcion, tecnologiasUsadas, proyecto.Antiguedad, categoria.Nombre);

            return View(proyectoViewModel);
        }
    }
}
