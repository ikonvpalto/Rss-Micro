using AutoMapper;
using Common.Extensions;
using Downloader.Common.Models;
using Gateway.Models;

namespace Gateway
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BaseRssSubscription, RssSourceManageModel>()
                .IncludeAllDerived()
                .ForMember(d => d.Url, o => o.MapFrom(s => s.RssSource))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, RssSourceManageModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid));

            this.AssertConfigurationIsValid();
        }
    }
}
