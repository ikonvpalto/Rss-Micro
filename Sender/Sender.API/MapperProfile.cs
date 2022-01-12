using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common.Extensions;
using Sender.API.Models;
using Sender.Common.Models;

namespace Sender.API
{
    public sealed class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ReceiversModel, ReceiverEmailModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.GroupGuid, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.Email, o => o.Ignore());

            CreateMap<IEnumerable<ReceiverEmailModel>, ReceiversModel>()
                .ForMember(d => d.Guid, o => o.MapFrom((s, _, _) => s.FirstOrDefault()?.GroupGuid ?? Guid.Empty))
                .ForMember(d => d.Receivers, o => o.MapFrom(s => s.Select(f => f.Email)));

            this.AssertConfigurationIsValid();
        }
    }
}
