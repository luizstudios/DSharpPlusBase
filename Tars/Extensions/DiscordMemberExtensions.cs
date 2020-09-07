using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Linq;
using Tars.Core;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordMember"/> methods.
    /// </summary>
    public static class DiscordMemberExtensions
    {
        /// <summary>
        /// Convert <see cref="DiscordMember"/> to <see cref="DiscordUser"/>.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <returns>The <see cref="DiscordUser"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DiscordUser ToDiscordUser(this DiscordMember discordMember)
        {
            discordMember.IsNotNull();
            return discordMember;
        }

        /// <summary>
        /// Get the highest role of this member in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The highest <see cref="DiscordRole"/> of the member in relation to the role of the Discord server.</returns>
        public static DiscordRole GetHighestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            discordMember.IsNotNull();

            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return predicate is null ? memberRolesOrded.FirstOrDefault() : memberRolesOrded.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get the lowest role of this member in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The lowest <see cref="DiscordRole"/> of the member in relation to the role of the Discord server.</returns>
        public static DiscordRole GetLowestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            discordMember.IsNotNull();

            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return predicate is null ? memberRolesOrded.LastOrDefault() : memberRolesOrded.LastOrDefault(predicate);
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the member can be banned from the server. <c>Attention!</c> can be banned by the <c>bot</c>.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool CanBeBanned(this DiscordMember discordMember)
        {
            discordMember.IsNotNull();

            return !discordMember.IsOwner && !discordMember.IsAdministrator() && discordMember.GetHighestRole().IsBelow(TarsBase._discordClient.CurrentUser.GetHighestRole());
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the member is a server administrator.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsAdministrator(this DiscordMember discordMember)
        {
            discordMember.IsNotNull();

            return discordMember.Roles.Any(r => r.Permissions.HasPermission(Permissions.Administrator));
        }
    }
}