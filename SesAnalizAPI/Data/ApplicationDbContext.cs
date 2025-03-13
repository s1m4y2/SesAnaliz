using Microsoft.EntityFrameworkCore;
using SesAnalizAPI.Models;

namespace SesAnalizAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AudioRecord> AudioRecords { get; set; }
        public DbSet<SentimentAnalysis> Sentiments { get; set; }
    }
}
