using System;
using System.Collections.Generic;

namespace Gateway.Models
{
    public abstract class BaseRssSubscription
    {
        public TimeSpan Periodicity { get; set; }

        public string RssSource { get; set; }

        public ICollection<string> Filters { get; set; }

        public ICollection<string> Receivers { get; set; }
    }
}