using NddCargo.Integration.Application.Features.Tolls.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands
{
    public class TollUnloadConfirmationCommandObjectMother
    {
        public static TollUnloadConfirmationCommand TollUnloadConfirmationCommand => new TollUnloadConfirmationCommand
        {
            CertificateS3 = "123456789",
            IdentifierUnload = 987654321,
            IsConfirmed = true
        };
    }
}
