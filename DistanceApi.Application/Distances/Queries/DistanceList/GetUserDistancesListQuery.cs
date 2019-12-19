using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Application.Distances.Queries.DistanceList
{
    public class GetUserDistancesListQuery : IRequest<UserDistanceListVm>
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
    }
}
