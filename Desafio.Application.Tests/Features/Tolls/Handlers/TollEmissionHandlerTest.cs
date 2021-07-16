using FluentAssertions;
using MediatR;
using Moq;
using ndd.SharedKernel.Result;
using NddCargo.Integration.Application.Features.Tolls.Commands;
using NddCargo.Integration.Application.Features.Tolls.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Tolls.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.Core.Exceptions;
using NddCargo.Integration.Domain.Features.Tolls;
using NddCargo.Integration.Domain.ObjectMother.Features.Tolls;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.Handlers
{
    [TestFixture]
    [Category("Application.Tolls.Handlers")]
    public class TollEmissionHandlerTest : TestBase
    {
        private TollEmissionHandler _handler;
        private TollEmissionCommand _command;

        private readonly Mock<ITollEmissionRepository> _fakeTollEmissionRepository;
        private readonly Mock<ITollEmissionConfigurationRepository> _fakeTollEmissionConfigurationRepository;
        private readonly Mock<IMediator> _fakeMediator;

        public TollEmissionHandlerTest()
        {
            _fakeTollEmissionRepository = new Mock<ITollEmissionRepository>();
            _fakeTollEmissionConfigurationRepository = new Mock<ITollEmissionConfigurationRepository>();
            _fakeMediator = new Mock<IMediator>();
        }

        [SetUp()]
        public void Initialize()
        {
            _fakeTollEmissionRepository.Reset();
            _handler = new TollEmissionHandler(_fakeMediator.Object, _fakeTollEmissionRepository.Object, _fakeTollEmissionConfigurationRepository.Object);
        }

        [Test]
        public async Task Test_TollEmissionHandler_DeveriaRealizarAEmissaoDeValePedagio()
        {
            // Arrange
            _command = TollEmissionCommandObjectMother.TollEmissionCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            var tollEmissionConfiguration = TollEmissionConfigurationObjectMother.TollEmissionConfiguration;

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync())
                .ReturnsAsync(tollEmissionConfiguration);

            _fakeTollEmissionRepository
                .Setup(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()))
                .ReturnsAsync(tollEmission);

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.UpdateAsync())
                .ReturnsAsync(ndd.SharedKernel.Result.Unit.Successful);

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<TollEmission>();
            
            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync(), Times.Once);
            _fakeTollEmissionRepository.Verify(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()), Times.Once);
            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.UpdateAsync(), Times.Once);
        }

        [Test]
        public async Task Test_TollEmissionHandler_DeveriaRetornarFalha_CasoTollEmissionConfigurationNaoSejaEncontrado()
        {
            // Arrange
            _command = TollEmissionCommandObjectMother.TollEmissionCommand;

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync())
                .ReturnsAsync(new Exception());

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync(), Times.Exactly(4));
            _fakeTollEmissionRepository.Verify(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()), Times.Never);
        }

        [Test]
        public async Task Test_TollEmissionHandler_DeveriaRetornarExcecao_AoRealizarAEmissaoDeValePedagio()
        {
            // Arrange
            _command = TollEmissionCommandObjectMother.TollEmissionCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            var tollEmissionConfiguration = TollEmissionConfigurationObjectMother.TollEmissionConfiguration;

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync())
                .ReturnsAsync(tollEmissionConfiguration);

            _fakeTollEmissionRepository
                .Setup(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()))
                .ReturnsAsync(ExceptionReturnBase.CreateException(ExceptionReturnBase.Error999, new Exception().Message));

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<BusinessException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync(), Times.Exactly(3));
            _fakeTollEmissionRepository.Verify(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()), Times.Once);
        }

        [Test]
        public async Task Test_TollEmissionHandler_DeveriaRetornarExcecao_AoAtualizarAConfiguracaoDeEmissaoDeValePedagio()
        {
            // Arrange
            _command = TollEmissionCommandObjectMother.TollEmissionCommand;
            var tollEmission = TollEmissionObjectMother.TollEmission;
            var tollEmissionConfiguration = TollEmissionConfigurationObjectMother.TollEmissionConfiguration;

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync())
                .ReturnsAsync(tollEmissionConfiguration);

            _fakeTollEmissionRepository
                .Setup(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()))
                .ReturnsAsync(tollEmission);

            _fakeTollEmissionConfigurationRepository
                .Setup(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.UpdateAsync())
                .ReturnsAsync(ExceptionReturnBase.CreateException(ExceptionReturnBase.Error999, new Exception().Message));

            // Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            // Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<BusinessException>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();

            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.GetAsync(), Times.Exactly(2));
            _fakeTollEmissionRepository.Verify(tollEmissionRepository => tollEmissionRepository.AddAsync(It.IsAny<TollEmission>()), Times.Once);
            _fakeTollEmissionConfigurationRepository.Verify(tollEmissionConfigurationRepository => tollEmissionConfigurationRepository.UpdateAsync(), Times.Exactly(2));
        }
    }
}
