using NddCargo.Integration.Application.Features.Routes.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands;
using NddCargo.Integration.Core.Common;
using System.Collections.Generic;

namespace NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands
{
    public class RouteByCoordinateCommandObjectMother
    {
        public static RouteByCoordinateCommand RouteByCoordinateCommand => new RouteByCoordinateCommand
        {
            Points = new List<RouteAddressCoordinateCommand>()
            {
                RouteAddressCoordinateCommandObjectMother.RouteAddressCoordinateCommand,
                RouteAddressCoordinateCommandObjectMother.RouteAddressCoordinateCommandTwo
            },
            VehicleAxis = VehicleAxis.VeiculoDePasseioDoisEixos
        };
    }
}
