using System.Collections.Specialized;

namespace Gord0.ChunkyMonkey.CodeGenerator.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<string> ToEnumerable(this StringCollection stringCollection)
        {
            foreach (var item in stringCollection)
            {
                yield return item;
            }
        }
    }
}
