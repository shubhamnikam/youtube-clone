using System.Text.RegularExpressions;

namespace Domain.Rules;

public class RegexMatchingRule : IBusinessRule
{

    private readonly string? text;
    private readonly string propertyName;
    private readonly string pattern;
    private readonly bool allowEmpty;

    public RegexMatchingRule(string? text, string propertyName, string pattern, bool allowEmpty = true)
    {
        this.text = text;
        this.propertyName = propertyName;
        this.pattern = pattern;
        this.allowEmpty = allowEmpty;
    }

    public virtual string BrokenReason
    {
        get
        {
            return $"{propertyName} is invalid";
        }
    }

    public virtual bool IsBroken()
    {
        if (allowEmpty && string.IsNullOrEmpty(text))
        {
            return false;
        }

        string textCalc = text == null ? string.Empty : text;
        return !Regex.IsMatch(textCalc, pattern);
    }
}
