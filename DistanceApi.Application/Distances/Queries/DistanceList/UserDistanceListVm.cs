using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Application.Distances.Queries.DistanceList
{
    public class UserDistanceListVm
    {
        public IList<UserDistanceVm> UserDistances { get; set; }

        public int Page { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
    }
}
