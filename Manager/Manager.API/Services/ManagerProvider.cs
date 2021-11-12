using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Manager.API.Repositories;
using Manager.API.Resources;
using Manager.Common.Contracts;
using Manager.Common.Models;

namespace Manager.API.Services
{
    public sealed class ManagerProvider : IManagerProvider
    {
        private static readonly Regex CronRegex =
            new(@"(@(annually|yearly|monthly|weekly|daily|hourly|reboot))|(@every (\d+(ns|us|µs|ms|s|m|h))+)|((((\d+,)+\d+|(\d+(\/|-)\d+)|\d+|\*) ?){5,7})");

        private readonly IJobPeriodicityRepository _jobPeriodicityRepository;
        private readonly IMapper _mapper;

        public ManagerProvider(IJobPeriodicityRepository jobPeriodicityRepository, IMapper mapper)
        {
            _jobPeriodicityRepository = jobPeriodicityRepository;
            _mapper = mapper;
        }

        public async Task<JobModel> GetAsync(Guid guid)
        {
            var jobPeriodicity = await _jobPeriodicityRepository.GetAsync(guid).ConfigureAwait(false);
            return _mapper.Map<JobModel>(jobPeriodicity);
        }

        public async Task<IEnumerable<JobModel>> GetAsync()
        {
            var jobPeriodicities = await _jobPeriodicityRepository.GetAsync().ConfigureAwait(false);
            return jobPeriodicities.Select(_mapper.Map<JobModel>);
        }

        public Task EnsureJobPeriodicityIsValidAsync(string jobPeriodicity)
        {
            return CronRegex.IsMatch(jobPeriodicity)
                ? Task.CompletedTask
                : Task.FromException(new BadRequestException(Localization.NotAJobPeriodicity));
        }
    }
}
