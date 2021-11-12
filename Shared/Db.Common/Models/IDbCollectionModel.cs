using System;

namespace Db.Common.Models
{
    public interface IDbCollectionModel : IDbModel
    {
        Guid GroupGuid { get; }
    }
}