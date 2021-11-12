using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Db.Common.Models;
using Db.Common.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Db.Common.Repositories
{
    public abstract class BaseCollectionRepository<TModel> : BaseRepository<TModel>, IBaseCollectionRepository<TModel>
        where TModel : class, IDbCollectionModel
    {
        protected BaseCollectionRepository(DbContext dbContext, DbSet<TModel> dbSet) : base(dbContext, dbSet) { }

        public async Task<IEnumerable<TModel>> GetGroupAsync(Guid groupGuid)
        {
            return await GetAsync(s => s.GroupGuid == groupGuid);
        }

        public async Task<bool> IsGroupExistsAsync(Guid groupGuid)
        {
            return await IsExistsAsync(s => s.GroupGuid == groupGuid);
        }

        public async Task UpdateGroupAsync(IEnumerable<TModel> models)
        {
            models = models.ToList();

            var groupGuid = models.First().GroupGuid;
            var oldModels = await DbSet.Where(s => s.GroupGuid == groupGuid).ToListAsync();
            DbContext.RemoveRange(oldModels);

            DbSet.AddRange(models);

            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteGroupAsync(Guid groupGuid)
        {
            await DeleteAsync(s => s.GroupGuid == groupGuid);
        }
    }
}