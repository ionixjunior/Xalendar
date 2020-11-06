using System.Collections.Generic;
using System.Linq;

namespace Xalendar.View.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection is null)
                return true;

            return !collection.Any();
        }
    }
}
