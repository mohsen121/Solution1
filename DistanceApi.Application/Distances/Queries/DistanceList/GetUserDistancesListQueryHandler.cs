using AutoMapper;
using AutoMapper.QueryableExtensions;
using DistanceApi.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceApi.Application.Distances.Queries.DistanceList
{
    public class GetUserDistancesListQueryHandler : IRequestHandler<GetUserDistancesListQuery, UserDistanceListVm>
    {
        private ICurrentUserService _currentUserService;
        private IAppDb _appDb;
        private IMapper _mapper;

        public GetUserDistancesListQueryHandler(ICurrentUserService currentUserService, IAppDb appDb,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _appDb = appDb;
            _mapper = mapper;
        }
        public async Task<UserDistanceListVm> Handle(GetUserDistancesListQuery request, CancellationToken cancellationToken)
        {
            var dists = _appDb.UserDistances
                .Where(x => x.UserId == _currentUserService.UserId)
                .AsQueryable();

            var response = new UserDistanceListVm
            {
                UserDistances = await dists.OrderByDescending(x => x.Id)
                .Skip((request.Page - 1) * request.Count)
                .Take(request.Count)
                .Include(x => x.FromPoint)
                .Include(x => x.ToPoint)
                .AsNoTracking()
                .Select(x => new UserDistanceVm {
                    DateCalculated = x.Created,
                    FromPoint = x.FromPoint,
                    ToPoint = x.ToPoint,
                    Distance = x.Distance
                }).ToListAsync(),
                Page = request.Page,
                Count = request.Count,
                TotalCount = await dists.CountAsync()
            };

            return response;
        }
    }
}
