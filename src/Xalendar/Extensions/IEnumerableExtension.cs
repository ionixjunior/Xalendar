using System.Collections.Generic;
using System.Linq;

namespace Xalendar.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
        {
            if (collection is { })
                return !collection.Any();

            return true;
        }
    }
}
