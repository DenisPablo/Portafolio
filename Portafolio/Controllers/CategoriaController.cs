using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    /// <summary>
    /// Controlador para gestionar las categorías en el sistema.
    /// </summary>
    public class CategoriaController : Controller
    {
        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioUsuario repositorioUsuario;

        public CategoriaController(IRepositorioCategoria repositorioCategoria, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioUsuario = repositorioUsuario;
        }

        /// <summary>
        /// Muestra una lista de todas las categorías del usuario actual.
        /// </summary>
        /// <returns>Una vista con la lista de categorías.</returns>
        public async Task<IActionResult> Index()
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            IEnumerable<Categoria> categorias = await repositorioCategoria.ObtenerCategorias(UsuarioID);

            return View(categorias);
        }

        /// <summary>
        /// Muestra la vista para crear una nueva categoría.
        /// </summary>
        /// <returns>Una vista con un modelo de categoría vacío.</returns>
        [HttpGet]
        public IActionResult Crear()
        {
            Categoria categoria = new Categoria();
            return View("CrearEditar", categoria);
        }

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="categoria">Instancia de la clase Categoría con la información a crear.</param>
        /// <returns>Redirige a la lista de categorías si la creación es exitosa.</returns>
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.UsuarioID = await repositorioUsuario.ObtenerUsuario();
            await repositorioCategoria.Crear(categoria);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Muestra una vista de confirmación para eliminar una categoría.
        /// </summary>
        /// <param name="CategoriaID">Identificador de la categoría a eliminar.</param>
        /// <returns>Una vista de confirmación si la categoría existe, o una vista de error si no se encuentra.</returns>
        public async Task<IActionResult> ConfirmarEliminar(int CategoriaID)
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Categoria categoria = await repositorioCategoria.ObtenerCategoriasPorID(CategoriaID, UsuarioID);

            if (categoria == null)
            {
                return View("Error404");
            }

            return View("_Partials/_Confirmar", categoria);
        }

        /// <summary>
        /// Elimina una categoría del sistema.
        /// </summary>
        /// <param name="CategoriaID">Identificador de la categoría a eliminar.</param>
        /// <returns>Redirige a la lista de categorías si la eliminación es exitosa, o a una vista de error si no se encuentra.</returns>
        [HttpPost]
        public async Task<IActionResult> Eliminar(int CategoriaID)
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Categoria categoria = await repositorioCategoria.ObtenerCategoriasPorID(CategoriaID, UsuarioID);

            if (categoria == null)
            {
                return View("Error404");
            }

            await repositorioCategoria.EliminarCategoria(CategoriaID, UsuarioID);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Muestra la vista para editar una categoría existente.
        /// </summary>
        /// <param name="CategoriaID">Identificador de la categoría a editar.</param>
        /// <returns>Una vista para editar la categoría si existe, o una vista de error si no se encuentra.</returns>
        public async Task<IActionResult> Editar(int CategoriaID)
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Categoria categoria = await repositorioCategoria.ObtenerCategoriasPorID(CategoriaID, UsuarioID);

            if (categoria == null)
            {
                return View("Error404");
            }

            return View("CrearEditar", categoria);
        }

        /// <summary>
        /// Actualiza una categoría existente en el sistema.
        /// </summary>
        /// <param name="categoria">Instancia de la clase Categoría con la información modificada.</param>
        /// <returns>Redirige a la lista de categorías si la edición es exitosa, o muestra una vista de error si la categoría no es válida.</returns>
        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            categoria.UsuarioID = UsuarioID;

            if (!ModelState.IsValid)
            {
                return View("Error404");
            }

            await repositorioCategoria.EditarCategoria(categoria);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifica si una categoría con un nombre específico ya existe para el usuario.
        /// </summary>
        /// <param name="nombre">Nombre de la categoría a verificar.</param>
        /// <returns>Un valor booleano en formato JSON que indica si la categoría ya existe.</returns>
        [HttpGet]
        public async Task<IActionResult> VerificarExistenciaCategoria(string nombre)
        {
            var UsuarioID = await repositorioUsuario.ObtenerUsuario();
            var yaExisteCategoria = await repositorioCategoria.ExisteCategoria(nombre, UsuarioID);

            if (yaExisteCategoria)
            {
                return Json($"El nombre ya existe");
            }

            return Json(true);
        }
    }
}
