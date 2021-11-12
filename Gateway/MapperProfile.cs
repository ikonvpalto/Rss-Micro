using AutoMapper;
using Common.Extensions;
using Downloader.Common.Models;
using Filter.Common.Models;
using Gateway.Models;
using Sender.Common.Models;

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

            CreateMap<RssSourceReadModel, RssSubscription>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.RssSource, o => o.MapFrom(d => d.Url))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<BaseRssSubscription, NewsFilterModel>()
                .IncludeAllDerived()
                .ForMember(d => d.Filters, o => o.MapFrom(s => s.Filters))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, NewsFilterModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid));

            CreateMap<NewsFilterModel, RssSubscription>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.Filters, o => o.MapFrom(d => d.Filters))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<BaseRssSubscription, ReceiversModel>()
                .IncludeAllDerived()
                .ForMember(d => d.Receivers, o => o.MapFrom(s => s.Receivers))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, ReceiversModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid));

            CreateMap<ReceiversModel, RssSubscription>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.Receivers, o => o.MapFrom(d => d.Receivers))
                .ForAllOtherMembers(o => o.Ignore());

            this.AssertConfigurationIsValid();
        }
    }
}
