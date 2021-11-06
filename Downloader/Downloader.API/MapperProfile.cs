using AutoMapper;
using Common.Extensions;
using Downloader.API.Models;
using Downloader.Common.Models;
using Downloader.Common.Models.RequestModels;

namespace Downloader.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RssSourceRequestModel, RssSourceManageModel>()
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSourceManageModel, RssSource>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<RssSource, RssSourceReadModel>();

            CreateMap<RssSourceResponseItem, NewsItem>();

            this.AssertConfigurationIsValid();
        }
    }
}
