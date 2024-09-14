namespace Domain.Events.Domain;

public interface IDomainEventsDispatcher
{
    Task DispatchDomainEventsAsync();
}
