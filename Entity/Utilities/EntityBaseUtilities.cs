using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Entity.Base.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Entity.Base.Utilities
{
    public static class EntityBaseUtilities
    {
        public static DiscordColor RandomColor()
        {
            var rgbColor = new Random();
            return new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255));
        }

        internal static DiscordEmoji FindEmoji(string emojiNameOrId)
        {
            if (string.IsNullOrWhiteSpace(emojiNameOrId))
                throw new ArgumentNullException("The emoji name or Id can't be null!");

            var discordClient = EntityBase._discordClient;

            string oldNameEmoji = emojiNameOrId;
            emojiNameOrId = emojiNameOrId.ToLower();
            ulong.TryParse(emojiNameOrId, out ulong emojiId);

            foreach (var guild in discordClient.Guilds.Values)
            {
                var emoji = guild.Emojis.Values.FirstOrDefault(e => e.Name.ToLower() == emojiNameOrId.Replace(":", "") || e.Id == emojiId ||
                                                                    e.ToString().ToLower() == emojiNameOrId);
                if (emoji != null)
                    return emoji;
            }

            try
            {
                return DiscordEmoji.FromName(discordClient, $"{(emojiNameOrId.StartsWith(":") && emojiNameOrId.EndsWith(":") ? emojiNameOrId : $":{emojiNameOrId}:")}");
            }
            catch { }

            //UnicodeCategory? unicodeCategory = null;
            //if (char.TryParse(stringEmojiOrId, out char resultChar))
            //    unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(resultChar);
            
            return !string.IsNullOrWhiteSpace(emojiNameOrId) && CharUnicodeInfo.GetUnicodeCategory(emojiNameOrId, 0) == UnicodeCategory.OtherSymbol ?
                   DiscordEmoji.FromUnicode(discordClient, emojiNameOrId) : null;
        }

        internal static string RemoveAccents(string text)
        {
            var sbReturn = new StringBuilder();

            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }
    }
}