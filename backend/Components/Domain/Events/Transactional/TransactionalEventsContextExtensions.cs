using EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Events.Transactional;

public static class TransactionalEventsContextExtensions
{
    private static Type? propertyType;

    public static void AddOutboxMessage(
            this ITransactionalEventsContext context,
            IntegrationEventBase @event,
            Action<IIntegrationEventProperties>? propertiesConfigurator = null)
    {
        var transactionalEvent = CreateEvent(context, @event, propertiesConfigurator);
        context.AddEvent(transactionalEvent);
    }

    public static void AddOutboxMessage(
          this ITransactionalEventsContext context,
          string? groupId,
          IntegrationEventBase @event,
          Action<IIntegrationEventProperties>? propertiesConfigurator = null)
    {

        var transactionalEvent = CreateEvent(context, @event, propertiesConfigurator);

        context.AddEvent(groupId, transactionalEvent);
    }


    private static TransactionalEvent CreateEvent(
           this ITransactionalEventsContext context,
           IntegrationEventBase eventBase,
           Action<IIntegrationEventProperties>? propertiesConfigurator = null)
    {
        if (propertyType is null)
        {
            var eventBus = context.ServiceProvider.GetRequiredService<IEventBus>();

            if (eventBus is null)
            {
                throw new ArgumentNullException("Event bus is not added");
            }

            Interlocked.CompareExchange(ref propertyType, eventBus.GetIntegrationEventPropertiesType(), null);
        }

        IIntegrationEventProperties properties = (Activator.CreateInstance(propertyType) as IIntegrationEventProperties)!;
        propertiesConfigurator?.Invoke(properties);

        var outboxMessage = new OutboxMessage(eventBase, properties);

        var transactionalEvent = new TransactionalEvent(OutboxMessage.Category, outboxMessage);
        return transactionalEvent;
    }
}
