using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PiggyBank.Domain.Infrastructure
{
    public class DbWorker : IDisposable
    {
        private readonly DbContext _context;

        public DbWorker(DbContext context)
            => _context = context;

        public DbContext Context => _context;

        public DbSet<T> GetRepository<T>() where T : class
            => _context.Set<T>();
  
        //TODO Refactor
        public Task<int> SaveAsync()
            => _context.SaveChangesAsync();

        public void Dispose()
            => _context?.Dispose();
    }
}
