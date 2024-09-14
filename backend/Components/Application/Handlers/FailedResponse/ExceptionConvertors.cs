namespace Application.Handlers.FailedResponse;

public interface IExceptionConvertorsBuilder
{
    void AddConvertor(IExceptionConvertor identifier);
    void Clear();
}

public class ExceptionConvertorsBuilder : IExceptionConvertorsBuilder
{

    private List<IExceptionConvertor> _convertors;

    public ExceptionConvertorsBuilder()
    {
        _convertors = new List<IExceptionConvertor>();
    }

    public void Clear()
    {
        _convertors.Clear();
    }

    public void AddConvertor(IExceptionConvertor identifier)
    {
        _convertors.Add(identifier);
    }

    public List<IExceptionConvertor> Build()
    {
        return _convertors;
    }

}

public static class ExceptionConvertors
{
    private static List<IExceptionConvertor> _convertors;
    static ExceptionConvertors()
    {

    }

    public static void Register(Action<IExceptionConvertorsBuilder>? configure)
    {
        var builder = new ExceptionConvertorsBuilder();
        configure?.Invoke(builder);
        AddDefaultConvertors(builder);
        _convertors = builder.Build();
    }
    private static void AddDefaultConvertors(IExceptionConvertorsBuilder builder)
    {
        builder.AddConvertor(new BusinessRuleValidationExceptionConvertor());
        builder.AddConvertor(new ValidationExceptionConvertor());
        builder.AddConvertor(new AppExceptionConvertor());
        builder.AddConvertor(new HttpRequestExceptionConvertor());
    }
}
