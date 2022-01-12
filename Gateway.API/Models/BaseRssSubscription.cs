using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{

    public abstract class BaseRssSubscription
    {
        /// <summary>
        /// Cron expression for periodicity of news mailing
        /// </summary>
        [Required]
        public string Periodicity { get; set; }

        [Required]
        public string RssSource { get; set; }

        /// <summary>
        /// List of regexes to filter header and description of news items
        /// </summary>
        public ICollection<string> Filters { get; set; }

        /// <summary>
        /// List of emails where to send news
        /// </summary>
        [Required]
        public ICollection<string> Receivers { get; set; }
    }
}
