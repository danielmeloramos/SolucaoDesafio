using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NddCargo.Integration.Application.Features.Accounts.Commands;
using NddCargo.Integration.Application.Features.Accounts.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Exceptions;
using NddCargo.Integration.Domain.Features.Accounts;
using NddCargo.Integration.Domain.ObjectMother.Features.Accounts;
using NddCargo.Integration.Infra.Auths;
using NddCargo.Integration.Infra.ObjectMother.Features.Auth;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Accounts.Handlers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Accounts.Handlers")]
    public class LoginHandlerTest : TestBase
    {
        private LoginHandler _handler;
        private LoginCommand _command;

        private readonly Mock<IJwtAuthManager> _fakeJwtAuthManager;
        private readonly Mock<IUserRepository> _fakeUserRepository;

        public LoginHandlerTest()
        {
            _fakeJwtAuthManager = new Mock<IJwtAuthManager>();
            _fakeUserRepository = new Mock<IUserRepository>();
        }

        [SetUp]
        public void Initialize()
        {
            _fakeJwtAuthManager.Reset();
            _fakeUserRepository.Reset();

            _handler = new LoginHandler(_fakeJwtAuthManager.Object, _fakeUserRepository.Object);
        }

        [Test]
        public async Task Test_LoginHandler_DeveriaRealizarAutenticacaoDoUsuarioQuandoForValido()
        {
            //Arrange
            _command = LoginCommandObjectMother.LoginCommand;
            var user = UserObjectMother.User;
            var jwtAuthResult = JwtAuthResultObjectMother.JwtAuthResult;

            _fakeUserRepository.Setup(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _fakeJwtAuthManager.Setup(s => s.GenerateTokenAsync(It.IsAny<string>(), It.IsAny<Claim[]>(), It.IsAny<DateTime>())).ReturnsAsync(jwtAuthResult);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<JwtAuthResult>();
            _fakeUserRepository.Verify(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fakeJwtAuthManager.Verify(s => s.GenerateTokenAsync(It.IsAny<string>(), It.IsAny<Claim[]>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public async Task Test_LoginHandler_DeveriaNaoRealizarAutenticacaoDoUsuarioQuandoNaoEncontrarOUsuario()
        {
            //Arrange
            _command = LoginCommandObjectMother.LoginCommand;
            var notFoundException = new NotFoundException();
            _fakeUserRepository.Setup(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(notFoundException);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<NotFoundException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeUserRepository.Verify(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fakeJwtAuthManager.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Test_LoginHandler_DeveriaNaoRealizarAutenticacaoDoUsuarioQuandoFalharAoGerarToken()
        {
            //Arrange
            _command = LoginCommandObjectMother.LoginCommand;
            var user = UserObjectMother.User;
            var securityTokenException = new SecurityTokenException();
            _fakeUserRepository.Setup(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
            _fakeJwtAuthManager.Setup(s => s.GenerateTokenAsync(It.IsAny<string>(), It.IsAny<Claim[]>(), It.IsAny<DateTime>())).ReturnsAsync(securityTokenException);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<InvalidCredentialsException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeUserRepository.Verify(s => s.GetByUserNameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fakeJwtAuthManager.Verify(s => s.GenerateTokenAsync(It.IsAny<string>(), It.IsAny<Claim[]>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
