namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers
{
    /// <summary>
    /// Helper class for comparing SortedLists.
    /// </summary>
    public static class SortedListComparer
    {
        /// <summary>
        /// Compares two SortedLists for equality by comparing their keys and values.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the SortedLists.</typeparam>
        /// <typeparam name="TValue">The type of the values in the SortedLists.</typeparam>
        /// <param name="list1">The first SortedList to compare.</param>
        /// <param name="list2">The second SortedList to compare.</param>
        /// <returns><c>true</c> if the SortedLists are equal; otherwise, <c>false</c>.</returns>
        public static bool Compare<TKey, TValue>(
            this SortedList<TKey, TValue> list1,
            SortedList<TKey, TValue> list2) where TKey : notnull
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!EqualityComparer<TKey>.Default.Equals(list1.Keys[i], list2.Keys[i]) ||
                    !EqualityComparer<TValue>.Default.Equals(list1.Values[i], list2.Values[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
