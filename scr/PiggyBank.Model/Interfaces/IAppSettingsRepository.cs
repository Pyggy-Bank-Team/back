using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface IAppSettingsRepository
    {
        Task<AppSettings> GetAsync(Guid userId, CancellationToken token);
    }
}