using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
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

        public IActionResult Crear() 
        {
            Tecnologia tecnologia = new Tecnologia();
            return View("CrearEditar", tecnologia);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Tecnologia tecnologia) 
        {
            if (!ModelState.IsValid) 
            {
                return View("Error404");
            }

            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            
            tecnologia.Estado = true;
            tecnologia.UsuarioID = UsuarioID;

            await repositorioTecnologia.Crear(tecnologia);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int TecnologiaID) 
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Tecnologia tecnologia = await repositorioTecnologia.ObtenerTecnologiasPorID(TecnologiaID, UsuarioID);

            if (tecnologia == null) {
                return View("Error404");
            }

            await repositorioTecnologia.EliminarTecnologia(TecnologiaID, UsuarioID);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmarEliminar(int TecnologiaID) 
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Tecnologia tecnologia = await repositorioTecnologia.ObtenerTecnologiasPorID(TecnologiaID, UsuarioID);

            if (tecnologia == null) 
            {
                return View("Error404");
            }

            return View("_Partials/_Confirmar", tecnologia);
        }

        public async Task<IActionResult> Editar(int TecnologiaID) 
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Tecnologia tecnologia = await repositorioTecnologia.ObtenerTecnologiasPorID(TecnologiaID, UsuarioID);

            if (tecnologia == null)
            {
                return View("Error404");
            }

            return View("CrearEditar", tecnologia);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Tecnologia tecnologia) 
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            tecnologia.UsuarioID = UsuarioID;

            if (!ModelState.IsValid)
            {
                return View("Error404");
            }

            await repositorioTecnologia.EditarTecnologia(tecnologia);
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExistenciaTecnologia(string nombre)
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var yaExisteTecnologia = await repositorioTecnologia.ExisteTecnologia(nombre, UsuarioID);

            if (yaExisteTecnologia)
            {
                return Json($"El nombre ya existe");
            }

            return Json(true);
        }
    }
}
