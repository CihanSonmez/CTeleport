using CTeleport.Application.Features.Distance.Enums;

namespace CTeleport.Application.Features.Distance.DTOs.Responses
{
    public class DistanceResponse
    {
        public double Distance { get; set; }
        public DistanceUnit Unit { get; set; }
    }
}
