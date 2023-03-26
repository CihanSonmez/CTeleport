using CTeleport.Domain.Enums;
using CTeleport.Domain.Exceptions;
using CTeleport.Localization;

namespace CTeleport.Application.Features.Distance.Rules
{
    public class CoordinateRules
    {
        private readonly Localizer _localizer;

        public CoordinateRules(Localizer localizer)
        {
            _localizer = localizer;
        }

        public void LatitudeShouldBeValid(string iataCode, double latitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new BusinessException(ErrorType.InvalidCoordinates, _localizer.InvalidLatitude(latitude, iataCode));
        }

        public void LongitudeShouldBeValid(string iataCode, double longitude)
        {
            if (longitude < -180 || longitude > 180)
                throw new BusinessException(ErrorType.InvalidCoordinates, _localizer.InvalidLongitude(longitude, iataCode));
        }
    }
}