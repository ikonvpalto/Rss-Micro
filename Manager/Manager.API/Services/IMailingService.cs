using System;
using System.Threading.Tasks;

namespace Manager.API.Services
{
    public interface IMailingService
    {
        Task SendNewsAsync(Guid jobId);
    }
}