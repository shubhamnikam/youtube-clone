namespace Domain.Rules;

public class CharactersRule : IBusinessRule
{
    private readonly string? text;
    private readonly string propertyName;
    private readonly char[] invalidCharacters;
    public CharactersRule(string? text, string propertyName, char[] invalidCharacters)
    {
        this.text = text;
        this.propertyName = propertyName;
        this.invalidCharacters = invalidCharacters;
    }

    public virtual string BrokenReason
    {
        get
        {
            return $"{propertyName} contains invalid character";
        }
    }

    public bool IsBroken()
    {
        if (!string.IsNullOrEmpty(text))
        {
            if (invalidCharacters.Any(x => text.Contains(x)))
            {
                return true;
            }
        }
        return false;
    }
}
