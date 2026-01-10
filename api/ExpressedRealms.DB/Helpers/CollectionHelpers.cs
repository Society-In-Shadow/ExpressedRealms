namespace ExpressedRealms.DB.Helpers;

public static class CollectionHelpers
{
    public static void Sync<TExisting, TKey>(
        ICollection<TExisting> existing,
        IEnumerable<TKey> incomingKeys,
        Func<TExisting, TKey> keySelector,
        Func<TKey, TExisting> factory
    )
        where TKey : notnull
    {
        var incomingSet = incomingKeys.ToHashSet();

        foreach (var item in existing
                     .Where(x => !incomingSet.Contains(keySelector(x)))
                     .ToList())
        {
            existing.Remove(item);
        }

        var existingSet = existing
            .Select(keySelector)
            .ToHashSet();

        foreach (var key in incomingSet.Except(existingSet))
        {
            existing.Add(factory(key));
        }
    }
}