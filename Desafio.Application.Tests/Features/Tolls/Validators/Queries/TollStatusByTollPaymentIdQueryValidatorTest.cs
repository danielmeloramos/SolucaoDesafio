using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.Tolls.Queries;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Queries;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Validators.Queries
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Tolls.Validators.Queries")]
    public class TollStatusByTollPaymentIdQueryValidatorTest
    {
        private TollStatusByTollPaymentIdQueryValidator _validator;
        private TollStatusByTollPaymentIdQuery _query;

        [OneTimeSetUp]
        public void Initialize() => _validator = new TollStatusByTollPaymentIdQueryValidator();

        [SetUp]
        public void PrepareEnvironment() => _query = TollStatusByTollPaymentIdQueryObjectMother.TollStatusByTollPaymentIdQuery;

        [Test]
        public void Test_TollStatusByTollPaymentIdQueryValidator_DeveriaPossuirUmaQueryValida()
        {
            //Action
            var result = _validator.Validate(_query);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_TollStatusByTollPaymentIdQueryValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloValorNulo()
        {
            //Arrange
            _query.TollPaymentId = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollFind => tollFind.TollPaymentId, _query).WithErrorMessage("Protocolo de pedágio é obrigatório.");
        }

        [Test]
        public void Test_TollStatusByTollPaymentIdQueryValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloValorVazio()
        {
            //Arrange
            _query.TollPaymentId = string.Empty;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollFind => tollFind.TollPaymentId, _query).WithErrorMessage("Protocolo de pedágio é obrigatório.");
        }

        [Test]
        public void Test_TollStatusByTollPaymentIdQueryValidator_DeveriaPossuirErroQuandoInformadoNumeroDeProtocoloNaoNumerico()
        {
            //Arrange
            _query.TollPaymentId = "abc";

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(tollFind => tollFind.TollPaymentId, _query).WithErrorMessage("Protocolo de pedágio não é um número válido.");
        }
        
    }
}
