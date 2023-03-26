using CTeleport.Domain.Common;

namespace CTeleport.Application.Features.Distance.Utils
{
    public static class DistanceCalculator
    {
        private const short EarthRadiusInKilometer = 6371;
        private const double OneKilometerInMile = 0.621371192;

        public static double CalculateInKilometer(Coordinate coordinate1, Coordinate coordinate2)
        {
            // Haversine formula
            // reference: https://www.geeksforgeeks.org/program-distance-two-points-earth/

            var lon1 = coordinate1.Longitude * Math.PI / 180;
            var lon2 = coordinate2.Longitude * Math.PI / 180;
            var lat1 = coordinate1.Latitude * Math.PI / 180;
            var lat2 = coordinate2.Latitude * Math.PI / 180;

            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            return c * EarthRadiusInKilometer;
        }

        public static double CalculateInMile(Coordinate coordinate1, Coordinate coordinate2)
        {
            return CalculateInKilometer(coordinate1, coordinate2) * OneKilometerInMile;
        }
    }
}