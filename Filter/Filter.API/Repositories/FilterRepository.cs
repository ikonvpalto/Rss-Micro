using Db.Common.Repositories;
using Filter.API.Database;
using Filter.API.Models;

namespace Filter.API.Repositories
{
    public sealed class FilterRepository : BaseCollectionRepository<FilterModel>, IFilterRepository
    {
        public FilterRepository(FilterDbContext dbContext) : base(dbContext, dbContext.Filters)
        {
        }
    }
}
