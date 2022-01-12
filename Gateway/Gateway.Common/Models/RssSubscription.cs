using System;
using System.Collections.Generic;

namespace Gateway.Common.Models
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
    public sealed class RssSubscription
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// Cron expression for periodicity of news mailing
        /// </summary>
        public string Periodicity { get; set; }

        public bool NeedToSendEmails { get; set; }

        public string RssSource { get; set; }

        /// <summary>
        /// List of regexes to filter header and description of news items
        /// </summary>
        public ICollection<string> Filters { get; set; } = Array.Empty<string>();

        /// <summary>
        /// List of emails where to send news
        /// </summary>
        public ICollection<string> Receivers { get; set; } = Array.Empty<string>();
    }
}
