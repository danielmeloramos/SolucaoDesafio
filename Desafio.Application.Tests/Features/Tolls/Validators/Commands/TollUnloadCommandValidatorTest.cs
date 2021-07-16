using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NddCargo.Integration.Core.Common;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Validators.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Aplication.Toll.Commands")]
    public class TollUnloadCommandValidatorTest
    {
        private TollUnloadCommandValidator _validator;
        private TollUnloadCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollUnloadCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollUnloadCommandObjectMother.TollUnloadCommand;

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmCommandValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmCertificadoS1Nulo()
        {
            //Arrange
            _command.CertifiedS1 = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CertifiedS1, _command).WithErrorMessage("Certificado S1 é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmCertificadoS1Vazio()
        {
            //Arrange
            _command.CertifiedS1 = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CertifiedS1, _command).WithErrorMessage("Certificado S1 é obrigatório.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmDocumentNumberCardHolderInvalido()
        {
            //Arrange
            _command.DocumentNumberCardHolder = 0;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.DocumentNumberCardHolder, _command).WithErrorMessage("CPF está inválido.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmDocumentNumberOperationMenorQueZero()
        {
            //Arrange
            _command.DocumentNumberOperation = -1;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.DocumentNumberOperation, _command).WithErrorMessage("Número do documento da operação tem que ser maior que zero.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmDocumentNumberExchangeStationMenorQueZero()
        {
            //Arrange
            _command.DocumentNumberExchangeStation = -1;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.DocumentNumberExchangeStation, _command).WithErrorMessage("Número do documento do posto de troca tem que ser maior que zero.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmTypeDocumentInvalido()
        {
            //Arrange
            _command.TypeDocument = (TypeDocument)200;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.TypeDocument, _command).WithErrorMessage("O tipo do documento está inválido.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoInformadoUmValueNegativo()
        {
            //Arrange
            _command.Value = -1;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.Value, _command).WithErrorMessage("O valor não pode ser negativo.");
        }

        [Test]
        public void Test_TollUnloadCommandValidator_DeveriaPossuirUmErroQuandoNaoInformadoOsDadosDoCartao()
        {
            //Arrange
            _command.TollUnloadCardData = null;
  
            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.TollUnloadCardData, _command).WithErrorMessage("Os dados do cartão são obrigatórios.");
        }
    }
}
