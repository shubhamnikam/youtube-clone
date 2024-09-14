using Domain.Events.Domain;
using MediatR;

namespace Application.Handlers;

public class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly IMediator _mediator;
    private readonly IDomainEventsAccessor? _domainEventsAccessor;

    public DomainEventsDispatcher(IMediator mediator, IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _domainEventsAccessor = serviceProvider.GetService(typeof(IDomainEventsAccessor)) as IDomainEventsAccessor;
    }

    public async Task DispatchDomainEventsAsync()
    {
        if (_domainEventsAccessor is null)
        {
            return;
        }

        var domainEvents = _domainEventsAccessor.GetDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
