using System.Diagnostics.CodeAnalysis;

namespace ue.Extensions;

public static class CommonExtensions
{
    public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TKey, TValue> compute)
    {
        if (dictionary.TryGetValue(key, out var existing))
            return existing;

        var result = compute(key);
        dictionary[key] = result;
        return result;
    }
}