using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Domain.Entities
{
    public class UserDistance : BaseEntity
    {
        public int Id { get; set; }

        public virtual Location FromPoint { get; set; }

        public virtual Location ToPoint { get; set; }

        public double Distance { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

    }
}
