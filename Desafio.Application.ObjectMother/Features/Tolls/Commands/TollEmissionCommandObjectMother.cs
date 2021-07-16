using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Core.Common;
using System.Collections.Generic;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollEmissionCommandObjectMother
    {
        public static TollEmissionCommand TollEmissionCommand => new TollEmissionCommand
        {
            CarrierDocumentNumber = "06255692000103",
            ConductorDocumentNumber = "98133010071",
            ShipperDocumentNumber = "06255692000103",
            TollPlazas = new List<TollPlazaCommand> { TollPlazaCommandObjectMother.TollPlazaCommand },
            VehicleAxis = VehicleAxis.VeiculoDePasseioDoisEixos,
            VehiclePlate = "MWM9622"
        };
    }
}
