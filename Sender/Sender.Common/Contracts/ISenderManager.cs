using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Downloader.Common.Models;
using Sender.Common.Models;

namespace Sender.Common.Contracts
{
    public interface ISenderManager
    {
        Task CreateAsync(ReceiversModel model);

        Task UpdateAsync(ReceiversModel model);

        Task DeleteAsync(Guid receiversGuid);

        Task SendNewsAsync(Guid receiversGuid, IEnumerable<NewsItem> news);
    }
}