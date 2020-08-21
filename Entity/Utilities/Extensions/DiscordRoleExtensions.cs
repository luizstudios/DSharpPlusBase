using DSharpPlus.Entities;
using Entity.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Utilities.Extensions
{
    public static class DiscordRoleExtensions
    {
        public static IReadOnlyList<DiscordMember> GetMembers(this DiscordRole role)
        {
            if (role == null)
                throw new ArgumentNullException("The role can't be null!");

            var guildRole = EntityBase._discordClient.Guilds.Values.FirstOrDefault(g => g.Roles.Values.Contains(role));
            return guildRole.Members.Values.Where(m => m.Roles.Contains(role)).ToList();
        }

        public static bool IsAbove(this DiscordRole roleAbove, DiscordRole role) 
            => roleAbove == null ? throw new ArgumentNullException("The above role can't be null!") : role == null ? 
                                                                                                      throw new ArgumentNullException("The role can't be null!") : 
                                                                                                      roleAbove.Position < role.Position;

        public static bool IsBelow(this DiscordRole roleBelow, DiscordRole role) 
            => roleBelow == null ? throw new ArgumentNullException("The below role can't be null!") : role == null ?
                                                                                                      throw new ArgumentNullException("The role can't be null!") : 
                                                                                                      roleBelow.Position > role.Position;

        public static decimal GetPercentageOfMembers(this DiscordRole role) 
            => role == null ? throw new ArgumentNullException("The role can't be null!") : Math.Round((decimal)GetMembers(role).Count() * 100 / 
                                                                                                      EntityBase._discordClient.Guilds.Values.FirstOrDefault(g => 
                                                                                                      g.Roles.Values.Contains(role)).MemberCount, 5);
    }
}