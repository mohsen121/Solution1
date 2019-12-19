using DistanceApi.Application.Common.Interfaces;
using DistanceApi.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceApi.Application.Distances.Commands
{
    public class CreateAndCalculateDistanceCommand : IRequest<double>
    {
        public double Lat1 { get; set; }
        public double Lon1 { get; set; }

        public double Lat2 { get; set; }
        public double Lon2 { get; set; }
    }

    public class Handler : IRequestHandler<CreateAndCalculateDistanceCommand, double>
    {
        private IAppDb _db;
        private ICurrentUserService _currentUserService;

        public Handler(IAppDb db, ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<double> Handle(CreateAndCalculateDistanceCommand request, CancellationToken cancellationToken)
        {
            double R = 6371; // km

            double sLat1 = Math.Sin(ToRadians(request.Lat1));
            double sLat2 = Math.Sin(ToRadians(request.Lat2));
            double cLat1 = Math.Cos(ToRadians(request.Lat1));
            double cLat2 = Math.Cos(ToRadians(request.Lat2));
            double cLon = Math.Cos(ToRadians(request.Lon1) - ToRadians(request.Lon2));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            var entity = new UserDistance
            {
                FromPoint = new Domain.Location
                {
                    Latitude = request.Lat1,
                    Longitude = request.Lon1
                },
                ToPoint = new Domain.Location
                {
                    Latitude = request.Lat2,
                    Longitude = request.Lon2,
                },
                Distance = dist,

                UserId = _currentUserService.UserId,
                Created = DateTime.Now,
                LastModified = DateTime.Now,
            };

            _db.UserDistances.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return dist;
        }
        private double ToRadians(double x)
        {
            return x * Math.PI / 180;
        }
    }
}
