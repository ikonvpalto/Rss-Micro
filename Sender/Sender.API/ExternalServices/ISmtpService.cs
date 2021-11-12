using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sender.API.ExternalServices
{
    public interface ISmtpService
    {
        Task SendMailAsync(IEnumerable<string> receivers, string subject, string body);
    }
}
