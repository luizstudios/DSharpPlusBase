using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Utilities.Extensions
{
    public static class DiscordGuildExtensions
    {
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

        public static DiscordRole FindRole(this DiscordGuild guild, string roleNameOrId)
        {
            if (guild == null)
                throw new ArgumentNullException("The guild can't be null!");

            if (string.IsNullOrWhiteSpace(roleNameOrId))
                throw new ArgumentNullException("The emoji name or Id can't be null!");

            ulong.TryParse(roleNameOrId, out ulong resultId);

            return guild.Roles.Values.FirstOrDefault(r => r.Name.ToLower() == roleNameOrId.ToLower() || r.Id == resultId);
        }

        public static IReadOnlyList<DiscordRole> GetOrganizedRoles(this DiscordGuild guild) 
            => guild == null ? throw new ArgumentNullException("The guild can't be null!") : guild.Roles.Values.OrderByDescending(r => r.Position).ToList();

        public static DiscordRole GetHighestRole(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            var organizedRoles = guild.GetOrganizedRoles();
            return guild == null ? throw new ArgumentNullException("The guild can't be null!") : predicate == null ? organizedRoles.FirstOrDefault() : 
                                                                                                                     organizedRoles.FirstOrDefault(predicate);
        }

        public static DiscordRole GetLowestRoleAfterEveryone(this DiscordGuild guild, Func<DiscordRole, bool> predicate = null)
        {
            var organizedRoles = guild.GetOrganizedRoles();
            return guild == null ? throw new ArgumentNullException("The guild can't be null!") : 
                                   predicate == null ? organizedRoles.Where(r => r.Name.ToLower() != "@everyone").LastOrDefault() :
                                                       organizedRoles.Where(r => r.Name.ToLower() != "@everyone").LastOrDefault(predicate);
        }
    }
}