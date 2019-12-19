using DistanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Persistence.Configurations
{
    public class UserDistanceConfiguration : IEntityTypeConfiguration<UserDistance>
    {
        public void Configure(EntityTypeBuilder<UserDistance> builder)
        {
            builder.OwnsOne(x => x.FromPoint);
            builder.OwnsOne(x => x.ToPoint);
        }
    }
}
