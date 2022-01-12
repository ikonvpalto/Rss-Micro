using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Models
{
    /// <summary>
    /// Rss subscription
    /// </summary>
    /// <example>
    /// {
    ///     "guid": "17a42ccd-bdac-421d-896e-35f208a0eefc",
    ///     "periodicity": "*/5 * * * *",
    ///     "rssSource": "https://onliner.by/feed",
    ///     "filters": [
    ///         "covid"
    ///     ],
    ///     "receivers": [
    ///         "example@gmail.com"
    ///     ]
    /// }
    /// </example>
    public sealed class RssSubscription : BaseRssSubscription
    {
        [Required]
        public Guid Guid { get; set; }
    }
}
