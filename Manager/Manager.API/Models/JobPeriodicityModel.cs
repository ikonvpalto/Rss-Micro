using System;
using Db.Common.Models;

namespace Manager.API.Models
{
    public sealed class JobPeriodicityModel : IDbModel
    {
        public Guid Guid { get; set; }

        public bool IsJobEnabled { get; set; }

        public string Periodicity { get; set; }
    }
}
