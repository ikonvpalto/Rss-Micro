using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Db.Common.Repositories;
using Filter.API.Models;

namespace Filter.API.Repositories
{
    public interface IFilterRepository : IBaseRepository<FilterModel>
    {
        Task<IEnumerable<FilterModel>> GetByGroupGuidAsync(Guid groupGuid);

        Task DeleteByGroupGuidAsync(Guid groupGuid);
    }
}
