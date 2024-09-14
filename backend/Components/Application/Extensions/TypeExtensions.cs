namespace Application.Extensions;

public static class TypeExtensions
{
    public static string GetGenericTypeName(this Type type)
    {
        string typeName = string.Empty;

        if (type.IsGenericType)
        {
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(x => x.Name));
            typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
        }
        else
        {
            typeName = type.Name;
        }
        return typeName;
    }
}
