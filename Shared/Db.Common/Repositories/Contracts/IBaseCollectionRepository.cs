using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Db.Common.Models;

namespace Db.Common.Repositories.Contracts
{
    public interface IBaseCollectionRepository<TModel> : IBaseRepository<TModel>
        where TModel : IDbCollectionModel
    {
        Task<IEnumerable<TModel>> GetGroupAsync(Guid groupGuid);

        Task<bool> IsGroupExistsAsync(Guid groupGuid);

        Task UpdateGroupAsync(IEnumerable<TModel> models);

        Task DeleteGroupAsync(Guid groupGuid);
    }
}