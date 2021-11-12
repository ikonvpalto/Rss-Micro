using System;
using System.Threading.Tasks;
using Manager.Common.Models;

namespace Manager.Common.Contracts
{
    public interface IManagerManager
    {
        Task CreateAsync(JobModel job);

        Task UpdateAsync(JobModel job);

        Task DeleteAsync(Guid jobGuid);
    }
}
