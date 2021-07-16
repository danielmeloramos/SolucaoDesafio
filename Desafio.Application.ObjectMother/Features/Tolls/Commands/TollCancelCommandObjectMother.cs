using NddCargo.Integration.Application.Features.Tolls.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollCancelCommandObjectMother
    {
        public static TollCancelCommand TollValidCancelCommand => new TollCancelCommand
        {
            TollPaymentId = "123456"
        };
    }
}
