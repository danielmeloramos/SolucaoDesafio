using FluentAssertions;
using Moq;
using ndd.SharedKernel.Result;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.Features.Tolls.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.DataCenterIntegration.Features.Tolls;
using NddCargo.Integration.DataCenterIntegration.Features.Tolls.Commands;
using NddCargo.Integration.Domain.Features.TollLoadHistories;
using NddCargo.Integration.Domain.Features.TollUnloadProtocols;
using NddCargo.Integration.Domain.ObjectMother.Features.TollLoadHistories;
using NddCargo.Integration.Domain.ObjectMother.Features.TollUnloadProtocols;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Handlers
{
    public class TollUnloadConfirmationHandlerTest : TestBase
    {
        private TollUnloadConfirmationHandler _handler;
        private TollUnloadConfirmationCommand _command;

        private readonly Mock<ITollUnloadProtocolRepository> _fakeProtocolReversalTollRepository;
        private readonly Mock<ITollService> _fakeTollService;
        private readonly Mock<ITollLoadHistoryRepository> _fakeTollLoadHistoryRepository;

        public TollUnloadConfirmationHandlerTest()
        {
            _fakeProtocolReversalTollRepository = new Mock<ITollUnloadProtocolRepository>();
            _fakeTollService = new Mock<ITollService>();
            _fakeTollLoadHistoryRepository = new Mock<ITollLoadHistoryRepository>();
        }

        [SetUp]
        public void Initialize()
        {
            _fakeProtocolReversalTollRepository.Reset();
            _handler = new TollUnloadConfirmationHandler(_fakeTollService.Object, _fakeProtocolReversalTollRepository.Object, _fakeTollLoadHistoryRepository.Object);
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRealizarAConfirmacaoDoEstornoDeValePedagio()
        {
            // Arrange
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            var reverseTollValueConfirmationCommand = ReverseTollValueConfirmationCommandObjectMother.ReverseTollValueConfirmationCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;
            var tollLoadHistories = new List<TollLoadHistory>()
            {
                TollLoadHistoryObjectMother.TollLoadHistory
            }.AsQueryable();

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(protocolReversalToll);

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync())
                .ReturnsAsync(Unit.Successful);

            _fakeTollService
                .Setup(tollService => tollService.ConfirmReverseTollValue(reverseTollValueConfirmationCommand))
                .ReturnsAsync(Unit.Successful);

            _fakeTollLoadHistoryRepository
                .Setup(tollLoadHistoryRepository => tollLoadHistoryRepository.GetTollLoadHistories(It.IsAny<long>(), It.IsAny<int>()))
                .Returns(tollLoadHistories);

            _fakeTollLoadHistoryRepository
                 .Setup(tollLoadHistoryRepository => tollLoadHistoryRepository.UpdateAsync())
                 .Verifiable();

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<TollUnloadProtocol>();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync(), Times.Exactly(3));
            _fakeTollService.Verify(tollService => tollService.ConfirmReverseTollValue(It.IsAny<ReverseTollValueConfirmationCommand>()), Times.Once);
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRetornarException_QuandoNaoEncontrarOProtocoloDeCargaDePedagio()
        {
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            _command.IdentifierUnload = long.MaxValue;

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new Exception());

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRetornarException_QuandoNaoConseguirAtualizarOProtocoloDeCargaDePedagio()
        {
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(protocolReversalToll);

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync())
                .ReturnsAsync(new Exception());

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync(), Times.Once);
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRetornarException_QuandoNaoConseguirRealizarAConfirmacaoDoEstornoDeValePedagio()
        {
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(protocolReversalToll);

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync())
                .ReturnsAsync(Unit.Successful);

            _fakeTollService
                .Setup(tollService => tollService.ConfirmReverseTollValue(It.IsAny<ReverseTollValueConfirmationCommand>()))
                .ReturnsAsync(new Exception());

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync(), Times.Exactly(2));
            _fakeTollService.Verify(tollService => tollService.ConfirmReverseTollValue(It.IsAny<ReverseTollValueConfirmationCommand>()), Times.Exactly(3));
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRetornarException_QuandoNaoConseguirAtualizarOStatusDoProtocoloDeCargaDePedagioParaS3Sent()
        {
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(protocolReversalToll);

            _fakeProtocolReversalTollRepository
                .SetupSequence(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync())
                .ReturnsAsync(Unit.Successful)
                .ReturnsAsync(new Exception());

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync(), Times.Exactly(2));
        }

        [Test]
        public async Task Test_TollUnloadConfirmationHandler_DeveriaRetornarException_QuandoNaoConseguirAtualizarOStatusDoProtocoloDeCargaDePedagioParaFinished()
        {
            _command = TollUnloadConfirmationCommandObjectMother.TollUnloadConfirmationCommand;
            var reverseTollValueConfirmationCommand = ReverseTollValueConfirmationCommandObjectMother.ReverseTollValueConfirmationCommand;
            var protocolReversalToll = TollUnloadProtocolObjectMother.TollUnloadProtocol;

            _fakeProtocolReversalTollRepository
                .Setup(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(protocolReversalToll);

            _fakeProtocolReversalTollRepository
                .SetupSequence(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync())
                .ReturnsAsync(Unit.Successful)
                .ReturnsAsync(Unit.Successful)
                .ReturnsAsync(new Exception());

            _fakeTollService
                .Setup(tollService => tollService.ConfirmReverseTollValue(reverseTollValueConfirmationCommand))
                .ReturnsAsync(Unit.Successful);

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.GetAsync(It.IsAny<int>(), It.IsAny<long>()), Times.Once);
            _fakeProtocolReversalTollRepository.Verify(protocolReversalTollRepository => protocolReversalTollRepository.UpdateAsync(), Times.Exactly(3));
            _fakeTollService.Verify(tollService => tollService.ConfirmReverseTollValue(It.IsAny<ReverseTollValueConfirmationCommand>()), Times.Exactly(2));
        }
    }
}
