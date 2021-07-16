using FluentAssertions;
using Moq;
using NddCargo.Integration.Application.Features.TollLoadHistories.Handlers;
using NddCargo.Integration.Application.Features.TollLoadHistories.Queries;
using NddCargo.Integration.Application.ObjectMother.Features.ConsultUnloadBalances.Queries;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Features.TollLoadHistories;
using NddCargo.Integration.Domain.ObjectMother.Features.TollLoadHistories;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.TollLoadHistories.Handlers
{
    [TestFixture]
    [Category("Application.TollLoadHistories.Handlers")]
    public class TollLoadHistoryHandlerTest : TestBase
    {
        private TollLoadHistoryHandler _handler;
        private ValueToUnloadQuery _query;

        private readonly Mock<ITollLoadHistoryRepository> _fakeTollLoadHistoryRepository;

        public TollLoadHistoryHandlerTest() => _fakeTollLoadHistoryRepository = new Mock<ITollLoadHistoryRepository>();

        [SetUp]
        public void Initialize()
        {
            _fakeTollLoadHistoryRepository.Reset();

            _handler = new TollLoadHistoryHandler(_fakeTollLoadHistoryRepository.Object);
        }

        [Test]
        public async Task Test_ConsultRefundBalanceHandler_DeveriaRealizarAConsultaDeValorParaEstornoDePedagio()
        {
            //Arrange
            _query = ConsultUnloadBalanceQueryObjectMother.ValueToUnloadQuery;
            IQueryable<TollLoadHistory> tollLoadHistories = new List<TollLoadHistory>()
            {
                TollLoadHistoryObjectMother.TollLoadHistory
            }.AsQueryable();

            _fakeTollLoadHistoryRepository.Setup(s => s.GetTollLoadHistory(It.IsAny<long>(), It.IsAny<int>())).Returns(tollLoadHistories);

            //Action
            var response = await _handler.Handle(_query, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            //response.Success.Should().BeOfType<IQueryable<TollLoadHistory>>();
            _fakeTollLoadHistoryRepository.Verify(s => s.GetTollLoadHistory(It.IsAny<long>(), It.IsAny<int>()), Times.Once);
        }
    }
}
