using NddCargo.Integration.Application.Features.Tolls.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollPlazaCommandObjectMother
    {
        public static TollPlazaCommand TollPlazaCommand => new TollPlazaCommand
        {
            Cnp = "35353103464040103",
            Name = "Pedagio Agulha Norte",
            Value = 34.80M
        };
    }
}
