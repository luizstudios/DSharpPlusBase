using DSharpPlus;
using DSharpPlus.Entities;
using DiscordBotBase.Core;
using System;
using System.Linq;

namespace DiscordBotBase.Extensions
{
    public static class DiscordUserExtensions
    {
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

        public static DiscordRole GetHighestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrdered = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrdered.FirstOrDefault() : memberRolesOrdered.FirstOrDefault(predicate);
        }

        public static DiscordRole GetLowestRole(this DiscordUser discordUser, Func<DiscordRole, bool> predicate = null)
        {
            if (discordUser == null)
                throw new ArgumentNullException("The DiscordUser can't be null!");

            var memberRolesOrded = discordUser.ToDiscordMember().Roles.OrderByDescending(r => r.Position);
            return predicate == null ? memberRolesOrded.LastOrDefault() : memberRolesOrded.LastOrDefault(predicate);
        }

        public static bool CanBeBanned(this DiscordUser discordUser)
        {
            var discordMember = discordUser.ToDiscordMember();
            return !discordMember.IsOwner && !discordMember.IsAdministrator() && discordMember.GetHighestRole().IsAbove(BotBase._discordClient.CurrentUser.GetHighestRole());
        }

        public static bool IsAdministrator(this DiscordUser discordUser) => discordUser.GetHighestRole().Permissions.HasPermission(Permissions.Administrator);
    }
}