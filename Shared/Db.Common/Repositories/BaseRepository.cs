using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Db.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Db.Common.Repositories
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : class, IDbModel
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TModel> _dbSet;

        protected BaseRepository(DbContext dbContext, DbSet<TModel> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public async Task CreateAsync(TModel model)
        {
            _dbSet.Add(model);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CreateAsync(IEnumerable<TModel> models)
        {
            _dbSet.AddRange(models);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<TModel> GetAsync(Guid guid)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Guid == guid).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>> GetAsync()
        {
            return await _dbSet.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> query)
        {
            return await _dbSet.Where(query).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsExistsAsync(Guid guid)
        {
            return await _dbSet.AnyAsync(s => s.Guid == guid).ConfigureAwait(false);
        }

        public async Task<bool> IsExistsAsync(Expression<Func<TModel, bool>> query)
        {
            return await _dbSet.AnyAsync(query).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TModel model)
        {
            _dbSet.Update(model);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var source = await _dbSet.SingleOrDefaultAsync(s => s.Guid == guid);
            _dbContext.Remove(source);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Expression<Func<TModel, bool>> query)
        {
            var models = await _dbSet.Where(query).ToListAsync();
            _dbContext.RemoveRange(models);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
