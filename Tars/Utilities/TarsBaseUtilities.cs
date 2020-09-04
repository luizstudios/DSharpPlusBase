using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Globalization;
using System.Linq;
using Tars.Core;
using Tars.Extensions;

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
            if (emojiNameOrId.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException("The emoji name or id can't be null!");

            DiscordClient discordClient = TarsBase._discordClient;

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
    }
}