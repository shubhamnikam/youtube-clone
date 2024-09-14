
using Domain.Events.Transactional;

namespace Infrastructure.TransactionalEvents.Processing
{
    public interface ITransactionalEventsHandler
    {
        Task ProcessTransactionalEventsAsync(List<TransactionalEvent> events, CancellationToken cancellationToken);
    }
}
