using DSharpPlus.Entities;
using DiscordBotBase.Core;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using DSharpPlus;

namespace DiscordBotBase.Utilities
{
    public static class BaseUtilities
    {
        internal static DiscordEmoji FindEmoji(string emojiNameOrId)
        {
            if (string.IsNullOrWhiteSpace(emojiNameOrId))
                throw new ArgumentNullException("The emoji name or Id can't be null!");

            DiscordClient discordClient = BotBase._discordClient;

            string oldNameEmoji = emojiNameOrId;
            emojiNameOrId = emojiNameOrId.ToLower();
            ulong.TryParse(emojiNameOrId, out ulong emojiId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var emoji = guild.Emojis.Values.FirstOrDefault(e => e.Name.ToLower() == emojiNameOrId.Replace(":", "") || e.Id == emojiId ||
                                                                    e.ToString().ToLower() == emojiNameOrId);
                if (emoji != null)
                    return emoji;
            }

            try
            {
                return DiscordEmoji.FromName(discordClient, 
                                             $"{(emojiNameOrId.StartsWith(":") && emojiNameOrId.EndsWith(":") ? emojiNameOrId : $":{emojiNameOrId}:")}");
            }
            catch { }

            return !string.IsNullOrWhiteSpace(emojiNameOrId) && CharUnicodeInfo.GetUnicodeCategory(emojiNameOrId, 0) == UnicodeCategory.OtherSymbol ?
                   DiscordEmoji.FromUnicode(discordClient, emojiNameOrId) : null;
        }

        internal static string RemoveAccents(string text)
        {
            var stringBuilder = new StringBuilder();
            foreach (char c in text.Normalize(NormalizationForm.FormD).ToCharArray().Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                stringBuilder.Append(c);

            return stringBuilder.ToString();
        }
    }
}