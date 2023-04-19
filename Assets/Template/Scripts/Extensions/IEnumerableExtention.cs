using System;
using System.Collections.Generic;

namespace Template.Extension
{
    /// <summary>
    /// スクリプト
    /// </summary>
    public static class IEnumerableExtention
    {
        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action(item);
        }
    }
}