using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Threading.Tasks;

namespace Portafolio.Servicios
{
    public interface ICloudinaryService
    {
        Task<DeletionResult> ElimanarImagenAsync(string publicID);
        Task<ImageUploadResult> SubirImagenAsyc(Stream imageStream, string nombre);
    }

    /// <summary>
    /// Proporciona métodos para interactuar con el servicio Cloudinary para la carga de imágenes.
    /// </summary>
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CloudinaryService"/>.
        /// Configura el cliente de Cloudinary con las credenciales proporcionadas.
        /// </summary>
        public CloudinaryService()
        {
            var account = new Account(
                "dkxj86ene",       // Reemplaza con tu Cloud Name
                "262662459624839", // Reemplaza con tu API Key
                "yYBLGZvuMhCm6IzN_pFVcNaTR1k"); // Reemplaza con tu API Secret

            _cloudinary = new Cloudinary(account);
        }
        
        /// <summary>
        /// Sube una imagen al servicio de Cloudinary.
        /// </summary>
        /// <param name="imageStream">El flujo de la imagen que se desea subir.</param>
        /// <param name="nombre">El nombre del archivo de la imagen.</param>
        /// <returns>Una tarea que representa el resultado de la operación de carga. El resultado contiene detalles sobre la imagen subida.</returns>
        /// <exception cref="ArgumentNullException">Se lanza si <paramref name="imageStream"/> es null.</exception>
        public async Task<ImageUploadResult> SubirImagenAsyc(Stream imageStream, string nombre)
        {
            if (imageStream == null)
            {
                throw new ArgumentNullException(nameof(imageStream), "El flujo de la imagen no puede ser nulo.");
            }

            var ParametrosSubida = new ImageUploadParams()
            {
                File = new FileDescription(nombre, imageStream),
                Transformation = new Transformation().Height(1000).Width(1000).Crop("fill")
            };

            var results = await _cloudinary.UploadAsync(ParametrosSubida);
            return results;
        }

        /// <summary>
        /// Borra una imagen del servicio de Cloudinary
        /// </summary>
        /// <param name="publicID">ID publico de la imagen</param>
        /// <returns>Una tarea que representa el resultado de la operacion de carga. El resultado contiene detalles sobre la imagen borrada</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<DeletionResult> ElimanarImagenAsync(string publicID) 
        {
            if (!string.IsNullOrWhiteSpace(publicID)) 
            {
                throw new ArgumentException(nameof(publicID));
            }

            var parametrosEliminacion = new DeletionParams(publicID);
            var resultados = await _cloudinary.DestroyAsync(parametrosEliminacion);
            return resultados;
        }
    }
}