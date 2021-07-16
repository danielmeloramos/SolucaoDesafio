using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Validators.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Tolls.Validators")]
    public class TollEmissionCommandValidatorTest
    {
        private TollEmissionCommandValidator _validator;
        private TollEmissionCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollEmissionCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollEmissionCommandObjectMother.TollEmissionCommand;

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirUmComandoValido()
        {
            // Arrange – Action
            var result = _validator.Validate(_command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoCarrierDocumentNumberForVazio()
        {
            _command.CarrierDocumentNumber = string.Empty;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.CarrierDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode ser vazio.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoCarrierDocumentNumberForUmNumeroDeDocumentoInvalido()
        {
            _command.CarrierDocumentNumber = new string('*', 14);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.CarrierDocumentNumber, _command)
                .WithErrorMessage("Número de documento está inválido.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoCarrierDocumentNumberExcederOTamanhoDoCampo()
        {
            _command.CarrierDocumentNumber = new string('*', 15);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.CarrierDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode possuir mais de 14 caracteres.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoCarrierDocumentNumberForMenorQueOTamanhoDoCampo()
        {
            _command.CarrierDocumentNumber = new string('*', 10);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.CarrierDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode possuir menos de 11 caracteres.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoConductorDocumentNumberForVazio()
        {
            _command.ConductorDocumentNumber = string.Empty;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ConductorDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode ser vazio.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoConductorDocumentNumberForUmCpfInvalido()
        {
            _command.ConductorDocumentNumber = new string('*', 11);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ConductorDocumentNumber, _command)
                .WithErrorMessage("CPF está inválido.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoConductorDocumentNumberExcederOTamanhoDoCampo()
        {
            _command.ConductorDocumentNumber = new string('*', 12);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ConductorDocumentNumber, _command)
                .WithErrorMessage("Número de documento deve possuir 11 caracteres.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoConductorDocumentNumberForMenorQueOTamanhoDoCampo()
        {
            _command.ConductorDocumentNumber = new string('*', 10);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ConductorDocumentNumber, _command)
                .WithErrorMessage("Número de documento deve possuir 11 caracteres.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoShipperDocumentNumberForNulo()
        {
            _command.ShipperDocumentNumber = null;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ShipperDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode ser nulo.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoShipperDocumentNumberForVazio()
        {
            _command.ShipperDocumentNumber = string.Empty;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ShipperDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode ser vazio.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoShipperDocumentNumberForUmNumeroDeDocumentoInvalido()
        {
            _command.ShipperDocumentNumber = new string('*', 14);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ShipperDocumentNumber, _command)
                .WithErrorMessage("Número de documento está inválido.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoShipperDocumentNumberExcederOTamanhoDoCampo()
        {
            _command.ShipperDocumentNumber = new string('*', 15);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ShipperDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode possuir mais de 14 caracteres.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoShipperDocumentNumberForMenorQueOTamanhoDoCampo()
        {
            _command.ShipperDocumentNumber = new string('*', 10);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.ShipperDocumentNumber, _command)
                .WithErrorMessage("Número de documento não pode possuir menos de 11 caracteres.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehiclePlateForNulo()
        {
            _command.VehiclePlate = null;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehiclePlate, _command)
                .WithErrorMessage("Placa do veículo não pode ser nula.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehiclePlateForVazio()
        {
            _command.VehiclePlate = string.Empty;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehiclePlate, _command)
                .WithErrorMessage("Placa do veículo não pode ser vazia.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehiclePlatePossuirExcederOTamanhoDoCampo()
        {
            _command.VehiclePlate = new string('*', 8);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehiclePlate, _command)
                .WithErrorMessage("Placa do veículo deve possuir 7 caracteres.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehiclePlateNaoObedecerOPadraoEsperado()
        {
            _command.VehiclePlate = new string('*', 7);

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehiclePlate, _command)
                .WithErrorMessage($"Placa do veículo {_command.VehiclePlate} está fora do padrão esperado. Espera-se o padrão ABC1234 ou ABC1D23.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehicleAxisNaoForDoTipoVehicleAxisEnum()
        {
            _command.VehicleAxis = 0;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehicleAxis, _command)
                .WithErrorMessage("Eixo veicular está inválido.");
        }
        
        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoVehicleAxisNaoForUmVehicleAxisEnum()
        {
            _command.VehicleAxis = 0;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.VehicleAxis, _command)
                .WithErrorMessage("Eixo veicular está inválido.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoTollPlazasForUmaListaNula()
        {
            _command.TollPlazas = null;

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.TollPlazas, _command)
                .WithErrorMessage("Praças de pedágio é obrigatório.");
        }

        [Test]
        public void Test_TollEmissionCommandValidator_DeveriaPossuirErro_QuandoTollPlazasForVazio()
        {
            _command.TollPlazas = new List<TollPlazaCommand>() { };

            _validator
                .ShouldHaveValidationErrorFor(tollEmissionCommand => tollEmissionCommand.TollPlazas, _command)
                .WithErrorMessage("Praças de pedágio é obrigatório.");
        }
    }
}
