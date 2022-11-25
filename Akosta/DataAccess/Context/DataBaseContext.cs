using Akosta.DataAccess.Core.Interfaces.DBContext;
using Akosta.DataAccess.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Akosta.DataAccess.Context
{
    public class DataBaseContext : DbContext, IDbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<UserRto> Users { get; set; }
        public DbSet<StudyRto> Studys { get; set; }
    }
}
