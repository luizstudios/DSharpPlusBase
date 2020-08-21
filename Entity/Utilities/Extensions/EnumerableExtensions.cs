using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ElementIsFirst<T>(this List<T> list, T element, int startIndex = 0) 
            => list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ?
                              throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) == 0;

        public static bool ElementIsFirst<T>(this T[] array, T element, int startIndex = 0)
        {
            if (array == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }

        public static bool ElementIsFirst<T>(this IReadOnlyCollection<T> collection, T element, int startIndex = 0)
        {
            var list = collection.ToList();
            return list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ?
                                        throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }

        public static bool ElementIsFirst<T>(this IReadOnlyList<T> readOnlyList, T element, int startIndex = 0)
        {
            if (readOnlyList == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = readOnlyList.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }


        public static bool ElementIsLast<T>(this List<T> list, T element, int startIndex = 0) 
            => list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ? 
                              throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) + 1 == list.Count;
        
        public static bool ElementIsLast<T>(this T[] array, T element, int startIndex = 0)
        {
            if (array == null)
                throw new ArgumentNullException("The array can't be null!");

            if (element == null)
                throw new ArgumentNullException("The element can't be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) + 1 == arrayToList.Count;
        }

        public static bool ElementIsLast<T>(this IReadOnlyCollection<T> collection, T element, int startIndex = 0)
        {
            var list = collection.ToList();
            return list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ?
                                  throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) + 1 == list.Count;
        }

        public static bool ElementIsLast<T>(this IReadOnlyList<T> readOnlyList, T element, int startIndex = 0)
        {
            var list = readOnlyList.ToList();
            return list == null ? throw new ArgumentNullException("The list can't be null!") : element == null ?
                                  throw new ArgumentNullException("The element can't be null!") : list.FindIndex(startIndex, e => e.Equals(element)) + 1 == list.Count;
        }
    }
}