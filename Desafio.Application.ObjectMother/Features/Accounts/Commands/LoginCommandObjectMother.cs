using NddCargo.Integration.Application.Features.Accounts.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands
{
    public class LoginCommandObjectMother
    {
        public static LoginCommand LoginCommand => new LoginCommand
        {
            UserName = "personal.card",
            Password = "123456"
        };
    }
}
