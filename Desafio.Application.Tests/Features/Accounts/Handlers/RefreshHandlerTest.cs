using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NddCargo.Integration.Application.Features.Accounts.Commands;
using NddCargo.Integration.Application.Features.Accounts.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Exceptions;
using NddCargo.Integration.Domain.ObjectMother.Features.Accounts;
using NddCargo.Integration.Infra.Auths;
using NddCargo.Integration.Infra.ObjectMother.Features.Auth;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Accounts.Handlers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Accounts.Handlers")]
    public class RefreshHandlerTest : TestBase
    {
        private RefreshHandler _handler;
        private RefreshTokenCommand _command;

        private readonly Mock<IJwtAuthManager> _fakeJwtAuthManager;

        public RefreshHandlerTest() => _fakeJwtAuthManager = new Mock<IJwtAuthManager>();

        [SetUp]
        public void Initialize()
        {
            _fakeJwtAuthManager.Reset();

            _handler = new RefreshHandler(_fakeJwtAuthManager.Object);
        }

        [Test]
        public async Task Test_LoginHandler_DeveriaRealizarAGeracaoDoTokenDeAtualizacaoQuandoForValido()
        {
            //Arrange
            _command = RefreshTokenCommandObjectMother.RefreshTokenCommand;
            var jwtAuthResult = JwtAuthResultObjectMother.JwtAuthResult;
            _fakeJwtAuthManager.Setup(s => s.Refresh(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(jwtAuthResult);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<JwtAuthResult>();
            _fakeJwtAuthManager.Verify(s => s.Refresh(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public async Task Test_LoginHandler_DeveriaNaoRealizarAGeracaoDoTokenDeAtualizacaoQuandoOcorrerAlgumaFalha()
        {
            //Arrange
            _command = RefreshTokenCommandObjectMother.RefreshTokenCommand;
            var securityTokenException = new SecurityTokenException();
            _fakeJwtAuthManager.Setup(s => s.Refresh(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(securityTokenException);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<InvalidCredentialsException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeJwtAuthManager.Verify(s => s.Refresh(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
