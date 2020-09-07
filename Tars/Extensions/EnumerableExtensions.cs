using System;
using System.Collections.Generic;
using System.Linq;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="IEnumerable{T}"/> methods and <see cref="T[]"/> methods.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the specified element is the first in the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="element">Element to be checked.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ElementIsFirst<T>(this T[] array, T element, int startIndex = 0)
        {
            array.IsNotNull(false);

            if ((element is string elementString && elementString.IsNullOrEmptyOrWhiteSpace()) || element is null)
                throw new ArgumentNullException($"The {nameof(element)} can be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the specified element is the first in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="element">Element to be checked.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ElementIsFirst<T>(this IEnumerable<T> enumerable, T element, int startIndex = 0)
        {
            enumerable.IsNotNull(false);

            if ((element is string elementString && elementString.IsNullOrEmptyOrWhiteSpace()) || element is null)
                throw new ArgumentNullException($"The {nameof(element)} can be null!");

            var arrayToList = enumerable.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) == 0;
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the specified element is the last in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="element">Element to be checked.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ElementIsLast<T>(this T[] array, T element, int startIndex = 0)
        {
            array.IsNotNull(false);

            if ((element is string elementString && elementString.IsNullOrEmptyOrWhiteSpace()) || element is null)
                throw new ArgumentNullException($"The {nameof(element)} can be null!");

            var arrayToList = array.ToList();
            return arrayToList.FindIndex(startIndex, e => e.Equals(element)) + 1 == arrayToList.Count;
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the specified element is the last in the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="element">Element to be checked.</param>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ElementIsLast<T>(this IEnumerable<T> enumerable, T element, int startIndex = 0)
        {
            enumerable.IsNotNull(false);

            if ((element is string elementString && elementString.IsNullOrEmptyOrWhiteSpace()) || element is null)
                throw new NullReferenceException($"The {nameof(element)} can be null!");

            var list = enumerable.ToList();
            return list.FindIndex(startIndex, e => e.Equals(element)) + 1 == list.Count;
        }
    }
}