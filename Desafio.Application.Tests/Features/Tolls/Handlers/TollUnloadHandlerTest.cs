using FluentAssertions;

using Moq;

using ndd.SharedKernel.Result;

using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.Features.Tolls.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.DataCenterIntegration.Features.Tolls;
using NddCargo.Integration.DataCenterIntegration.Features.Tolls.Commands;
using NddCargo.Integration.DataCenterIntegration.ObjectMother.Features.Tolls.Dto;
using NddCargo.Integration.Domain.Features.CardHolders;
using NddCargo.Integration.Domain.Features.ExchangeStations;
using NddCargo.Integration.Domain.Features.TollUnloadProtocols;
using NddCargo.Integration.Domain.ObjectMother.Features.CardHolders;
using NddCargo.Integration.Domain.ObjectMother.Features.ExchangeStations;
using NddCargo.Integration.Domain.ObjectMother.Features.TollUnloadProtocols;

using NUnit.Framework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Handlers
{
    [TestFixture]
    [Category("Application.Tolls.Handlers")]
    public class TollUnloadHandlerTest : TestBase
    {
        private TollUnloadHandler _handler;
        private TollUnloadCommand _command;

        private readonly Mock<ITollService> _fakeTollService;
        private readonly Mock<ITollUnloadProtocolRepository> _fakeTollUnloadRepository;
        private readonly Mock<IExchangeStationRepository> _fakeExchangeStationRepository;
        private readonly Mock<ICardHolderRepository> _fakeCardHolderRepository;

        public TollUnloadHandlerTest()
        {
            _fakeTollService = new Mock<ITollService>();
            _fakeTollUnloadRepository = new Mock<ITollUnloadProtocolRepository>();
            _fakeExchangeStationRepository = new Mock<IExchangeStationRepository>();
            _fakeCardHolderRepository = new Mock<ICardHolderRepository>();
        }

        [SetUp]
        public void Initialize()
        {
            _fakeTollService.Reset();
            _fakeTollUnloadRepository.Reset();
            _fakeExchangeStationRepository.Reset();
            _fakeCardHolderRepository.Reset();

            _handler = new TollUnloadHandler(_fakeTollService.Object,
                                            _fakeTollUnloadRepository.Object,
                                            _fakeExchangeStationRepository.Object,
                                            _fakeCardHolderRepository.Object);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRealizarOEstornoDePedagioQuandoForValido()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var exchangeStation = ExchangeStationObjectMother.ExchangeStation;
            var cardHolder = CardHolderObjectMother.CardHolder;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(protocolReversalToll);
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(exchangeStation);
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(cardHolder);
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(reverseTollValue);
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Unit());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<TollUnloadProtocol>();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Once);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Once);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Once);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRetornarFalhaQuandoNaoEncontrarPostoDeAbastecimentoPeloNumeroDoDocumento()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var cardHolder = CardHolderObjectMother.CardHolder;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(protocolReversalToll);
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(new Exception());
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(cardHolder);
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(reverseTollValue);
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Unit());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Never);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Never);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Never);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Never);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRetornarFalhaQuandoNaoConseguirAdicionarOProtocoloDeEstornoDePedagio()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var exchangeStation = ExchangeStationObjectMother.ExchangeStation;
            var cardHolder = CardHolderObjectMother.CardHolder;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(new Exception());
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(exchangeStation);
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(cardHolder);
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(reverseTollValue);
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Unit());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Once);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Never);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Never);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Never);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRetornarFalhaQuandoNaoEncontrarOPortadorDoCartaoPeloNumeroDoDocumento()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var exchangeStation = ExchangeStationObjectMother.ExchangeStation;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(protocolReversalToll);
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(exchangeStation);
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(new Exception());
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(reverseTollValue);
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Unit());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Once);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Never);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Never);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRetornarFalhaQuandoNaoConseguirIntegrarComODataCenterApi()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var exchangeStation = ExchangeStationObjectMother.ExchangeStation;
            var cardHolder = CardHolderObjectMother.CardHolder;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(protocolReversalToll);
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(exchangeStation);
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(cardHolder);
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(new Exception());
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Unit());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Once);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Once);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Never);
        }

        [Test]
        public async Task Test_TollUnloadHandler_DeveriaRetornarFalhaQuandoNaoConseguirAtualizarOProtocoloDeEstornoDePedagio()
        {
            //Arrange
            _command = TollUnloadCommandObjectMother.TollUnloadCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var exchangeStation = ExchangeStationObjectMother.ExchangeStation;
            var cardHolder = CardHolderObjectMother.CardHolder;
            var reverseTollValue = ReverseTollDtoObjectMother.ReverseTollDto;
            _fakeTollUnloadRepository.Setup(s => s.AddAsync(It.IsAny<TollUnloadProtocol>())).ReturnsAsync(protocolReversalToll);
            _fakeExchangeStationRepository.Setup(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(exchangeStation);
            _fakeCardHolderRepository.Setup(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>())).ReturnsAsync(cardHolder);
            _fakeTollService.Setup(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>())).ReturnsAsync(reverseTollValue);
            _fakeTollUnloadRepository.Setup(s => s.UpdateAsync()).ReturnsAsync(new Exception());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollUnloadRepository.Verify(s => s.AddAsync(It.IsAny<TollUnloadProtocol>()), Times.Once);
            _fakeExchangeStationRepository.Verify(s => s.GetExchangeStationByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeCardHolderRepository.Verify(s => s.GetCardHolderByPersonDocumentNumber(It.IsAny<long>()), Times.Once);
            _fakeTollService.Verify(s => s.TollUnload(It.IsAny<ReverseTollValueCommand>()), Times.Once);
            _fakeTollUnloadRepository.Verify(s => s.UpdateAsync(), Times.Once);
        }
    }
}