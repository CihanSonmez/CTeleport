using CTeleport.Application.Features.Distance.DTOs.Responses;
using CTeleport.Application.Features.Distance.Enums;
using CTeleport.Application.Features.Distance.Rules;
using CTeleport.Application.Features.Distance.Utils;
using CTeleport.Application.Interfaces;
using CTeleport.Application.Wrappers;
using CTeleport.Domain.Common;
using MediatR;

namespace CTeleport.Application.Features.Distance.Queries
{
    public class GetDistanceQuery : IRequest<DataResponse<DistanceResponse>>
    {
        public string FirstIataCode { get; set; }
        public string SecondIataCode { get; set; }
        public DistanceUnit Unit { get; set; }

        public class GetDistanceQueryHandler : IRequestHandler<GetDistanceQuery, DataResponse<DistanceResponse>>
        {
            private IAirportService airportService;
            private CoordinateRules coordinateRules;
            public GetDistanceQueryHandler(IAirportService airportService,
                CoordinateRules coordinateRules)
            {
                this.airportService = airportService;
                this.coordinateRules = coordinateRules;
            }

            public async Task<DataResponse<DistanceResponse>> Handle(GetDistanceQuery request, CancellationToken cancellationToken)
            {
                var firstAirportCoordinate = await this.airportService.GetCoordinate(request.FirstIataCode);

                ValidateCoordinates(request.FirstIataCode, firstAirportCoordinate);

                var secondAirportCoordinate = await this.airportService.GetCoordinate(request.SecondIataCode);

                ValidateCoordinates(request.SecondIataCode, secondAirportCoordinate);

                var distance = request.Unit switch
                {
                    DistanceUnit.Miles => DistanceCalculator.CalculateInMile(firstAirportCoordinate, secondAirportCoordinate),
                    DistanceUnit.Kilometers => DistanceCalculator.CalculateInKilometer(firstAirportCoordinate, secondAirportCoordinate),
                    _ => throw new NotImplementedException($"Distance calculator not implemented for unit {request.Unit}")
                };

                var responseModel = new DistanceResponse
                {
                    Distance = distance,
                    Unit = request.Unit
                };

                return DataResponse<DistanceResponse>.Success(responseModel);
            }

            private void ValidateCoordinates(string iataCode, Coordinate coordinate)
            {
                coordinateRules.LatitudeShouldBeValid(iataCode, coordinate.Latitude);
                coordinateRules.LongitudeShouldBeValid(iataCode, coordinate.Longitude);
            }
        }
    }
}