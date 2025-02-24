using Microsoft.AspNetCore.Mvc;

namespace Portafolio.Controllers
{
    public class CvController : Controller
    {

        private readonly IWebHostEnvironment _env;

        public CvController(IWebHostEnvironment env)
        {
            _env = env;
        }


        public IActionResult Subir() {
            return View();   
        }

        [HttpPost]
        public async Task<IActionResult> Subir(IFormFile cv)
        {
            if (cv == null || cv.Length == 0)
            {
                ViewBag.Error = "Selecciona un archivo.";
                return View();
            }

            // Verifica que el archivo sea un PDF
            if (!cv.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = "Solo se permiten archivos PDF.";
                return View();
            }

            var carpeta = Path.Combine(_env.WebRootPath, "cv");
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            // Eliminar cualquier archivo existente en la carpeta
            var archivosExistentes = Directory.GetFiles(carpeta, "*.pdf");
            foreach (var archivo in archivosExistentes)
            {
                System.IO.File.Delete(archivo);
            }

            var rutaCv = Path.Combine(carpeta, cv.FileName);
            using (var stream = new FileStream(rutaCv, FileMode.Create))
            {
                await cv.CopyToAsync(stream);
            }

            ViewBag.Success = "Archivo subido correctamente.";
            return View();
        }

        public IActionResult Download()
        {
            var carpeta = Path.Combine(_env.WebRootPath, "cv");
            var filePath = Directory.GetFiles(carpeta, "*.pdf").FirstOrDefault(); // Obtiene el primer PDF en la carpeta

            if (filePath == null)
            {
                return NotFound("No se encontró ningún archivo PDF.");
            }

            var mimeType = "application/pdf";
            var fileName = Path.GetFileName(filePath);

            return PhysicalFile(filePath, mimeType, fileName);
        }

    }
}
