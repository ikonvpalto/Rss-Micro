using AutoMapper;
using Common.Extensions;
using Downloader.Common.Models;
using Filter.Common.Models;
using Gateway.Common.Models;
using Manager.Common.Models;
using Sender.Common.Models;

namespace Gateway
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RssSubscription, RssSourceManageModel>()
                .ForMember(d => d.Url, o => o.MapFrom(s => s.RssSource))
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid));

            CreateMap<RssSourceReadModel, RssSubscription>()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.RssSource, o => o.MapFrom(d => d.Url))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, NewsFilterModel>()
                .ForMember(d => d.Filters, o => o.MapFrom(s => s.Filters))
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ReverseMap()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.Filters, o => o.MapFrom(d => d.Filters))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, ReceiversModel>()
                .ForMember(d => d.Receivers, o => o.MapFrom(s => s.Receivers))
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ReverseMap()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.Receivers, o => o.MapFrom(d => d.Receivers))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSubscription, JobModel>()
                .ForMember(d => d.Periodicity, o => o.MapFrom(s => s.Periodicity))
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.IsJobEnabled, o => o.MapFrom(d => d.NeedToSendEmails))
                .ReverseMap()
                .ForMember(d => d.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(d => d.Periodicity, o => o.MapFrom(d => d.Periodicity))
                .ForMember(d => d.NeedToSendEmails, o => o.MapFrom(d => d.IsJobEnabled))
                .ForAllOtherMembers(o => o.Ignore());

            this.AssertConfigurationIsValid();
        }
    }
}
