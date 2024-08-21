using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    /// <summary>
    /// Interconecta las vistas de tecnologia con los servicios de la base de datos
    /// </summary>
    public class TecnologiaController : Controller
    {
        private readonly IRepositorioTecnologia repositorioTecnologia;
        private readonly IRepositorioUsuario repositorioUsuario;

        public TecnologiaController(IRepositorioTecnologia repositorioTecnologia, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioTecnologia = repositorioTecnologia;
            this.repositorioUsuario = repositorioUsuario;
        }

        /// <summary>
        /// Muestra una lista de todas las tecnologías del usuario actual.
        /// </summary>
        /// <returns>Una vista con la lista de tecnologías.</returns>
        public async Task<IActionResult> Index()
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var tecnologias = await repositorioTecnologia.ObtenerTecnologias(UsuarioID);

            return View(tecnologias);
        }

        /// <summary>
        /// Muestra la vista para crear una nueva tecnología.
        /// </summary>
        /// <returns>Una vista con un modelo de tecnología vacío.</returns>
        public IActionResult Crear()
        {
            Tecnologia tecnologia = new Tecnologia();
            return View("CrearEditar", tecnologia);
        }

        /// <summary>
        /// Crea una nueva tecnología en el sistema.
        /// </summary>
        /// <param name="tecnologia">Instancia de la clase Tecnología con la información a crear.</param>
        /// <returns>Redirige a la lista de tecnologías si la creación es exitosa, o muestra una vista de error si el modelo es inválido.</returns>
        [HttpPost]
        public async Task<IActionResult> Crear(Tecnologia tecnologia)
        {
            if (!ModelState.IsValid)
            {
                return View("Error404");
            }

            var UsuarioID = await repositorioUsuario.ObtenerUsuario();

            tecnologia.UsuarioID = UsuarioID;

            await repositorioTecnologia.Crear(tecnologia);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Elimina una tecnología del sistema.
        /// </summary>
        /// <param name="TecnologiaID">Identificador de la tecnología a eliminar.</param>
        /// <returns>Redirige a la lista de tecnologías si la eliminación es exitosa, o muestra una vista de error si la tecnología no se encuentra.</returns>
        [HttpPost]
        public async Task<IActionResult> Eliminar(int TecnologiaID)
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Tecnologia tecnologia = await repositorioTecnologia.ObtenerTecnologiasPorID(TecnologiaID, UsuarioID);

            if (tecnologia == null)
            {
                return View("Error404");
            }

            await repositorioTecnologia.EliminarTecnologia(TecnologiaID, UsuarioID);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Muestra una vista de confirmación para eliminar una tecnología.
        /// </summary>
        /// <param name="TecnologiaID">Identificador de la tecnología a eliminar.</param>
        /// <returns>Una vista de confirmación si la tecnología existe, o una vista de error si no se encuentra.</returns>
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

        /// <summary>
        /// Muestra la vista para editar una tecnología existente.
        /// </summary>
        /// <param name="TecnologiaID">Identificador de la tecnología a editar.</param>
        /// <returns>Una vista para editar la tecnología si existe, o una vista de error si no se encuentra.</returns>
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

        /// <summary>
        /// Actualiza una tecnología existente en el sistema.
        /// </summary>
        /// <param name="tecnologia">Instancia de la clase Tecnología con la información modificada.</param>
        /// <returns>Redirige a la lista de tecnologías si la edición es exitosa, o muestra una vista de error si el modelo es inválido.</returns>
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

        /// <summary>
        /// Verifica si una tecnología con un nombre específico ya existe para el usuario.
        /// </summary>
        /// <param name="nombre">Nombre de la tecnología a verificar.</param>
        /// <returns>Un valor booleano en formato JSON que indica si la tecnología ya existe.</returns>
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

