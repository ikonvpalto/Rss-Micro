using Db.Common.Repositories.Contracts;
using Sender.API.Models;

namespace Sender.API.Repository
{
    public interface IEmailRepository : IBaseCollectionRepository<ReceiverEmailModel> { }
}
