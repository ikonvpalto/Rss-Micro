using Db.Common.Repositories;
using Manager.API.Database;
using Manager.API.Models;

namespace Manager.API.Repositories
{
    public sealed class JobPeriodicityRepository : BaseRepository<JobPeriodicityModel>, IJobPeriodicityRepository
    {
        public JobPeriodicityRepository(ManagerDbContext dbContext) : base(dbContext, dbContext.JobPeriodicities) { }
    }
}