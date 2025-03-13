using Microsoft.AspNetCore.Mvc;
using SesAnalizAPI.Data;
using SesAnalizAPI.Models;

namespace SesAnalizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AudioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public IActionResult UploadAudio([FromBody] AudioRecord audio)
        {
            _context.AudioRecords.Add(audio);
            _context.SaveChanges();
            return Ok(audio);
        }
    }
}
