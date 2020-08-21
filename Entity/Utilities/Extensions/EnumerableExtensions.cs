using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ElementIsFirst<T>(this T[] array, T element, int startIndex = 0)
        {
            if (array == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }

        public static bool ElementIsFirst<T>(this IEnumerable<T> enumerable, T element, int startIndex = 0)
        {
            if (enumerable == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = enumerable.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }


        public static bool ElementIsLast<T>(this T[] array, T element, int startIndex = 0)
        {
            if (array == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) + 1 == arrayToList.Count;
        }

        public static bool ElementIsLast<T>(this IEnumerable<T> enumerable, T element, int startIndex = 0)
        {
            var list = enumerable.ToList();
            return list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ?
                                  throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) + 1 == list.Count;
        }


        public static string ToReadableString(this char[] charArray)
        {
            var stringBuilder = new StringBuilder();
            foreach (char c in charArray)
                stringBuilder.Append(c);

            return stringBuilder.ToString();
        }

        public static string ToReadableString(this IEnumerable<char> charEnumerable)
        {
            var stringBuilder = new StringBuilder();
            foreach (char c in charEnumerable)
                stringBuilder.Append(c);

            return stringBuilder.ToString();
        }
    }
}