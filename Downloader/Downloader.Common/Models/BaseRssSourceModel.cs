using System;

namespace Downloader.Common.Models
{
    public class BaseRssSourceModel
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }
    }
}