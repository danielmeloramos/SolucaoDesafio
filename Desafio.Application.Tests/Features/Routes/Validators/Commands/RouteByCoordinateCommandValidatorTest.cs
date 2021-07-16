using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Routes.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Routes;
using NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands;
using NddCargo.Integration.Core.Common;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Routes.Validators.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Routes.Validators.Commands")]
    public class RouteByCoordinateCommandValidatorTest
    {
        private RouteByCoordinateCommandValidator _validator;
        private RouteByCoordinateCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new RouteByCoordinateCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = RouteByCoordinateCommandObjectMother.RouteByCoordinateCommand;

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoEixoVeicularInvalido()
        {
            //Arrange
            _command.VehicleAxis = (VehicleAxis)200;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.VehicleAxis, _command).WithErrorMessage("Eixo veicular é inválido.");
        }

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoPontosDeParadaForVazio()
        {
            //Arrange
            _command.Points = new List<RouteAddressCoordinateCommand>();

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Points, _command).WithErrorMessage("Lista de endereços do Roteirizador é obrigatória.");
        }

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoPontosDeParadaForNulo()
        {
            //Arrange
            _command.Points = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Points, _command).WithErrorMessage("Lista de endereços do Roteirizador é obrigatória.");
        }

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoSomenteUmPontoDeParada()
        {
            //Arrange
            _command.Points = new List<RouteAddressCoordinateCommand>() { RouteAddressCoordinateCommandObjectMother.RouteAddressCoordinateCommand };

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Points, _command).WithErrorMessage("Deve haver no mínimo dois pontos de paradas.");
        }

        [Test]
        public void Test_RouteByCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoPontoDeOrigemForIgualAoPontoDeDestino()
        {
            //Arrange
            var points = RouteAddressCoordinateCommandObjectMother.RouteAddressCoordinateCommand;
            _command.Points = new List<RouteAddressCoordinateCommand>() { points, points };

            //Action - Assert
            _validator
                .ShouldHaveValidationErrorFor(x => x.Points, _command)
                .WithErrorMessage($"O ponto de destino com Latitude {points.Latitude} e Longitude {points.Longitude} deve ser diferente do ponto de origem.");
        }
    }
}
