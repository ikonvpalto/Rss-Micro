using System.Collections.Generic;

namespace Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinString(this IEnumerable<string> strings, string separator = ", ")
            => string.Join(separator, strings);
    }
}
