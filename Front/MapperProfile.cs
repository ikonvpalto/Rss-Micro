using System;
using System.Linq;
using AutoMapper;
using Common.Extensions;
using Front.ViewModels;
using Gateway.Common.Models;

namespace Front;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<RssSubscription, RssSubscriptionViewModel>()
            .ForMember(d => d.Filters, o => o.MapFrom(s => s.Filters.JoinString(", ")))
            .ForMember(d => d.Receivers, o => o.MapFrom(s => s.Receivers.JoinString(", ")))
            .ReverseMap()
            .ForMember(d => d.Filters, o => o.MapFrom(
                s => s.Filters
                    .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .ToArray()))
            .ForMember(d => d.Receivers, o => o.MapFrom(
                s => s.Receivers
                    .Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .ToArray()));
    }
}
