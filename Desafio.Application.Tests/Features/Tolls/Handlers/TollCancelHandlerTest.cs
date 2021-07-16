using FluentAssertions;
using MediatR;
using Moq;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.Features.Tolls.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Domain.Common;
using NddCargo.Integration.Domain.Exceptions;
using NddCargo.Integration.Domain.Features.Tolls;
using NddCargo.Integration.Domain.ObjectMother.Features.Tolls;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Handlers
{
    [TestFixture]
    [Category("Application.Tolls.Handlers")]
    public class TollCancelHandlerTest : TestBase
    {
        private TollCancelHandler _handler;
        private TollCancelCommand _command;

        private readonly Mock<ITollEmissionRepository> _fakeTollEmissionRepository;
        private readonly Mock<ITollCancelRepository> _fakeTollCancelRepository;
        private readonly Mock<IMediator> _fakeMediator;

        public TollCancelHandlerTest()
        {
            _fakeTollEmissionRepository = new Mock<ITollEmissionRepository>();
            _fakeTollCancelRepository = new Mock<ITollCancelRepository>();
            _fakeMediator = new Mock<IMediator>();
        }

        [SetUp]
        public void Initialize()
        {
            _fakeTollEmissionRepository.Reset();

            _handler = new TollCancelHandler(_fakeMediator.Object,_fakeTollEmissionRepository.Object, _fakeTollCancelRepository.Object);
        }

        [Test]
        public async Task Test_TollCancelHandler_DeveriaRealizarOCancelamentoDoValePedagio()
        {
            //Arrange
            _command = TollCancelCommandObjectMother.TollValidCancelCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            var tollCancel = TollCancelObjectMother.TollCancel;
            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(tollEmission);
            _fakeTollCancelRepository.Setup(s => s.AddAsync(It.IsAny<TollCancel>())).ReturnsAsync(tollCancel);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<SuccessfulRequest>();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Test_TollCancelHandler_DeveriaRetornarFalhaCasoADataDeValidadeTenhaExpirado()
        {
            //Arrange
            _command = TollCancelCommandObjectMother.TollValidCancelCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            tollEmission.ExpirationDate = DateTime.Now.AddDays(-1);
            var tollCancel = TollCancelObjectMother.TollCancel;
            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(tollEmission);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<HasExpirationDateException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Test_TollCancelHandler_DeveriaRetornarFalhaCasoNaoSejaEncontradoUmIdValido()
        {
            //Arrange
            _command = TollCancelCommandObjectMother.TollValidCancelCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            tollEmission.Id = Guid.NewGuid();
            var tollCancel = TollCancelObjectMother.TollCancel;
            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(new Exception());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Test_TollCancelHandler_DeveriaRetornarFalhaCasoOCancelamentoDeValePedagioJaEstejaCancelado()
        {
            //Arrange
            _command = TollCancelCommandObjectMother.TollValidCancelCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            tollEmission.Status = TollEmissionStatus.Canceled;
            var tollCancel = TollCancelObjectMother.TollCancel;
            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(tollEmission);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<HasTollCancelledException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Test_TollCancelHandler_DeveriaRetornarFalhaCasoOcorraAlgumErro()
        {
            //Arrange
            _command = TollCancelCommandObjectMother.TollValidCancelCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            var tollCancel = TollCancelObjectMother.TollCancel;
            _fakeTollEmissionRepository.Setup(s => s.GetByTollPaymentIdAsync(It.IsAny<string>())).ReturnsAsync(tollEmission);
            _fakeTollCancelRepository.Setup(s => s.AddAsync(It.IsAny<TollCancel>())).ReturnsAsync(new Exception());

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeTollEmissionRepository.Verify(s => s.GetByTollPaymentIdAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
