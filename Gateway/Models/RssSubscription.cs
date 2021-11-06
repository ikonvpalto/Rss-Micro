using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{
    public sealed class RssSubscription : BaseRssSubscription
    {
        [Required]
        public Guid Guid { get; set; }
    }
}
