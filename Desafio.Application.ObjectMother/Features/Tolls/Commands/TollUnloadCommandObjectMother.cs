using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Core.Common;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollUnloadCommandObjectMother
    {
        public static TollUnloadCommand TollUnloadCommand => new TollUnloadCommand
        {
            CertifiedS1 = "012345",
            //DocumentNumberCardHolder = 02177677025, //Erro com esse cpf
            DocumentNumberCardHolder = 37601889076,
            DocumentNumberExchangeStation = 1234567,
            DocumentNumberOperation = 2,
            TollErp = "123456",
            Value = 2,
            TypeDocument = TypeDocument.Ciot,
            TollUnloadCardData = TollUnloadCardDataCommandObjectMother.TollUnloadCardDataCommand
        };
    }
}
