using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Domain
{
    public class BaseEntity
    {

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
