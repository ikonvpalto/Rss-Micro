using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Db.Common.Models;

namespace Db.Common.Repositories.Contracts
{
    public interface IBaseRepository<TModel>
        where TModel : IDbModel
    {
        Task CreateAsync(TModel model);

        Task CreateAsync(IEnumerable<TModel> models);

        Task<TModel> GetAsync(Guid guid);

        Task<IEnumerable<TModel>> GetAsync();

        Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> query);

        Task<bool> IsExistsAsync(Guid guid);

        Task<bool> IsExistsAsync(Expression<Func<TModel, bool>> query);

        Task UpdateAsync(TModel model);

        Task DeleteAsync(Guid guid);

        Task DeleteAsync(Expression<Func<TModel, bool>> query);
    }
}
