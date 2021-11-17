using System;

namespace Downloader.Common.Models
{
    public sealed class RssSourceReadModel : BaseRssSourceModel
    {
        public DateTime LastSuccessDownloading { get; set; }
    }
}
