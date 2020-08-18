using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlusBase.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DSharpPlusBase.Utilities
{
    public static class BotBaseUtilities
    {
        public static DiscordColor RandomColorEmbed()
        {
            var rgbColor = new Random();
            return new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255));
        }

        public static DiscordEmoji FindEmoji(DiscordClient discordClient, string emojiName)
        {
            if (discordClient == null)
                throw new ArgumentNullException("The DiscordClient can't be null!");
            else if (string.IsNullOrWhiteSpace(emojiName))
                throw new ArgumentNullException("The emoji can't be null!");

            string oldNameEmoji = emojiName;
            emojiName = emojiName.ToLower();
            ulong.TryParse(emojiName, out ulong emojiId);

            foreach (var guild in discordClient.Guilds.Values)
            {
                var emojiFind = guild.Emojis.Values.FirstOrDefault(e => e.Name.ToLower() == emojiName.Replace(":", "") || e.Id == emojiId || 
                                                                        e.ToString().ToLower() == emojiName);
                if (emojiFind != null)
                    return emojiFind;
            }

            throw new EmojiNotFoundException(oldNameEmoji);
        }

        public static ulong MentionToMemberId(string memberMention) 
            => !string.IsNullOrWhiteSpace(memberMention) ? ulong.Parse(string.Join(string.Empty, Regex.Split(memberMention, @"[^\d]"))) :
                                                           throw new ArgumentNullException("The member mention can't be null!");

        public static DiscordRole FindRole(DiscordClient discordClient, string roleName)
        {
            if (discordClient == null)
                throw new ArgumentNullException("The DiscordClient can't be null!");
            else if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("The emoji can't be null!");

            string oldNameRole = roleName;
            roleName = roleName.ToLower();
            ulong.TryParse(roleName, out ulong roleId);

            foreach (var guild in discordClient.Guilds.Values)
            {
                var role = guild.Roles.Values.FirstOrDefault(r => r.Name.ToLower() == roleName || r.Mention.ToLower() == roleName || r.Id == roleId);
                if (role != null)
                    return role;
            }

            throw new RoleNotFoundException(oldNameRole);
        }
    }
}