using Moq;
using ndd.SharedKernel.DomainEvent.MediatR;
using NddCargo.Integration.Application.Features.Tolls.NotificationHandlers;
using NddCargo.Integration.Domain.Features.Tolls;
using NddCargo.Integration.Domain.Features.Tolls.Events;
using NddCargo.Integration.Domain.ObjectMother.Features.Tolls;
using NddCargo.Integration.Domain.ObjectMother.Features.Tolls.Events;
using NddCargo.IntegrationEvents;
using NServiceBus;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace NddCargo.Integration.Application.Tests.Features.Tolls.NotificationHandlers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.Tolls.NotificationHandlers")]
    public class TollCancelNotificationHandlerTest
    {
        private TollCancelNotificationHandler _notificationHandler;
        private readonly Mock<IMessageSession> _fakeMessageSession;
        private DomainEventAdapter<TollCancelDomainEvent> _notification;

        public TollCancelNotificationHandlerTest() => _fakeMessageSession = new Mock<IMessageSession>();

        [SetUp]
        public void Initialize()
        {
            _fakeMessageSession.Reset();

            _notificationHandler = new TollCancelNotificationHandler(_fakeMessageSession.Object);
        }

        /*[Test]
        public async Task Test_TollCancelNotificationHandler_DeveriaRealizarOCancelamentoDoValePedagio()
        {
            //Arrange
            _fakeMessageSession.Setup(s => s.Publish(It.IsAny<TollCancelIntegrationEvent>())).Verifiable();
            _notification = new DomainEventAdapter<TollCancelDomainEvent>(TollCancelDomainEventObjectMother.TollCancelDomainEvent);

            //Action
            await _notificationHandler.Handle(_notification, It.IsAny<CancellationToken>());

            //Assert
            _fakeMessageSession.Setup(s => s.Publish(It.IsAny<TollCancelIntegrationEvent>())).Verifiable();
        }*/

    }
}
