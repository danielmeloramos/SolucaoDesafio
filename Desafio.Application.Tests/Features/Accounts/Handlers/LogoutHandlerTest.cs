using FluentAssertions;
using Moq;
using NddCargo.Integration.Application.Features.Accounts.Commands;
using NddCargo.Integration.Application.Features.Accounts.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Features.ResponseRequests;
using NddCargo.Integration.Infra.Auths;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Accounts.Handlers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Accounts.Handlers")]
    public class LogoutHandlerTest : TestBase
    {
        private LogoutHandler _handler;
        private LogoutCommand _command;

        private readonly Mock<IJwtAuthManager> _fakeJwtAuthManager;

        public LogoutHandlerTest() => _fakeJwtAuthManager = new Mock<IJwtAuthManager>();

        [SetUp]
        public void Initialize()
        {
            _fakeJwtAuthManager.Reset();
            _handler = new LogoutHandler(_fakeJwtAuthManager.Object);
        }

        [Test]
        public async Task Test_LogoutHandler_DeveriaRealizarLogoutDoUsuario()
        {
            //Arrange
            _command = LogoutCommandObjectMother.LogoutCommand;
            _fakeJwtAuthManager.Setup(s => s.RemoveRefreshTokenByUserName(It.IsAny<string>())).Verifiable();

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<SuccessResponseRequest>();
            _fakeJwtAuthManager.Verify(s => s.RemoveRefreshTokenByUserName(It.IsAny<string>()), Times.Once);
        }
    }
}
