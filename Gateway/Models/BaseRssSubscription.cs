using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{
    public abstract class BaseRssSubscription
    {
        [Required]
        public string Periodicity { get; set; }

        [Required]
        public string RssSource { get; set; }

        public ICollection<string> Filters { get; set; }

        [Required]
        public ICollection<string> Receivers { get; set; }
    }
}
