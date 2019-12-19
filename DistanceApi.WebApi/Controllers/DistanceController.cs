using DistanceApi.Application.Distances.Commands;
using DistanceApi.Application.Distances.Queries.DistanceList;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceApi.WebApi.Controllers
{
    public class DistanceController : BaseApiController
    {
        public DistanceController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<double>> Calculate([FromBody] CreateAndCalculateDistanceCommand command)
        {
            var result = await Mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UserDistanceListVm>> History([FromBody]GetUserDistancesListQuery query)
        {
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
