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
            List<ProyectoViewModel> proyectosViewModel = new();
            var proyectos = await repositorioProyecto.ObtenerProyectosVisitante();

            foreach (var proyecto in proyectos)
            {
                IEnumerable<Tecnologia> tecnologias = await repositorioTecnologiaUsada.ObtenerTecnologiasProyecto(proyecto.ProyectoID, proyecto.UsuarioID);
                
                var categoria = await repositorioCategoria.ObtenerCategoriasPorID(proyecto.CategoriaID, proyecto.UsuarioID);


                ProyectoViewModel proyectoViewModel = new(proyecto.Titulo, proyecto.Descripcion, tecnologias, proyecto.Antiguedad, categoria.Nombre);

                proyectosViewModel.Add(proyectoViewModel);
              
            }


            return View(proyectosViewModel);
        }
    }
}
