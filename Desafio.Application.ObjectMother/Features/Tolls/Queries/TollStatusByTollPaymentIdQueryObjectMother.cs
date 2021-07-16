using NddCargo.Integration.Application.Features.Tolls.Queries;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Queries
{
    public class TollStatusByTollPaymentIdQueryObjectMother
    {
        public static TollStatusByTollPaymentIdQuery TollStatusByTollPaymentIdQuery => new TollStatusByTollPaymentIdQuery
        {
            TollPaymentId = "123456"
        };
    }
}
