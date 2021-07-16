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
    public class TollPlazaCommandValidatorTest
    {
        private TollPlazaCommandValidator _validator;
        private TollPlazaCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollPlazaCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollPlazaCommandObjectMother.TollPlazaCommand;

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoNomeNulo()
        {
            //Arrange
            _command.Name = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command).WithErrorMessage("Nome é obrigatório.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoNomeVazio()
        {
            //Arrange
            _command.Name = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command).WithErrorMessage("Nome é obrigatório.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoNomeComMaisDeDuzentosECinquantaECincoCaracteres()
        {
            //Arrange
            _command.Name = new string('*', 256); ;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Name, _command).WithErrorMessage("Nome deve possuir entre 1 e 255 caracteres.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_NaoDeveriaPossuirErroQuandoInformadoNomeComUmCaracter()
        {
            //Arrange
            _command.Name = "a";

            //Action - Assert
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, _command);
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_NaoDeveriaPossuirErroQuandoInformadoNomeComDuzentosECinquantaECincoCaracteres()
        {
            //Arrange
            _command.Name = new string('*', 255);

            //Action - Assert
            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, _command);
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoValorIgualAZero()
        {
            //Arrange
            _command.Value = 0.00M;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Value, _command).WithErrorMessage("Valor deve ter no mínimo R$ 0,01.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoValorSuperiorAoIntervaloDeQuinzeVirgulaDois()
        {
            //Arrange
            _command.Value = 12345678912345.12M;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Value, _command).WithErrorMessage("Valor contém uma quantidade de dígitos superior ao suportado.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoCnpNulo()
        {
            //Arrange
            _command.Cnp = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Cnp, _command).WithErrorMessage("Cnp é obrigatório.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoCnpVazio()
        {
            //Arrange
            _command.Cnp = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Cnp, _command).WithErrorMessage("Cnp é obrigatório.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoCnpMaiorQueDezesseteCaracateres()
        {
            //Arrange
            _command.Cnp = new string('*', 18);

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Cnp, _command).WithErrorMessage("Cnp deve ter 17 caracteres.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoCnpMenorQueDezesseteCaracateres()
        {
            //Arrange
            _command.Cnp = new string('*', 16);

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Cnp, _command).WithErrorMessage("Cnp deve ter 17 caracteres.");
        }

        [Test]
        public void Test_TollPlazaCommandValidatorTest_DeveriaPossuirErroQuandoInformadoCnpInvalido()
        {
            //Arrange
            _command.Cnp = "abc";

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Cnp, _command).WithErrorMessage("Cnp está inválido.");
        }
    }
}