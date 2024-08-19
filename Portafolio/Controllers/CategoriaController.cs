using Microsoft.AspNetCore.Mvc;
using Portafolio.Models;
using Portafolio.Servicios;

namespace Portafolio.Controllers
{
    public class CategoriaController : Controller
    {

        private readonly IRepositorioCategoria repositorioCategoria;
        private readonly IRepositorioUsuario repositorioUsuario;

        public CategoriaController(IRepositorioCategoria repositorioCategoria, IRepositorioUsuario repositorioUsuario) {
            this.repositorioCategoria = repositorioCategoria;
            this.repositorioUsuario = repositorioUsuario;
        }

        public async Task<IActionResult> Index()
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            IEnumerable<Categoria> categorias = await repositorioCategoria.ObtenerCategorias(UsuarioID);

            return View(categorias);
        }

        public IActionResult Crear()
        {
            Categoria categoria = new Categoria();
            return View("CrearEditar", categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {

            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            categoria.Estado = true;
            categoria.UsuarioID = await repositorioUsuario.ObtenerUsuario();
            await repositorioCategoria.Crear(categoria);

            return RedirectToAction("Index");
        }

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

        public async Task<IActionResult> Editar(int CategoriaID) 
        {
            int UsuarioID = await repositorioUsuario.ObtenerUsuario();
            Categoria categoria = await repositorioCategoria.ObtenerCategoriasPorID(CategoriaID, UsuarioID);

            if (categoria == null) 
            {
                return View("Error404");
            }

            return View("CrearEditar",categoria);
        }

       
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