using System;
using System.Collections.Generic;
using Db.Common.Models;

namespace Filter.API.Models
{
    public sealed class FilterModel : IDbModel
    {
        public Guid Guid { get; set; }

        public Guid GroupGuid { get; set; }

        public string Filter { get; set; }
    }
}
