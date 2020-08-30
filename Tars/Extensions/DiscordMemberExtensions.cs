using DSharpPlus;
using DSharpPlus.Entities;
using Tars.Core;
using System;
using System.Linq;

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
            => discordMember ?? throw new ArgumentNullException("The DiscordMember can't be null!");

        /// <summary>
        /// Get the highest role of this member in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The highest <see cref="DiscordRole"/> of the member in relation to the role of the Discord server.</returns>
        public static DiscordRole GetHighestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return discordMember == null ? throw new ArgumentNullException("The DiscordMember can't be null!") : predicate == null ? memberRolesOrded.FirstOrDefault() :
                                                                                                                                     memberRolesOrded.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get the lowest role of this member in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The lowest <see cref="DiscordRole"/> of the member in relation to the role of the Discord server.</returns>
        public static DiscordRole GetLowestRole(this DiscordMember discordMember, Func<DiscordRole, bool> predicate = null)
        {
            var memberRolesOrded = discordMember.Roles.OrderByDescending(r => r.Position);
            return discordMember == null ? throw new ArgumentNullException("The DiscordMember can't be null!") : predicate == null ? memberRolesOrded.LastOrDefault() :
                                                                                                                                     memberRolesOrded.LastOrDefault(predicate);
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the member can be banned from the server.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool CanBeBanned(this DiscordMember discordMember)
            => !discordMember.IsOwner && !discordMember.IsAdministrator() && discordMember.GetHighestRole().IsAbove(TarsBotBase._discordClient.CurrentUser.GetHighestRole());

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the member is a server administrator.
        /// </summary>
        /// <param name="discordMember"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsAdministrator(this DiscordMember discordMember)
            => discordMember.GetHighestRole().Permissions.HasPermission(Permissions.Administrator);
    }
}