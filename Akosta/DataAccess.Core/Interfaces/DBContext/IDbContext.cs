using Akosta.DataAccess.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Akosta.DataAccess.Core.Interfaces.DBContext
{
    public interface IDbContext : IDisposable, IAsyncDisposable
    {
        DbSet<UserRto> Users { get; set; }
        DbSet<StudyRto> Studys { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
