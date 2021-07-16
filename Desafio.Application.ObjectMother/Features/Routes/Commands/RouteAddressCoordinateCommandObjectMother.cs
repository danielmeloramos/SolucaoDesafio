using NddCargo.Integration.Application.Features.Routes.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands
{
    public class RouteAddressCoordinateCommandObjectMother
    {
        public static RouteAddressCoordinateCommand RouteAddressCoordinateCommand => new RouteAddressCoordinateCommand
        {
             Latitude = -27.8155,
             Longitude = -50.3264
        };

        public static RouteAddressCoordinateCommand RouteAddressCoordinateCommandTwo => new RouteAddressCoordinateCommand
        {
            Latitude = -20.85828,
            Longitude = -46.52217
        };
    }
}
