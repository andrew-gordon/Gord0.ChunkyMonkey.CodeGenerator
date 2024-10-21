namespace Gord0.ChunkyMonkey.CodeGenerator.UnitTests.Helpers
{
    /// <summary>
    /// Helper class for comparing dictionaries.
    /// </summary>
    public static class DictionaryComparer
    {
        /// <summary>
        /// Compares two dictionaries for equality.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionaries.</typeparam>
        /// <param name="dict1">The first dictionary to compare.</param>
        /// <param name="dict2">The second dictionary to compare.</param>
        /// <returns><c>true</c> if the dictionaries are equal; otherwise, <c>false</c>.</returns>
        public static bool Compare<TKey, TValue>(IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2) where TKey : notnull
        {
            // Check if both dictionaries are null or the same reference
            if (ReferenceEquals(dict1, dict2)) return true;

            // Check if either is null
            if (dict1 == null || dict2 == null) return false;

            // Check if the dictionaries have the same number of elements
            if (dict1.Count != dict2.Count) return false;

            // Compare key-value pairs
            return dict1.All(kvp => dict2.TryGetValue(kvp.Key, out var value) && EqualityComparer<TValue>.Default.Equals(kvp.Value, value));
        }
    }
}
