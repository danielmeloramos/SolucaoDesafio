using NddCargo.Integration.Application.Features.TollLoadHistories.Queries;

namespace NddCargo.Integration.Application.ObjectMother.Features.ConsultUnloadBalances.Queries
{
    public class ConsultUnloadBalanceQueryObjectMother
    {
        public static ValueToUnloadQuery ValueToUnloadQuery => new ValueToUnloadQuery
        {
            CardHolderCpf = "83302839090"
        };
    }
}
