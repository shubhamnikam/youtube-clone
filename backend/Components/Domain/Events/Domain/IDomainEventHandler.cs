using MediatR;

namespace Domain.Events.Domain;

public interface IDomainEventHandler<in TDomainEvent>
    : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
}

