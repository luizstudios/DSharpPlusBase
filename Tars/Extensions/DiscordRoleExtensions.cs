using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Tars.Core;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordRole"/> methods.
    /// </summary>
    public static class DiscordRoleExtensions
    {
        #region Public methods
        /// <summary>
        /// Get a list that contains all the <see cref="DiscordMember"/> that have that role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>A <see cref="IReadOnlyList{DiscordRole}"/> with the members.</returns>
        public static IReadOnlyList<DiscordMember> GetMembers(this DiscordRole role)
        {
            role.IsNotNull();

            return CheckIfRoleExistsAndReturnTheGuild(role).Members.Values.Where(m => m.Roles.Contains(role)).ToList();
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether this <see cref="DiscordRole"/> is above another <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleAbove"></param>
        /// <param name="role">The other <see cref="DiscordRole"/> to compare.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsAbove(this DiscordRole roleAbove, DiscordRole role)
        {
            roleAbove.IsNotNull();
            role.IsNotNull();

            return roleAbove.Position > role.Position;
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether this <see cref="DiscordRole"/> is below another <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleBelow"></param>
        /// <param name="role">The other <see cref="DiscordRole"/> to compare.</param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsBelow(this DiscordRole roleBelow, DiscordRole role)
        {
            roleBelow.IsNotNull();
            role.IsNotNull();

            return roleBelow.Position < role.Position;
        }

        /// <summary>
        /// Returns a percentage of the members who hold this <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>A <see cref="decimal"/> with the percentage.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static decimal GetPercentageOfMembers(this DiscordRole role)
        {
            role.IsNotNull();

            return Math.Round((decimal)GetMembers(role).Count * 100 / CheckIfRoleExistsAndReturnTheGuild(role).MemberCount, 5);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Checks if any of the guilds that the bot is in contain the <see cref="DiscordRole"/> that was specified. If so, the <see cref="DiscordRole"/> will be returned.
        /// </summary>
        /// <param name="role"></param>
        private static DiscordGuild CheckIfRoleExistsAndReturnTheGuild(DiscordRole role)
            => TarsBase._discordClient.Guilds.Values.FirstOrDefault(g => g.Roles.Values.Contains(role)) ??
               throw new NullReferenceException($"No {nameof(DiscordGuild)} containing this {nameof(role)} were found!");
        #endregion
    }
}