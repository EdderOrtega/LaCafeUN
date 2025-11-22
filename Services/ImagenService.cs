using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ProyectoFinalPOO2.Services
{
    public interface IImagenService
    {
        Task<string> SubirImagenAsync(IFormFile archivo, string carpeta = "usuarios");
        Task<bool> EliminarImagenAsync(string publicId);
    }

    public class CloudinaryImagenService : IImagenService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryImagenService> _logger;

        public CloudinaryImagenService(IConfiguration configuration, ILogger<CloudinaryImagenService> logger)
        {
            _logger = logger;

            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            _logger.LogInformation($"[Cloudinary] Configurando con CloudName: {cloudName}");

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                _logger.LogWarning("❌ Cloudinary NO configurado. Faltan credenciales.");
                _cloudinary = null!;
                return;
            }

            try
            {
                var account = new Account(cloudName, apiKey, apiSecret);
                _cloudinary = new Cloudinary(account);
                _logger.LogInformation("✅ Cloudinary configurado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al configurar Cloudinary");
                _cloudinary = null!;
            }
        }

        public async Task<string> SubirImagenAsync(IFormFile archivo, string carpeta = "usuarios")
        {
            if (_cloudinary == null)
            {
                _logger.LogWarning("⚠️ Cloudinary no configurado, generando avatar por defecto");
                return $"https://ui-avatars.com/api/?name=Usuario&background=831D81&color=fff&size=200";
            }

            try
            {
                _logger.LogInformation($"📤 Subiendo imagen a Cloudinary. Carpeta: {carpeta}, Archivo: {archivo.FileName}");
                
                using var stream = archivo.OpenReadStream();
                
                // Configurar transformación según el tipo de carpeta
                var transformation = new Transformation()
                    .Width(500)
                    .Height(500)
                    .Crop("fill")
                    .Quality("auto");

                // Solo usar gravity:face para usuarios, NO para productos
                if (carpeta == "usuarios")
                {
                    transformation = transformation.Gravity("face");
                }
                else
                {
                    // Para productos, usar gravity:center
                    transformation = transformation.Gravity("center");
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(archivo.FileName, stream),
                    Folder = $"lacafe/{carpeta}",
                    Transformation = transformation
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                _logger.LogInformation($"📊 Cloudinary Response - StatusCode: {result.StatusCode}");

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"✅ Imagen subida exitosamente: {result.SecureUrl}");
                    return result.SecureUrl.ToString();
                }

                _logger.LogError($"❌ Error al subir imagen a Cloudinary. StatusCode: {result.StatusCode}, Error: {result.Error?.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Excepción al subir imagen a Cloudinary");
                _logger.LogError($"Detalles: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception: {ex.InnerException.Message}");
                }
                return string.Empty;
            }
        }

        public async Task<bool> EliminarImagenAsync(string publicId)
        {
            if (_cloudinary == null) return false;

            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen de Cloudinary");
                return false;
            }
        }
    }

    public class LocalImagenService : IImagenService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<LocalImagenService> _logger;

        public LocalImagenService(IWebHostEnvironment environment, ILogger<LocalImagenService> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public async Task<string> SubirImagenAsync(IFormFile archivo, string carpeta = "usuarios")
        {
            try
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", carpeta);
                Directory.CreateDirectory(uploadsPath);

                var extension = Path.GetExtension(archivo.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(uploadsPath, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                return $"/uploads/{carpeta}/{nombreArchivo}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar imagen localmente");
                return string.Empty;
            }
        }

        public Task<bool> EliminarImagenAsync(string publicId)
        {
            try
            {
                var rutaArchivo = Path.Combine(_environment.WebRootPath, publicId.TrimStart('/'));
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen local");
                return Task.FromResult(false);
            }
        }
    }
}
