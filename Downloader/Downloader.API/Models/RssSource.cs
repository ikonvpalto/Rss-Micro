using System;
using Db.Common.Models;

namespace Downloader.API.Models
{
    public sealed class RssSource : IDbModel
    {
        public Guid Guid { get; set; }

        public string Url { get; set; }

        public DateTime LastSuccessDownloading { get; set; }
    }
}
