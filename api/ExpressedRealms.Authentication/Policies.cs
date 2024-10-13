namespace ExpressedRealms.Authentication;

public class Policies
{
    public string Name { get; }

    // Private constructor to prevent external instantiation
    private Policies(string name)
    {
        Name = name;
    }

    // Predefined static instances for each policy
    public static readonly Policies ExpressionEditorPolicy = new(nameof(ExpressionEditorPolicy));

    // Override ToString for convenience
    public override string ToString()
    {
        return Name;
    }
}
