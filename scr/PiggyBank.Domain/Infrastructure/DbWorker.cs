using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Model;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Infrastructure
{
    public class DbWorker : IDisposable
    {
        private readonly PiggyContext _context;

        public DbWorker(PiggyContext context)
            => _context = context;

        public DbSet<T> GetRepository<T>() where T : class, IBaseModel
            => _context.Set<T>();

        //TODO Refactor
        public Task<int> SaveAsync()
            => _context.SaveChangesAsync();

        public void Dispose()
            => _context?.Dispose();
    }
}
