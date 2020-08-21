using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class DiscordMemberExtensions
    {
        public static DiscordUser ToDiscordUser(this DiscordMember discordMember) 
            => discordMember ?? throw new ArgumentNullException("The DiscordMember can't be null!");

        public static DiscordRole GetHighestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return discordMember == null ? throw new ArgumentNullException("The DiscordMember can't be null!") : predicate == null ? memberRolesOrded.FirstOrDefault() : 
                                                                                                                                     memberRolesOrded.FirstOrDefault(predicate);
        }

        public static DiscordRole GetLowestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return discordMember == null ? throw new ArgumentNullException("The DiscordMember can't be null!") : predicate == null ? memberRolesOrded.LastOrDefault() : 
                                                                                                                                     memberRolesOrded.LastOrDefault(predicate);
        }
    }
}