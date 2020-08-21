using DSharpPlus;
using DSharpPlus.Entities;
using Entity.Base.Core;
using Entity.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static DiscordMember ToDiscordMember(this string stringMemberOrId)
        {
            var discordClient = EntityBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringMemberOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            stringMemberOrId = stringMemberOrId.ToLower();

            ulong memberId = 0;
            if (ulong.TryParse(string.Join(string.Empty, Regex.Split(stringMemberOrId, @"[^\d]")), out ulong resultMemberId))
                memberId = resultMemberId;

            foreach (var guild in discordClient.Guilds.Values)
            {
                var member = guild.Members.Values.FirstOrDefault(m => m.Nickname?.ToLower() == stringMemberOrId || m.Username.ToLower() == stringMemberOrId || 
                                                                      m.Id == memberId);
                if (member != null)
                    return member;
            }

            return null;
        }

        public static DiscordEmoji ToDiscordEmoji(this string stringEmojiOrId) => EntityBaseUtilities.FindEmoji(stringEmojiOrId);

        public static DiscordRole ToDiscordRole(this string stringRoleOrId)
        {
            var discordClient = EntityBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringRoleOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            ulong roleId = 0;
            if (ulong.TryParse(string.Join(string.Empty, Regex.Split(stringRoleOrId, @"[^\d]")), out ulong resultRoleId))
                roleId = resultRoleId;

            foreach (var guild in discordClient.Guilds.Values)
            {
                var role = guild.Roles.Values.FirstOrDefault(r => r.Name.ToLower() == stringRoleOrId.ToLower() || r.Id == roleId);
                if (role != null)
                    return role;
            }

            return null;
        }

        public static DiscordChannel ToDiscordChannel(this string stringChannelOrId)
        {
            var discordClient = EntityBase._discordClient;
            if (discordClient == null)
                throw new NullReferenceException("The DiscordClient can't be null!");

            if (string.IsNullOrWhiteSpace(stringChannelOrId))
                throw new ArgumentNullException("The member mention or Id can't be null!");

            ulong channelId = 0;
            if (ulong.TryParse(string.Join(string.Empty, Regex.Split(stringChannelOrId, @"[^\d]")), out ulong resultChannelId))
                channelId = resultChannelId;

            foreach (var guild in discordClient.Guilds.Values)
            {
                foreach (var channel in guild.Channels.Values)
                {
                    string channelNewName = Regex.Replace(channel.Name, @"[^\w]", "").Replace("-", " ").Replace("_", " "),
                           channelRemovedAccents = EntityBaseUtilities.RemoveAccents(channelNewName),
                           stringChannelToLower = stringChannelOrId.ToLower();
                    if (channelNewName.ToLower() == stringChannelToLower || channelRemovedAccents == stringChannelToLower || channel.Id == channelId)
                        return channel;
                }
            }

            return null;
        }
    }
}