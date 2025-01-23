using System;
using System.Collections.Generic;

public static class ListExtension
{
    private static Random _random = new Random();

    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to pick a random element from.</param>
    /// <returns>A random element from the list.</returns>
    public static T GetRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("The list is null or empty.");
        }

        int index = _random.Next(list.Count);
        return list[index];
    }
}
