using CTeleport.Application.Features.Distance.DTOs.Responses;
using CTeleport.Application.Features.Distance.Enums;
using CTeleport.Application.Features.Distance.Queries;
using CTeleport.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace CTeleport.API.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistanceController : BaseApiController
    {
        /// <summary>
        /// Gets two iata codes and calculates distance between airports in miles
        /// </summary>
        /// <param name="firstIataCode"></param>
        /// <param name="secondIataCode"></param>
        /// <returns>distance between two airports in miles</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /distance/miles?firstIataCode=IST&amp;secondIataCode=AMS
        /// </remarks>
        [HttpGet("miles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<DistanceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDistanceInMiles(string firstIataCode, string secondIataCode)
        {
            return Ok(await Mediator.Send(new GetDistanceQuery
            {
                FirstIataCode = firstIataCode,
                SecondIataCode = secondIataCode,
                Unit = DistanceUnit.Miles
            }));
        }

        /// <summary>
        /// Gets two iata codes and calculates distance between airports in kilometers
        /// </summary>
        /// <param name="firstIataCode"></param>
        /// <param name="secondIataCode"></param>
        /// <returns>distance between two airports in kilometers</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /distance/kilometers?firstIataCode=IST&amp;secondIataCode=AMS
        /// </remarks>
        [HttpGet("kilometers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<DistanceResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDistanceInKilometers(string firstIataCode, string secondIataCode)
        {
            return Ok(await Mediator.Send(new GetDistanceQuery
            {
                FirstIataCode = firstIataCode,
                SecondIataCode = secondIataCode,
                Unit = DistanceUnit.Kilometers
            }));
        }
    }
}