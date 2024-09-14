using Domain.Events.Domain;
using Domain.Exceptions;
using Domain.Rules;

namespace Domain.Core;

public class DomainEntity : Entity, IDomainEventEmitter
{
    private List<IDomainEvent> domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public DomainEntity()
    {
        DomainEvents = domainEvents.AsReadOnly();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        lock (domainEvents)
        {
            domainEvents.Add(domainEvent);
        }
    }

    public void RemoveAllDomainEvents()
    {
        lock (domainEvents)
        {
            domainEvents.Clear();
        }
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        lock (domainEvents)
        {
            domainEvents.Remove(domainEvent);
        }
    }

    public void AddUniqueDomainEvent(IDomainEvent domainEvent)
    {
        lock (domainEvents)
        {
            domainEvents.RemoveAll(x =>
            {
                return x.GetType() == domainEvent.GetType();
            });

            domainEvents.Add(domainEvent);
        }
    }

    public void CheckRule<TRule>(TRule rule) where TRule : IBusinessRule
    {
        if (rule.IsBroken())
        {
            throw BusinessRuleValidationException.Create(rule);
        }
    }
    public void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw BusinessRuleValidationException.Create(rule);
        }
    }

    public void CheckRules(params IBusinessRule[] rules)
    {
        foreach (var rule in rules)
        {
            CheckRule(rule);
        }
    }
}
