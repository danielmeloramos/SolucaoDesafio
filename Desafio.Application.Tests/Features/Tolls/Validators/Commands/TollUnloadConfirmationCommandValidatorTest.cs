using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NUnit.Framework;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Validators.Commands
{
    public class TollUnloadConfirmationCommandValidatorTest
    {
        private TollUnloadConfirmationCommandValidator _validator;
        private TollUnloadConfirmationCommand _command;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollUnloadConfirmationCommandValidator();

        [SetUp]
        public void PrepareEnvironment() => _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;

        [Test]
        public void Test_TollUnloadConfirmationCommand_DeveriaPossuirUmComandoValido()
        {
            // Action
            var result = _validator.Validate(_command);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollUnloadConfirmationCommandValidator_DeveriaPossuirErro_QuandoIdentifierUnloadForMenorQueUm()
        {
            _command.IdentifierUnload = 0;

            _validator
                .ShouldHaveValidationErrorFor(tollUnloadConfirmationCommand => tollUnloadConfirmationCommand.IdentifierUnload, _command)
                .WithErrorMessage("Identificador da descarga deve ser maior que 0.");
        }
    }
}
