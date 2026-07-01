namespace ExpressedRealms.Scaffolder.Generator.Utilities;

public static class StringUtilities
{
    public static string InitialCase(string str)
    {
        return char.ToLower(str[0]) + str[1..];
    }
}
