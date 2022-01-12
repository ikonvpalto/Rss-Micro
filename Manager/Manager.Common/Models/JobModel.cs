using System;

namespace Manager.Common.Models
{
    public sealed class JobModel
    {
        public Guid Guid { get; set; }

        public bool IsJobEnabled { get; set; }

        public string Periodicity { get; set; }
    }
}
