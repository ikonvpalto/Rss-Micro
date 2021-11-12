using AutoMapper;
using Common.Extensions;
using Manager.API.Models;
using Manager.Common.Models;

namespace Manager.API
{
    public sealed class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<JobModel, JobPeriodicityModel>()
                .ReverseMap();

            this.AssertConfigurationIsValid();
        }
    }
}
