using DSharpPlus.Entities;
using DiscordBotBase.Core;
using DiscordBotBase.Utilities;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using DSharpPlus;

namespace DiscordBotBase.Extensions
{
    public static class StringExtensions
    {
        public static DiscordMember ToDiscordMember(this string stringMemberOrId)
        {
            DiscordClient discordClient = BotBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringMemberOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            stringMemberOrId = stringMemberOrId.ToLower();

            ulong.TryParse(string.Join(string.Empty, Regex.Split(stringMemberOrId, @"[^\d]")), out ulong memberId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var member = guild.Members.Values.FirstOrDefault(m => m.Nickname?.ToLower() == stringMemberOrId || m.Username.ToLower() == stringMemberOrId ||
                                                                      m.Id == memberId);
                if (member != null)
                    return member;
            }

            return null;
        }

        public static DiscordEmoji ToDiscordEmoji(this string stringEmojiOrId) => BaseUtilities.FindEmoji(stringEmojiOrId);

        public static DiscordRole ToDiscordRole(this string stringRoleOrId)
        {
            DiscordClient discordClient = BotBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringRoleOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            ulong.TryParse(string.Join(string.Empty, Regex.Split(stringRoleOrId, @"[^\d]")), out ulong resultRoleId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var role = guild.Roles.Values.FirstOrDefault(r => r.Name.ToLower() == stringRoleOrId.ToLower() || r.Id == resultRoleId);
                if (role != null)
                    return role;
            }

            return null;
        }

        public static DiscordChannel ToDiscordChannel(this string stringChannelOrId)
        {
            var discordClient = BotBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringChannelOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            ulong.TryParse(string.Join(string.Empty, Regex.Split(stringChannelOrId, @"[^\d]")), out ulong channelId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                foreach (DiscordChannel channel in guild.Channels.Values)
                {
                    string channelNewName = Regex.Replace(channel.Name, @"[^\w]", "").Replace("-", " ").Replace("_", " "),
                           channelRemovedAccents = BaseUtilities.RemoveAccents(channelNewName),
                           stringChannelToLower = stringChannelOrId.ToLower();
                    if (channelNewName.ToLower() == stringChannelToLower || channelRemovedAccents == stringChannelToLower || channel.Id == channelId)
                        return channel;
                }
            }

            return null;
        }

        public static bool StartWithNumber(this string stringValue) => Regex.IsMatch(stringValue, @"^\d"); 
    }
}