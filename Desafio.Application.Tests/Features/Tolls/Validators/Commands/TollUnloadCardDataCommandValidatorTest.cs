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
    [Category("Aplication.Toll.Commands")]
    public class TollUnloadCardDataCommandValidatorTest
    {
        private TollUnloadCardDataCommandValidator _validator;
        private TollUnloadCardDataCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollUnloadCardDataCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollUnloadCardDataCommandObjectMother.TollUnloadCardDataCommand;

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmCommandValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmaDataDeExpiracaoNula()
        {
            //Arrange
            _command.ExpirationDate = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.ExpirationDate, _command).WithErrorMessage("Data de expiração é obrigatória.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmaDataDeExpiracaoVazia()
        {
            //Arrange
            _command.ExpirationDate = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.ExpirationDate, _command).WithErrorMessage("Data de expiração é obrigatória.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmIdentificadorDoLeitorDeCartaoNulo()
        {
            //Arrange
            _command.IdentifierCardReader = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.IdentifierCardReader, _command).WithErrorMessage("A identificação do leitor de cartão é obrigatória.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmIdentificadorDoLeitorDeCartaoVazio()
        {
            //Arrange
            _command.IdentifierCardReader = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.IdentifierCardReader, _command).WithErrorMessage("A identificação do leitor de cartão é obrigatória.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmContadorinternoNulo()
        {
            //Arrange
            _command.InternalCounter = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.InternalCounter, _command).WithErrorMessage("Contador interno é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmContadorinternoVazio()
        {
            //Arrange
            _command.InternalCounter = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.InternalCounter, _command).WithErrorMessage("Contador interno é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmValorDoSaldoDoMoedeiroNulo()
        {
            //Arrange
            _command.CurrencyBalanceAmount = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CurrencyBalanceAmount, _command).WithErrorMessage("Valor do saldo do moedeiro é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmValorDoSaldoDoMoedeiroVazio()
        {
            //Arrange
            _command.CurrencyBalanceAmount = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CurrencyBalanceAmount, _command).WithErrorMessage("Valor do saldo do moedeiro é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmValorDoSaldoDoMoedeiroNaoNumerico()
        {
            //Arrange
            _command.CurrencyBalanceAmount = "oi";

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CurrencyBalanceAmount, _command).WithErrorMessage("Valor do saldo do moedeiro não é um número válido.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmNumeroContTxNulo()
        {
            //Arrange
            _command.NumberContTx = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.NumberContTx, _command).WithErrorMessage("Numero Cont TX é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCardDataCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmNumeroContTxVazio()
        {
            //Arrange
            _command.NumberContTx = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.NumberContTx, _command).WithErrorMessage("Numero Cont TX é obrigatório.");
        }
    }
}
