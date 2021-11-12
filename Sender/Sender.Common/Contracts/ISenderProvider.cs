using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sender.Common.Models;

namespace Sender.Common.Contracts
{
    public interface ISenderProvider
    {
        Task<ReceiversModel> GetAsync(Guid guid);

        Task<IEnumerable<ReceiversModel>> GetAsync();

        Task EnsureReceiversIsValidAsync(IEnumerable<string> receiverEmails);
    }
}
