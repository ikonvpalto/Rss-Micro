using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Db.Common.Repositories;
using Filter.API.Database;
using Filter.API.Models;

namespace Filter.API.Repositories
{
    public sealed class FilterRepository : BaseRepository<FilterModel>, IFilterRepository
    {
        public FilterRepository(FilterDbContext dbContext) : base(dbContext, dbContext.Filters)
        {
        }

        public async Task<IEnumerable<FilterModel>> GetByGroupGuidAsync(Guid groupGuid)
        {
            return await GetAsync(s => s.GroupGuid == groupGuid);
        }

        public async Task DeleteByGroupGuidAsync(Guid groupGuid)
        {
            await DeleteAsync(s => s.GroupGuid == groupGuid);
        }
    }
}
