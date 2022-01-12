using System;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Hangfire;
using Manager.API.Models;
using Manager.API.Repositories;
using Manager.API.Resources;
using Manager.Common.Contracts;
using Manager.Common.Models;

namespace Manager.API.Services
{
    public sealed class ManagerManager : IManagerManager
    {
        private readonly IMapper _mapper;
        private readonly IJobPeriodicityRepository _jobPeriodicityRepository;
        private readonly IManagerProvider _managerProvider;
        private readonly IRecurringJobManager _recurringJobManager;

        public ManagerManager(
            IMapper mapper,
            IJobPeriodicityRepository jobPeriodicityRepository,
            IManagerProvider managerProvider,
            IRecurringJobManager recurringJobManager)
        {
            _mapper = mapper;
            _jobPeriodicityRepository = jobPeriodicityRepository;
            _managerProvider = managerProvider;
            _recurringJobManager = recurringJobManager;
        }

        public async Task CreateAsync(JobModel job)
        {
            if (await _jobPeriodicityRepository.IsExistsAsync(job.Guid).ConfigureAwait(false))
            {
                throw new AlreadyExistsException(Localization.JobAlreadyExists);
            }

            await _managerProvider.EnsureJobPeriodicityIsValidAsync(job.Periodicity).ConfigureAwait(false);

            var model = _mapper.Map<JobPeriodicityModel>(job);

            await _jobPeriodicityRepository.CreateAsync(model).ConfigureAwait(false);

            if (job.IsJobEnabled) {
                ScheduleJob(job);
            }
        }

        public async Task UpdateAsync(JobModel job)
        {
            if (!await _jobPeriodicityRepository.IsExistsAsync(job.Guid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.JobNotFound);
            }

            await _managerProvider.EnsureJobPeriodicityIsValidAsync(job.Periodicity).ConfigureAwait(false);

            var model = _mapper.Map<JobPeriodicityModel>(job);

            await _jobPeriodicityRepository.UpdateAsync(model).ConfigureAwait(false);

            if (job.IsJobEnabled)
            {
                ScheduleJob(job);
            }
            else
            {
                RemoveJob(job);
            }
        }

        public async Task DeleteAsync(Guid jobGuid)
        {
            if (!await _jobPeriodicityRepository.IsExistsAsync(jobGuid).ConfigureAwait(false))
            {
                throw new NotFoundException(Localization.JobNotFound);
            }

            await _jobPeriodicityRepository.DeleteAsync(jobGuid).ConfigureAwait(false);

            _recurringJobManager.RemoveIfExists(jobGuid.ToString("D"));
        }

        private void ScheduleJob(JobModel job)
        {
            _recurringJobManager.AddOrUpdate<IMailingService>(
                job.Guid.ToString("D"),
                x => x.SendNewsAsync(job.Guid),
                job.Periodicity);
        }

        private void RemoveJob(JobModel job)
        {
            _recurringJobManager.RemoveIfExists(
                job.Guid.ToString("D"));
        }
    }
}
