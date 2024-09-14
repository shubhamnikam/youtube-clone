using System.Text.Json;
using SharedKernel.Utilities;

namespace Domain.Events.Transactional;

public class TransactionalEvent
{
    public string Category { get; private set; }
    public string Type { get; private set; }
    public string Data { get; private set; }

    public TransactionalEvent(string category, string type, string data)
    {
        Category = category;
        Type = type;
        Data = data;
    }

    public TransactionalEvent(string category, object eventObject, Type? type = null)
    {
        Category = category;
        Type = type?.FullName! ?? eventObject.GetType().FullName!;
        Data = JsonSerializer.Serialize(eventObject);
    }
    public object? GetEvent()
    {
        if (string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(Data))
        {
            return null;
        }

        var type = TypeCache.GetType(Type);

        if (type == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize(Data, type, new JsonSerializerOptions() { IncludeFields = true });
    }
}
