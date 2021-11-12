using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Db.Common.Models;
using Db.Common.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Db.Common.Repositories
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class, IDbModel
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TModel> DbSet;

        protected BaseRepository(DbContext dbContext, DbSet<TModel> dbSet)
        {
            DbContext = dbContext;
            DbSet = dbSet;
        }

        public async Task CreateAsync(TModel model)
        {
            DbSet.Add(model);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CreateAsync(IEnumerable<TModel> models)
        {
            DbSet.AddRange(models);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<TModel> GetAsync(Guid guid)
        {
            return await DbSet.SingleOrDefaultAsync(s => s.Guid == guid).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>> GetAsync()
        {
            return await DbSet.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> query)
        {
            return await DbSet.Where(query).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsExistsAsync(Guid guid)
        {
            return await DbSet.AnyAsync(s => s.Guid == guid).ConfigureAwait(false);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<TModel, bool>> query)
        {
            return await DbSet.AnyAsync(query).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TModel model)
        {
            DbSet.Update(model);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var model = await DbSet.SingleOrDefaultAsync(s => s.Guid == guid);
            DbContext.Remove(model);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<TModel, bool>> query)
        {
            var models = await DbSet.Where(query).ToListAsync();
            DbContext.RemoveRange(models);
            await DbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
