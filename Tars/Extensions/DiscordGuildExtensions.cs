﻿using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static DiscordEmoji FindEmoji(this DiscordGuild guild, string emojiNameOrId)
        {
            if (guild == null)
                throw new ArgumentNullException("The guild can't be null!");

            if (string.IsNullOrWhiteSpace(emojiNameOrId))
                throw new ArgumentNullException("The emoji name or Id can't be null!");

            string oldNameEmoji = emojiNameOrId;
            emojiNameOrId = emojiNameOrId.ToLower();
            ulong.TryParse(emojiNameOrId, out ulong emojiId);

            return guild.Emojis.Values.FirstOrDefault(e => e.Name.ToLower() == emojiNameOrId.Replace(":", "") || e.Id == emojiId || e.ToString().ToLower() == emojiNameOrId);
        }

        /// <summary>
        /// Search for the <see cref="DiscordRole"/> across the Discord server.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="roleNameOrId"><see cref="DiscordRole"/> name or id.</param>
        /// <returns>The <see cref="DiscordRole"/> found, or <see langword="null"/> if the bot can't find it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DiscordRole FindRole(this DiscordGuild guild, string roleNameOrId)
        {
            if (guild == null)
                throw new ArgumentNullException("The guild can't be null!");

            if (string.IsNullOrWhiteSpace(roleNameOrId))
                throw new ArgumentNullException("The emoji name or Id can't be null!");

            ulong.TryParse(roleNameOrId, out ulong resultId);

            return guild.Roles.Values.FirstOrDefault(r => string.Equals(r.Name, roleNameOrId, StringComparison.CurrentCultureIgnoreCase) || r.Id == resultId);
        }

        /// <summary>
        /// Get a list of all Discord server roles organized by the Discord hierarchy.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns><see cref="IReadOnlyList{DiscordRole}"/> with the roles.</returns>
        public static IReadOnlyList<DiscordRole> GetOrganizedRoles(this DiscordGuild guild)
            => guild == null ? throw new ArgumentNullException("The guild can't be null!") : guild.Roles.Values.OrderByDescending(r => r.Position).ToList();

        /// <summary>
        /// Get the highest role of the Discord server following the Discord hierarchy.
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Discord server highest <see cref="DiscordRole"/>.</returns>
        public static DiscordRole GetHighestRole(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            var organizedRoles = guild.GetOrganizedRoles();
            return guild == null ? throw new ArgumentNullException("The guild can't be null!") : predicate == null ? organizedRoles.FirstOrDefault() :
                                                                                                                     organizedRoles.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Get the lowest role of the Discord server after "@everyone".
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Discord server lowest <see cref="DiscordRole"/>.</returns>
        public static DiscordRole GetLowestRoleAfterEveryone(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            var organizedRoles = guild.GetOrganizedRoles();
            return guild == null ? throw new ArgumentNullException("The guild can't be null!") :
                                   predicate == null ? organizedRoles.LastOrDefault(r => !string.Equals(r.Name, "@everyone", StringComparison.CurrentCultureIgnoreCase)) :
                                                       organizedRoles.Where(r => !string.Equals(r.Name, "@everyone", StringComparison.CurrentCultureIgnoreCase)).LastOrDefault(predicate);
        }
    }
}