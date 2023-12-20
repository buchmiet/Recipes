using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Newtonsoft.Json;

namespace recipesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTestController : ControllerBase
    {

        [HttpGet("checkfile")]
        public IActionResult CheckFileAccessibility()
        {
            string filePath = "App_Data/certificate.pfx";
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Jeśli dotarliśmy tutaj, plik jest dostępny
                    return Ok("Plik jest dostępny i można go odczytać.");
                }
            }
            catch (Exception ex)
            {
                // W przypadku wyjątku zwrócimy informacje o błędzie
                return StatusCode(500, $"Nie można odczytać pliku: {ex.Message}");
            }
        }


        [HttpGet("test")]
        public IActionResult TestCertificate()
        {
            string text = "";
            try
            {            
                var certificate = new X509Certificate2("App_Data/certificate.pfx", "recipes", X509KeyStorageFlags.EphemeralKeySet);
                // Dodatkowe operacje z certyfikatem, jeśli to konieczne
                return Ok("Certyfikat został pomyślnie odczytany.");
            }
            catch (Exception ex)
            {
                var exceptionDetails = JsonConvert.SerializeObject(ex, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return StatusCode(500, $"Błąd odczytu certyfikatu: {exceptionDetails}");
            }
        }
    }
}
