namespace Domain.Events.Domain;

public interface IDomainEventsAccessor
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();

    void ClearDomainEvents();
}