using NddCargo.Integration.Application.Features.Tolls.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollUnloadCardDataCommandObjectMother
    {
        public static TollUnloadCardDataCommand TollUnloadCardDataCommand => new TollUnloadCardDataCommand
        {
            CurrencyBalanceAmount = "10",
            ExpirationDate = "2022-01-01",
            IdentifierCardReader = "1",
            InternalCounter = "2",
            NumberContTx = "123"
        };
    }
}
