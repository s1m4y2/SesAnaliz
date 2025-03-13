using System;
using System.ComponentModel.DataAnnotations;

namespace SesAnalizAPI.Models
{
    public class AudioRecord
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }
}
