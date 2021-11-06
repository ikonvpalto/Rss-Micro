using System;

namespace Downloader.API.Models
{
    public sealed class RssSourceResponseItem
    {
        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public string Description { get; set; }
    }
}
