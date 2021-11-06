using System;

namespace Downloader.Common.Models
{
    public sealed class RssSourceReadModel
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public DateTime LastSuccessDownloading { get; set; }
    }
}