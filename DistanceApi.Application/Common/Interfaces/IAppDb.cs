using DistanceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceApi.Application.Common.Interfaces
{
    public interface IAppDb
    {
        DbSet<UserDistance> UserDistances { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
