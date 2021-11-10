using System;
using System.Collections.Generic;

namespace Filter.Common.Models
{
    public sealed class NewsFilterModel
    {
        public Guid Guid { get; set; }

        public ICollection<string> Filters { get; set; }
    }
}
