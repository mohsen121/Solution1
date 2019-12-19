using AutoMapper;
using DistanceApi.Application.Common.Mappings;
using DistanceApi.Domain;
using DistanceApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Application.Distances.Queries.DistanceList
{
    public class UserDistanceVm : IMapFrom<UserDistance>
    {
        public Location FromPoint { get; set; }

        public Location ToPoint { get; set; }

        public double Distance { get; set; }

        public DateTime DateCalculated { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDistance, UserDistanceVm>()
                .ForMember(x => x.DateCalculated, x => x.MapFrom(a => a.Created))
                ;
        }

    }

    
}
