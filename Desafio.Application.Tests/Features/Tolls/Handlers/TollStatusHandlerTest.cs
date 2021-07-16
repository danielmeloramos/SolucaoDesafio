using FluentAssertions;
using Moq;
using NddCargo.Integration.Application.Features.Tolls.Handlers;
using NddCargo.Integration.Application.Features.Tolls.Queries;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Queries;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Features.Tolls;
using NddCargo.Integration.Domain.ObjectMother.Features.Tolls;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Handlers
{
    [TestFixture]
    [Category("Application.Tolls.Handlers")]
    public class TollStatusHandlerTest : TestBase
    {
        private TollStatusHandler _handler;
        private TollStatusByTollPaymentIdQuery _query;

        private readonly Mock<ITollEmissionRepository> _fakeTollEmissionRepository;

        public TollStatusHandlerTest() => _fakeTollEmissionRepository = new Mock<ITollEmissionRepository>();

        [SetUp]
        public void Initialize()
        {
            _fakeTollEmissionRepository.Reset();

            _handler = new TollStatusHandler(_fakeTollEmissionRepository.Object);
        }

        [Test]
        public async Task Test_TollStatusHandler_DeveriaRealizarAConsultaDeStatusDaOperacaoDeValePedagio()
        {
            //Arrange
            _query = TollStatusByTollPaymentIdQueryObjectMother.TollStatusByTollPaymentIdQuery;
            var tollEmission = TollEmissionObjectMother.TollEmission;

            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(tollEmission);

            //Action
            var response = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<TollEmission>();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
