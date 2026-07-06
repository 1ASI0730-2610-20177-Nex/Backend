using Electro.Corporation.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace Electro.Corporation.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}