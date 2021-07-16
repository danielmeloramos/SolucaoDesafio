using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Routes.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Routes.Validators.Commands
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Routes.Validators.Commands")]
    public class RouteAddressCoordinateCommandValidatorTest
    {
        private RouteAddressCoordinateCommandValidator _validator;
        private RouteAddressCoordinateCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new RouteAddressCoordinateCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = RouteAddressCoordinateCommandObjectMother.RouteAddressCoordinateCommand;

        [Test]
        public void Test_RouteAddressCoordinateCommandValidator_DeveriaPossuirUmComandoValido()
        {
            //Action
            var result = _validator.Validate(_command);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_RouteAddressCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoLatitudeForMenorQueMenosNoventa()
        {
            //Arrange
            _command.Latitude = -89;

            //Action - Assert
           // _validator.ShouldHaveValidationErrorFor(x => x.Latitude, _command).WithErrorMessage("Latitude deve ser maior ou igual a -90º.");
        }

        [Test]
        public void Test_RouteAddressCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoLatitudeForMenorMaiorQueNoventa()
        {
            //Arrange
            _command.Latitude = 91;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Latitude, _command).WithErrorMessage("Latitude deve ser menor ou igual a 90º.");
        }

        [Test]
        public void Test_RouteAddressCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoLongitudeForMenorQueMenosCentoEOitenta()
        {
            //Arrange
            _command.Longitude = -179;

            //Action - Assert
            // _validator.ShouldHaveValidationErrorFor(x => x.Longitude, _command).WithErrorMessage("Longitude deve ser maior ou igual a -180º.");
        }

        [Test]
        public void Test_RouteAddressCoordinateCommandValidator_DeveriaPossuirErroQuandoInformadoLongitudeForMenorMaiorQueCentoEOitenta()
        {
            //Arrange
            _command.Longitude = 181;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(x => x.Longitude, _command).WithErrorMessage("Longitude deve ser menor ou igual a 180º.");
        }
    }
}
