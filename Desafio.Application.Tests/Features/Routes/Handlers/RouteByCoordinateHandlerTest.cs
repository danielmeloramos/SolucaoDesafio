using FluentAssertions;
using Moq;
using NddCargo.Integration.Application.Features.Routes.Commands;
using NddCargo.Integration.Application.Features.Routes.Handlers;
using NddCargo.Integration.Application.ObjectMother.Features.Routes.Commands;
using NddCargo.Integration.Application.Tests.Initializer;
using NddCargo.Integration.DataCenterIntegration.Features.Routes;
using NddCargo.Integration.DataCenterIntegration.Features.Routes.Queries;
using NddCargo.Integration.DataCenterIntegration.ObjectMother.Features.Routes.Dto;
using NddCargo.Integration.Domain.Features.Routes;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Routes.Handlers
{
    [TestFixture]
    [Category("Application.Routes.Handlers")]
    public class RouteByCoordinateHandlerTest : TestBase
    {
        private RouteByCoordinateHandler _handler;
        private RouteByCoordinateCommand _command;

        private readonly Mock<IRouteService> _fakeRouteService;

        public RouteByCoordinateHandlerTest() => _fakeRouteService = new Mock<IRouteService>();

        [SetUp]
        public void Initialize()
        {
            _fakeRouteService.Reset();
            _command = RouteByCoordinateCommandObjectMother.RouteByCoordinateCommand;
            _handler = new RouteByCoordinateHandler(_fakeRouteService.Object);
        }

        [Test]
        public async Task Test_RouteByCoordinateHandler_DeveriaRealizarBuscarARotaPorCoordenadasQuandoForValido()
        {
            //Arrange
            var routeDto = RouteDtoObjectMother.RouteDto;
            _fakeRouteService.Setup(s => s.GetRouteData(It.IsAny<RouteQuery>())).ReturnsAsync(routeDto);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeFalse();
            response.Failure.Should().BeNull();
            response.IsSuccess.Should().BeTrue();
            response.Success.Should().BeOfType<RouteInfo>();
            _fakeRouteService.Verify(s => s.GetRouteData(It.IsAny<RouteQuery>()), Times.Once);
        }
        
        [Test]
        public async Task Test_RouteByCoordinateHandler_DeveriaNaoRealizarAutenticacaoDoUsuarioQuandoNaoEncontrarOUsuario()
        {
            //Arrange
            var exception = new Exception();
            _fakeRouteService.Setup(s => s.GetRouteData(It.IsAny<RouteQuery>())).ReturnsAsync(exception);

            //Action
            var response = await _handler.Handle(_command, It.IsAny<CancellationToken>());

            //Assert
            response.IsFailure.Should().BeTrue();
            response.Failure.Should().BeOfType<Exception>();
            response.IsSuccess.Should().BeFalse();
            response.Success.Should().BeNull();
            _fakeRouteService.Verify(s => s.GetRouteData(It.IsAny<RouteQuery>()), Times.Once);
        } 
    }
}
