using Microsoft.EntityFrameworkCore;
using static TestSoft.Controllers.TableController;

namespace TestSoft.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=tables.db");
       
    }
}
