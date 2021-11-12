using Db.Common.Repositories;
using Sender.API.Database;
using Sender.API.Models;

namespace Sender.API.Repository
{
    public sealed class EmailRepository : BaseCollectionRepository<ReceiverEmailModel>, IEmailRepository
    {
        public EmailRepository(SenderDbContext dbContext) : base(dbContext, dbContext.Emails)
        {
        }
    }
}
