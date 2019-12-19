using DistanceApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDb>(options =>
                options.UseInMemoryDatabase(databaseName: "DistanceDb"));

            services.AddScoped<IAppDb>(provider => provider.GetService<AppDb>());

            return services;
        }
    }
}
