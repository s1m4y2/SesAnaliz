using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SesAnalizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public AnalysisController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("process-live-audio")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ProcessLiveAudio([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Ses kaydı alınamadı.");

            try
            {
                string uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string pythonExe = @"C:\Users\Simay\AppData\Local\Microsoft\WindowsApps\python.exe";
                string scriptPath = Path.Combine(_env.ContentRootPath, "PythonScripts", "analyze_audio.py");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = $"\"{scriptPath}\" \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd().Trim();

                    if (string.IsNullOrWhiteSpace(output))
                    {
                        return StatusCode(500, "Python betiği boş çıktı üretti.");
                    }

                    if (!output.StartsWith("{")) // JSON olup olmadığını kontrol et
                    {
                        return StatusCode(500, $"Geçersiz çıktı: {output}");
                    }

                    var analysisResult = JsonSerializer.Deserialize<AnalysisResult>(output, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Ok(new
                    {
                        message = "Analiz tamamlandı!",
                        text = analysisResult.Text,
                        wordCount = analysisResult.WordCount,
                        topic = analysisResult.Topic
                    });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hata: {ex.Message}");
            }
        }


        [HttpPost("process-audio")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "Ses dosyasını analiz eder",
            Description = "Bir ses dosyasını alıp analiz eden API.",
            OperationId = "ProcessAudio",
            Tags = new[] { "Analysis" }
        )]
        [SwaggerResponse(200, "Analiz başarılı", typeof(object))]
        [SwaggerResponse(400, "Dosya yüklenmedi")]
        [SwaggerResponse(500, "İşlem sırasında hata oluştu")]
        public async Task<IActionResult> ProcessAudio([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya yüklenmedi.");

            try
            {
                // Dosyayı "Uploads" klasörüne kaydet
                string uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Python betiğini çalıştır
                string pythonExe = @"C:\Users\Simay\AppData\Local\Microsoft\WindowsApps\python.exe"; // Python yolunu kontrol edin
                string scriptPath = Path.Combine(_env.ContentRootPath, "PythonScripts", "analyze_audio.py");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExe,
                    Arguments = $"\"{scriptPath}\" \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    // JSON çıktısını deserialize et
                    var analysisResult = JsonSerializer.Deserialize<AnalysisResult>(output, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Ok(new
                    {
                        message = "Analiz tamamlandı!",
                        text = analysisResult.Text,
                        wordCount = analysisResult.WordCount,
                        topic = analysisResult.Topic
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"İşlem sırasında hata oluştu: {ex.Message}");
            }
        }

        // Python'dan gelen JSON çıktısını karşılamak için model
        private class AnalysisResult
        {
            public string Text { get; set; }
            public int WordCount { get; set; }
            public string Topic { get; set; }
        }
    }
}
