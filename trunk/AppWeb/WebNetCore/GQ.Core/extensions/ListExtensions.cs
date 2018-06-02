using System.Collections.Generic;
using System.Linq;

namespace GQ.Core.extensions
{
    static class List
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : IPclCloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    public interface IPclCloneable
    {
        object Clone();
    }
}
