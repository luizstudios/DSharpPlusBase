using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Linq;
using Tars.Core;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordUser"/> methods.
    /// </summary>
    public static class DiscordUserExtensions
    {
        /// <summary>
        /// Convert <see cref="DiscordUser"/> to <see cref="DiscordMember"/>.
        /// </summary>
        /// <param name="discordUser"></param>
        /// <returns>The <see cref="DiscordMember"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DiscordMember ToDiscordMember(this DiscordUser discordUser)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            try
            {
                return (DiscordMember)discordUser;
            }
            catch (InvalidCastException)
            {
                return discordUser.Id.ToDiscordMember();
            }
        }

        /// <summary>
        /// Get the highest role of this user in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordUser"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The highest <see cref="DiscordRole"/> of the user in relation to the role of the Discord server.</returns>
        public static DiscordRole GetHighestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrdered = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrdered.FirstOrDefault() : memberRolesOrdered.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get the lowest role of this user in relation to the hierarchy of the Discord server.
        /// </summary>
        /// <param name="discordUser"></param>
        /// <param name="predicate"></param>
        /// <returns>The lowest <see cref="DiscordRole"/> of the user in relation to the role of the Discord server.</returns>
        public static DiscordRole GetLowestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrded = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrded.LastOrDefault() : memberRolesOrded.LastOrDefault(predicate);
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the member can be banned from the server. <c>Attention!</c> can be banned by the <c>bot</c>.
        /// </summary>
        /// <param name="discordUser"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool CanBeBanned(this DiscordUser discordUser)
        {
            var discordMember = discordUser.ToDiscordMember();
            return !discordMember.IsOwner && !discordMember.IsAdministrator() && discordMember.GetHighestRole().IsBelow(TarsBase._discordClient.CurrentUser.GetHighestRole());
        }

        /// <summary>
        /// Returns a <see langword="bool"/> that says whether the user is a server administrator.
        /// </summary>
        /// <param name="discordUser"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsAdministrator(this DiscordUser discordUser) => discordUser.ToDiscordMember().Roles.Any(r => r.Permissions.HasPermission(Permissions.Administrator));
    }
}