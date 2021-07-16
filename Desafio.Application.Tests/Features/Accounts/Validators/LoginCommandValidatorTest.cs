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
    public class LoginCommandValidatorTest
    {
        private LoginCommandValidator _validator;
        private LoginCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new LoginCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = LoginCommandObjectMother.LoginCommand;

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoNomeDeUsuarioNulo()
        {
            //Arrange
            _command.UserName = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.UserName, _command).WithErrorMessage("Nome é obrigatório.");
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoNomeDeUsuarioVazio()
        {
            //Arrange
            _command.UserName = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.UserName, _command).WithErrorMessage("Nome é obrigatório.");
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoNomeDeUsuarioSemComQuantidadeDeCaracteresSuperiorACinquenta()
        {
            //Arrange
            _command.UserName = new string('*', 51);

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.UserName, _command).WithErrorMessage("Nome excedeu o limite máximo de 50 caracteres.");
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoSenhaDeUsuarioNulo()
        {
            //Arrange
            _command.Password = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.Password, _command).WithErrorMessage("Senha é obrigatória.");
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoSenhaDeUsuarioVazio()
        {
            //Arrange
            _command.Password = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.Password, _command).WithErrorMessage("Senha é obrigatória.");
        }

        [Test]
        public void Test_LoginCommandValidator_DeveriaPossuirErroQuandoInformadoSenhaDeUsuarioSemComQuantidadeDeCaracteresSuperiorAVinte()
        {
            //Arrange
            _command.Password = new string('*', 21); ;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(login => login.Password, _command).WithErrorMessage("Senha excedeu o limite máximo de 20 caracteres.");
        }
    }
}
