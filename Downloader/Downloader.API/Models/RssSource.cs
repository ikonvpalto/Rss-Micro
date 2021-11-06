using System;

namespace Downloader.API.Models
{
    public sealed class RssSource
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public DateTime LastSuccessDownloading { get; set; }
    }
}
