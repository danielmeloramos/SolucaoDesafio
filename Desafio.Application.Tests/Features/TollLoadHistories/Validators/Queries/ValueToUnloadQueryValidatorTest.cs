using FluentAssertions;
using FluentValidation.TestHelper;
using NddCargo.Integration.Application.Features.TollLoadHistories.Queries;
using NddCargo.Integration.Application.ObjectMother.Features.ConsultUnloadBalances.Queries;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Features.ConsultRefundBalances.Validators.Queries
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.TollLoadHistories.Validators.Queries")]
    public class ValueToUnloadQueryValidatorTest
    {
        private ValueToUnloadQueryValidator _validator;
        private ValueToUnloadQuery _query;

        [OneTimeSetUp]
        public void Initialize() => _validator = new ValueToUnloadQueryValidator();

        [SetUp]
        public void PrepareEnvironment() => _query = ConsultUnloadBalanceQueryObjectMother.ValueToUnloadQuery;

        [Test]
        public void Test_ConsultRefundBalanceQueryValidator_DeveriaPossuirUmaQueryValida()
        {
            //Action
            var result = _validator.Validate(_query);

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public void Test_ConsultRefundBalanceQueryValidator_DeveriaPossuirUmErroQuandoInformadoUmCpfNulo()
        {
            //Arrange
            _query.CardHolderCpf = null;

            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CardHolderCpf, _query).WithErrorMessage("CPF do portador do cartão não pode ser nulo.");
        }

        [Test]
        public void Test_ConsultRefundBalanceQueryValidator_DeveriaPossuirUmErroQuandoInformadoUmCpfVazio()
        {
            //Arrange
            _query.CardHolderCpf = String.Empty;
            //Action - Assert
            _validator.ShouldHaveValidationErrorFor(find => find.CardHolderCpf, _query).WithErrorMessage("CPF do portador do cartão não pode ser vazio.");
        }
    }
}
