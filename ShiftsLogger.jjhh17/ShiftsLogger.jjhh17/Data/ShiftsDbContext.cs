using Microsoft.EntityFrameworkCore;
using ShiftsLogger.jjhh17.Model;

namespace ShiftsLogger.jjhh17.Data
{
    public class ShiftsDbContext : DbContext
    {
        public ShiftsDbContext(DbContextOptions options) : base(options)
        {
          
        }

        public DbSet<Shift> Shifts { get; set; }
    }
}
