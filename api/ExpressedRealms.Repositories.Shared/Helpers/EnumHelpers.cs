namespace ExpressedRealms.Repositories.Shared.Helpers;

public static class EnumHelpers
{
    public static List<KeyValuePair<int, string>> GetEnumKeyValuePairs<T>()
        where T : Enum
    {
        var enumValues = Enum.GetValues(typeof(T));
        var enumNames = Enum.GetNames(typeof(T));
        var keyValuePairs = new List<KeyValuePair<int, string>>();

        for (int i = 0; i < enumValues.Length; i++)
        {
            keyValuePairs.Add(
                new KeyValuePair<int, string>((int)enumValues.GetValue(i), enumNames[i])
            );
        }

        return keyValuePairs;
    }
}
