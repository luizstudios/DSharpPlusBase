using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordGuild"/> methods.
    /// </summary>
    public static class DiscordGuildExtensions
    {
        /// <summary>
        /// Search for the <see cref="DiscordEmoji"/> across the Discord server.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="emojiNameOrId"><see cref="DiscordEmoji"/> name or id.</param>
        /// <returns>The <see cref="DiscordEmoji"/> found, or <see langword="null"/> if the bot can't find it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordEmoji FindEmoji(this DiscordGuild guild, string emojiNameOrId)
        {
            guild.IsNotNull();
            emojiNameOrId.IsNotNull();

            emojiNameOrId = emojiNameOrId.ToLower();
            ulong.TryParse(emojiNameOrId, out ulong emojiId);

            return guild.Emojis.Values.FirstOrDefault(e => string.Equals(e.Name, emojiNameOrId.Replace(":", "")) ||
                                                           string.Equals(e.ToString(), emojiNameOrId, StringComparison.CurrentCultureIgnoreCase) || e.Id == emojiId);
        }

        /// <summary>
        /// Search for the <see cref="DiscordRole"/> across the Discord server.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="roleNameOrId"><see cref="DiscordRole"/> name or id.</param>
        /// <returns>The <see cref="DiscordRole"/> found, or <see langword="null"/> if the bot can't find it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordRole FindRole(this DiscordGuild guild, string roleNameOrId)
        {
            guild.IsNotNull();
            roleNameOrId.IsNotNull();

            ulong.TryParse(roleNameOrId, out ulong resultId);

            return guild.Roles.Values.FirstOrDefault(r => string.Equals(r.Name, roleNameOrId, StringComparison.CurrentCultureIgnoreCase) || r.Id == resultId);
        }

        /// <summary>
        /// Get a list of all Discord server roles organized by the Discord hierarchy.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns><see cref="IReadOnlyList{DiscordRole}"/> with the roles.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static IReadOnlyList<DiscordRole> GetOrganizedRoles(this DiscordGuild guild)
        {
            guild.IsNotNull();
            return guild.Roles.Values.OrderByDescending(r => r.Position).ToList();
        }

        /// <summary>
        /// Get the highest role of the Discord server following the Discord hierarchy.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Discord server highest <see cref="DiscordRole"/>.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordRole GetHighestRole(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            guild.IsNotNull();

            var organizedRoles = guild.GetOrganizedRoles();
            return predicate is null ? organizedRoles.FirstOrDefault() : organizedRoles.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get the lowest role of the Discord server after "@everyone".
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Discord server lowest <see cref="DiscordRole"/>.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordRole GetLowestRoleAfterEveryone(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            guild.IsNotNull();

            var organizedRoles = guild.GetOrganizedRoles();

            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;

            return predicate is null ? organizedRoles.LastOrDefault(r => !string.Equals(r.Name, "@everyone", stringComparison)) :
                                       organizedRoles.Where(r => !string.Equals(r.Name, "@everyone", stringComparison)).LastOrDefault(predicate);
        }

        /// <summary>
        /// Deletes a channel.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="channelNameOrId">Channel name or id.</param>
        /// <param name="reason">Reason for audit logs.</param>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task DeleteChannelAsync(this DiscordGuild guild, string channelNameOrId, string reason = null)
        {
            guild.IsNotNull();

            await channelNameOrId.ToDiscordChannel().DeleteAsync(reason);
        }

        /// <summary>
        /// Deletes a channel.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="channel">Channel object.</param>
        /// <param name="reason">Reason for audit logs.</param>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task DeleteChannelAsync(this DiscordGuild guild, DiscordChannel channel, string reason = null)
        {
            guild.IsNotNull();

            await channel.DeleteAsync(reason);
        }

        /// <summary>
        /// Get the highest role of the Discord server following the Discord hierarchy and according to the permissions passed.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="permissions">Permissions of the role.</param>
        public static DiscordRole GetHighestRoleWithPermissions(this DiscordGuild guild, Permissions permissions = Permissions.None)
        {
            guild.IsNotNull();

            return guild.GetOrganizedRoles().FirstOrDefault(r => r.Permissions.HasPermission(permissions));
        }

        /// <summary>
        /// Get the lowest role of the Discord server following the Discord hierarchy and according to the permissions passed.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="permissions"></param>
        public static DiscordRole GetLowestRoleWithPermissions(this DiscordGuild guild, Permissions permissions = Permissions.None)
        {
            guild.IsNotNull();

            return guild.GetOrganizedRoles().LastOrDefault(r => r.Permissions.HasPermission(permissions));
        }
    }
}