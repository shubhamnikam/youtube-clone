namespace Domain.Rules;

public class LengthRule : IBusinessRule
{

    private readonly string? text;
    private readonly string propertyName;
    private readonly int? minLength;
    private readonly int? maxLength;

    public LengthRule(string? text, string propertyName, int? minLength, int? maxLength)
    {
        this.text = text;
        this.propertyName = propertyName;
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    public virtual string BrokenReason
    {
        get
        {
            if (minLength.HasValue && maxLength.HasValue)
            {
                if (minLength.Value == 1)
                {
                    return $"{propertyName} must be less than {maxLength.Value} characters";
                }
                else
                {
                    return $"{propertyName} must be between {minLength.Value} and {maxLength.Value} in length";
                }
            }

            if (minLength.HasValue)
            {
                if (minLength.Value == 1)
                {
                    return $"{propertyName} is required";
                }
                else
                {
                    return $"{propertyName} must be more than {minLength.Value} characters";
                }
            }

            if (maxLength.HasValue)
            {
                return $"{propertyName} must be less than {maxLength.Value} characters";
            }

            return string.Empty;
        }
    }

    public virtual bool IsBroken()
    {
        int length = string.IsNullOrEmpty(text) ? 0 : text.Length;

        if (minLength.HasValue && maxLength.HasValue)
        {
            return length < minLength.Value || length > maxLength.Value;
        }

        if (minLength.HasValue)
        {
            return length < minLength.Value;
        }

        if (maxLength.HasValue)
        {
            return length > maxLength.Value;
        }

        return false;
    }
}
