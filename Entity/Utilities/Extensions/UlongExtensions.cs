using DSharpPlus;
using DSharpPlus.Entities;
using Entity.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class UlongExtensions
    {
        public static DiscordMember ToDiscordMember(this ulong memberId)
        {
            if (memberId == 0)
                throw new ArgumentException("The member id can't be 0!");

            foreach (var guild in EntityBase._discordClient.Guilds.Values)
            {
                var member = guild.Members.Values.FirstOrDefault(m => m.Id == memberId);
                if (member != null)
                    return member;
            }

            return null;
        }

        public static DiscordRole ToDiscordRole(this ulong roleId)
        {
            if (roleId == 0)
                throw new ArgumentException("The role id can't be 0!");

            foreach (var guild in EntityBase._discordClient.Guilds.Values)
            {
                var member = guild.Roles.Values.FirstOrDefault(r => r.Id == roleId);
                if (member != null)
                    return member;
            }

            return null;
        }

        public static DiscordEmoji ToDiscordEmoji(this ulong emojiId)
        {
            if (emojiId == 0)
                throw new ArgumentException("The emoji id can't be 0!");

            foreach (var guild in EntityBase._discordClient.Guilds.Values)
            {
                var member = guild.Emojis.Values.FirstOrDefault(r => r.Id == emojiId);
                if (member != null)
                    return member;
            }

            return null;
        }
    }
}