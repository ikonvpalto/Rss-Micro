using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manager.Common.Models;

namespace Manager.Common.Contracts
{
    public interface IManagerProvider
    {
        Task<JobModel> GetAsync(Guid jobGuid);

        Task<IEnumerable<JobModel>> GetAsync();

        Task EnsureJobPeriodicityIsValidAsync(string jobPeriodicity);
    }
}
