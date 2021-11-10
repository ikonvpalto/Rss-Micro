using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Filter.API.Models;
using Filter.Common.Models;

namespace Filter.API
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<NewsFilterModel, FilterModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.GroupGuid, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.Filter, o => o.Ignore());


            CreateMap<IEnumerable<FilterModel>, NewsFilterModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.First().GroupGuid))
                .ForMember(d => d.Filters, o => o.MapFrom(s => s.Select(f => f.Filter)));
        }
    }
}
