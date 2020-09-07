using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tars.Core;
using Tars.Exceptions;
using Tars.Extensions;
using Tars.Tars.Exceptions;

namespace Tars.Utilities
{
    /// <summary>
    /// Tars utility class.
    /// </summary>
    internal static class TarsBaseUtilities
    {
        /// <summary>
        /// Internal method to search for emojis.
        /// </summary>
        /// <param name="emojiNameOrId"></param>
        /// <returns>A <see cref="DiscordEmoji"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        internal static DiscordEmoji FindEmoji(string emojiNameOrId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            emojiNameOrId.IsNotNull();

            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;

            ulong.TryParse(emojiNameOrId, out ulong emojiId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var emoji = guild.Emojis.Values.FirstOrDefault(e => string.Equals(e.Name, emojiNameOrId.Replace(":", ""), stringComparison) ||
                                                                    string.Equals(e.ToString(), emojiNameOrId, stringComparison) || e.Id == emojiId);
                if (emoji != null)
                    return emoji;
            }

            try
            {
                return DiscordEmoji.FromName(discordClient, $"{(emojiNameOrId.StartsWith(":") && emojiNameOrId.EndsWith(":") ? emojiNameOrId : $":{emojiNameOrId}:")}");
            }
            catch { }

            return !emojiNameOrId.IsNullOrEmptyOrWhiteSpace() && CharUnicodeInfo.GetUnicodeCategory(emojiNameOrId, 0) == UnicodeCategory.OtherSymbol ?
                   DiscordEmoji.FromUnicode(discordClient, emojiNameOrId) : null;
        }

        /// <summary>
        /// Check if the <see cref="T"/> isn't <see langword="null"/>. If it is not <see langword="null"/>, the type is returned.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="messageError">Message of error personalized.</param>
        /// <exception cref="NullReferenceException"></exception>
        internal static T IsNotNull<T>(this T type, string messageError = null)
            => type is string typeString && typeString.IsNullOrEmptyOrWhiteSpace() ? throw new NullReferenceException($"The {nameof(type)} can be null!") :
               type is null ? throw new NullReferenceException(messageError.IsNullOrEmptyOrWhiteSpace() ? $"The {nameof(T)} can be null!" : messageError) : type;

        /// <summary>
        /// Checks whether a list is <see langword="null"/> or has <see langword="null"/> elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ElementIsNullException"></exception>
        internal static void IsNotNull<T>(this IEnumerable<T> list, bool checkElements = true)
        {
            if (list is null)
                throw new NullReferenceException($"The list of {nameof(T)} can be null!");

            if (checkElements && list.Any(e => e is null))
                throw new ElementIsNullException($"No element within the {nameof(T)} list can be null!");
        }

        /// <summary>
        /// Checks whether an array is <see langword="null"/> or has <see langword="null"/> elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ElementIsNullException"></exception>
        internal static void IsNotNull<T>(this T[] array, bool checkElements = true)
        {
            if (array is null)
                throw new NullReferenceException($"The array of {nameof(T)} can be null!");

            if (checkElements && array.Any(e => e is null))
                throw new ElementIsNullException($"No element within the {nameof(T)} array can be null!");
        }

        /// <summary>
        /// Check if a <see langword="ulong"/> is 0.
        /// </summary>
        /// <param name="ulongNumber"><see langword="ulong"/> to check.</param>
        internal static void Is0(this ulong ulongNumber)
        {
            if (ulongNumber == 0)
                throw new Is0Exception($"The {nameof(UInt64)} can be 0!");
        }

        /// <summary>
        /// Check if a <see langword="long"/> is 0.
        /// </summary>
        /// <param name="longNumber"><see langword="long"/> to check.</param>
        internal static void Is0(this long longNumber)
        {
            if (longNumber == 0)
                throw new Is0Exception($"The {nameof(Int64)} can be 0!");
        }
    }
}