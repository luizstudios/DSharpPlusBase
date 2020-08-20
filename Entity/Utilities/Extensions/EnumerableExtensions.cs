using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ElementIsLast<T>(this List<T> list, T element, int startIndex = 0) => list.Count == list.FindIndex(startIndex, e => e.Equals(element)) + 1;

        public static bool ElementIsLast<T>(this T[] array, T element, int startIndex = 0)
        {
            var arrayToList = array.ToList();
            return arrayToList.Count == arrayToList.FindIndex(startIndex, e => e.Equals(element)) + 1;
        }

        public static T ElementAtIndex<T>(this List<T> list, int index) => list[--index];

        public static T ElementAtIndex<T>(this T[] array, int index) => array[--index];
    }
}