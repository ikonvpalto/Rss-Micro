using System;
using System.Collections.Generic;

namespace Sender.Common.Models
{
    public sealed class ReceiversModel
    {
        public Guid Guid { get; set; }

        public IEnumerable<string> Receivers { get; set; }
    }
}
