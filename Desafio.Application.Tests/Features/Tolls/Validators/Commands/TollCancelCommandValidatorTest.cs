using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Validators.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Tolls.Validators.Commands")]
    public class TollCancelCommandValidatorTest
    {
        private TollCancelCommandValidator _validator;
        private TollCancelCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollCancelCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollCancelCommandObjectMother.TollValidCancelCommand;

        [Test]
        public void Test_TollCancelCommandValidator_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollCancelCommandValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloNulo()
        {
            //Arrange
            _command.TollPaymentId = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollCancel => tollCancel.TollPaymentId, _command).WithErrorMessage("Protocolo de pedágio é obrigatório.");
        }

        [Test]
        public void Test_TollCancelCommandValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloComAspasVazias()
        {
            //Arrange
            _command.TollPaymentId = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollCancel => tollCancel.TollPaymentId, _command).WithErrorMessage("Protocolo de pedágio é obrigatório.");
        }

        [Test]
        public void Test_TollCancelCommandValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloNaoNumerico()
        {
            //Arrange
            _command.TollPaymentId = "abc";

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollCancel => tollCancel.TollPaymentId, _command).WithErrorMessage("Protocolo de pedágio não é um número válido.");
        }
    }
}
