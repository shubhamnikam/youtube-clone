namespace Domain.Rules;

public class DefinedEnumRule<TEnum> : IBusinessRule where TEnum : struct, Enum
{
    private readonly TEnum? enumValue;
    private readonly string propertyName;
    private readonly bool allowNull;

    public DefinedEnumRule(TEnum? enumValue, string propertyName, bool allowNull = false)
    {
        this.enumValue = enumValue;
        this.propertyName = propertyName;
        this.allowNull = allowNull;
    }

    public string BrokenReason => $"{propertyName} is invalid";

    public bool IsBroken()
    {
        if (!allowNull && !enumValue.HasValue)
        {
            return true;
        }

        return enumValue.HasValue && !Enum.IsDefined(enumValue.Value);
    }
}
