using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Accounts.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Accounts.Commands;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Accounts.Validators
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Accounts.Validators")]
    public class RefreshTokenCommandValidatorTest
    {
        private RefreshTokenCommandValidator _validator;
        private RefreshTokenCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new RefreshTokenCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = RefreshTokenCommandObjectMother.RefreshTokenCommand;

        [Test]
        public void Test_RefreshTokenCommandValidator_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_RefreshTokenCommandValidator_DeveriaPossuirErroQuandoInformadoRefreshTokenNulo()
        {
            //Arrange
            _command.RefreshToken = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(refreshToken => refreshToken.RefreshToken, _command).WithErrorMessage("RefreshToken é obrigatório.");
        }

        [Test]
        public void Test_RefreshTokenCommandValidator_DeveriaPossuirErroQuandoInformadoRefreshTokenVazio()
        {
            //Arrange
            _command.RefreshToken = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(refreshToken => refreshToken.RefreshToken, _command).WithErrorMessage("RefreshToken é obrigatório.");
        }
    }
}
