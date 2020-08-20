using DSharpPlus;
using DSharpPlus.Entities;
using Entity.Base.Core;
using Entity.Base.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class DiscordClientExtensions
    {
        public static DiscordEmoji FindEmoji(this DiscordClient _, string emojiNameOrId) => EntityBaseUtilities.FindEmoji(emojiNameOrId);

        public static DiscordRole FindRole(this DiscordClient discordClient, string roleNameOrId)
        {
            if (string.IsNullOrWhiteSpace(roleNameOrId))
                throw new ArgumentNullException("The role name or Id can't be null!");

            foreach (var guild in discordClient.Guilds.Values)
            {
                var role = guild.Roles.Values.FirstOrDefault(r => r.Name == roleNameOrId.ToLower() || r.Id == roleNameOrId.ToDiscordRole()?.Id);
                if (role != null)
                    return role;
            }

            return null;
        }
    }
}