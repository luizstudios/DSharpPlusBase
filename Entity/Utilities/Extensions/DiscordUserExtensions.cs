using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class DiscordUserExtensions
    {
        public static DiscordMember ToDiscordMember(this DiscordUser discordUser) 
            => discordUser == null ? throw new ArgumentNullException("The DiscordUser can't be null!") : (DiscordMember)discordUser;

        public static DiscordRole GetHighestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrded = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrded.FirstOrDefault() : memberRolesOrded.FirstOrDefault(predicate);
        }

        public static DiscordRole GetLowestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrded = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrded.LastOrDefault() : memberRolesOrded.LastOrDefault(predicate);
        }
    }
}