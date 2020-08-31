using DSharpPlus.Entities;
using Tars.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordRole"/> methods.
    /// </summary>
    public static class DiscordRoleExtensions
    {
        /// <summary>
        /// Get a list that contains all the <see cref="DiscordMember"/> that have that role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>A <see cref="IReadOnlyList{DiscordRole}"/> with the members.</returns>
        public static IReadOnlyList<DiscordMember> GetMembers(this DiscordRole role)
        {
            if (role == null)
                throw new ArgumentNullException("The role can't be null!");

            var guildRole = TarsBase._discordClient.Guilds.Values.FirstOrDefault(g => g.Roles.Values.Contains(role));
            return guildRole.Members.Values.Where(m => m.Roles.Contains(role)).ToList();
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether this <see cref="DiscordRole"/> is above another <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleAbove"></param>
        /// <param name="role">The other <see cref="DiscordRole"/> to compare.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsAbove(this DiscordRole roleAbove, DiscordRole role)
            => roleAbove == null ? throw new ArgumentNullException("The above role can't be null!") : role == null ?
                                                                                                      throw new ArgumentNullException("The role can't be null!") :
                                                                                                      roleAbove.Position < role.Position;

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether this <see cref="DiscordRole"/> is below another <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleBelow"></param>
        /// <param name="role">The other <see cref="DiscordRole"/> to compare.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsBelow(this DiscordRole roleBelow, DiscordRole role)
            => roleBelow == null ? throw new ArgumentNullException("The below role can't be null!") : role == null ?
                                                                                                      throw new ArgumentNullException("The role can't be null!") :
                                                                                                      roleBelow.Position > role.Position;

        /// <summary>
        /// Returns a percentage of the members who hold this <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>A <see cref="decimal"/> with the percentage.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal GetPercentageOfMembers(this DiscordRole role)
            => role == null ? throw new ArgumentNullException("The role can't be null!") : Math.Round((decimal)GetMembers(role).Count * 100 /
                                                                                                      (int)TarsBase._discordClient.Guilds.Values.FirstOrDefault(g =>
                                                                                                      g.Roles.Values.Contains(role))?.MemberCount, 5);
    }
}