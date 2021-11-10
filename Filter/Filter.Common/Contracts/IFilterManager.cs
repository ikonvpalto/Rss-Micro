using System;
using System.Threading.Tasks;
using Filter.Common.Models;

namespace Filter.Common.Contracts
{
    public interface IFilterManager
    {
        Task CreateAsync(NewsFilterModel filter);

        Task UpdateAsync(NewsFilterModel filter);

        Task DeleteAsync(Guid filterGuid);
    }
}