using System;

namespace Downloader.Common.Models
{
    public sealed class NewsItem
    {
        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public string Description { get; set; }
    }
}
