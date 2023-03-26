using CTeleport.Application.Features.Distance.DTOs.Responses;
using CTeleport.Application.Features.Distance.Enums;
using CTeleport.Application.Features.Distance.Rules;
using CTeleport.Application.Features.Distance.Utils;
using CTeleport.Application.Interfaces;
using CTeleport.Application.Wrappers;
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
                
                coordinateRules.LatitudeShouldBeValid(request.FirstIataCode, firstAirportCoordinate.Latitude);
                coordinateRules.LongitudeShouldBeValid(request.FirstIataCode, firstAirportCoordinate.Longitude);

                var secondAirportCoordinate = await this.airportService.GetCoordinate(request.SecondIataCode);
                
                coordinateRules.LatitudeShouldBeValid(request.SecondIataCode, secondAirportCoordinate.Latitude);
                coordinateRules.LongitudeShouldBeValid(request.SecondIataCode, secondAirportCoordinate.Longitude);

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
        }
    }
}