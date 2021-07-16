using NddCargo.Integration.Application.Features.Accounts.Commands;

namespace NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands
{
    public class LogoutCommandObjectMother
    {
        public static LogoutCommand LogoutCommand => new LogoutCommand("personal");
    }
}
