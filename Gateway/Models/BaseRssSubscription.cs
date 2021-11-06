using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Json;
using Newtonsoft.Json;

namespace Gateway.Models
{
    public abstract class BaseRssSubscription
    {
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        [Required]
        public TimeSpan Periodicity { get; set; }

        [Required]
        public string RssSource { get; set; }

        public ICollection<string> Filters { get; set; }

        [Required]
        public ICollection<string> Receivers { get; set; }
    }
}
