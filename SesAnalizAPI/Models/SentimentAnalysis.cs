using System.ComponentModel.DataAnnotations;

namespace SesAnalizAPI.Models
{
    public class SentimentAnalysis
    {
        [Key]
        public int Id { get; set; }
        public int AudioRecordId { get; set; }
        public string Sentiment { get; set; } // Mutlu, Öfkeli vs.
        public float Confidence { get; set; } // %90 gibi

        public AudioRecord AudioRecord { get; set; }
    }
}
