using DSharpPlus;
using DSharpPlus.Entities;
using DiscordBotBase.Core;
using System;
using System.Linq;

namespace DiscordBotBase.Extensions
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

        public static bool CanBeBanned(this DiscordMember discordMember)
            => !discordMember.IsOwner && !discordMember.IsAdministrator() && discordMember.GetHighestRole().IsAbove(BotBase._discordClient.CurrentUser.GetHighestRole());

        public static bool IsAdministrator(this DiscordMember discordMember) 
            => discordMember.GetHighestRole().Permissions.HasPermission(Permissions.Administrator);
    }
}