using Microsoft.EntityFrameworkCore;
using VinylRecordsApp.Models.Domain;

namespace VinylRecordsApp.Data
{
    public class VinylContext: DbContext
    {
        public VinylContext(DbContextOptions options) : base(options) { }

        public DbSet<Vinyl> VinylRecords { get; set; }
        
    }
}
