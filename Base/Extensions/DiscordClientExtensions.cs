using DSharpPlus;
using DSharpPlus.Entities;
using DiscordBotBase.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBotBase.Extensions
{
    public static class DiscordClientExtensions
    {
        public static DiscordEmoji FindEmoji(this DiscordClient discordClient, string emojiNameOrId)
            => discordClient == null ? throw new ArgumentNullException("The DiscordClient can't be null!") :
                                       string.IsNullOrWhiteSpace(emojiNameOrId) ? throw new ArgumentNullException("The emoji name or Id can't be null!") :
                                                                                  BaseUtilities.FindEmoji(emojiNameOrId);

        public static DiscordRole FindRole(this DiscordClient discordClient, string roleNameOrId)
            => discordClient == null ? throw new ArgumentNullException("The DiscordClient can't be null!") : string.IsNullOrWhiteSpace(roleNameOrId) ?
                                       throw new ArgumentNullException("The role name or Id can't be null!") : StringExtensions.ToDiscordRole(roleNameOrId);

        public static async Task SendSameMessageToMultipleChannelsAsync(this DiscordClient discordClient, string content = null, bool tts = false, DiscordEmbed embed = null,
                                                                        IEnumerable<IMention> mentions = null, params DiscordChannel[] channels)
        {
            if (discordClient == null)
                throw new ArgumentNullException("The DiscordClient can't be null!");

            foreach (var channel in channels)
                await channel.SendMessageAsync(content, tts, embed, mentions);
        }

        public static DiscordMember FindMember(this DiscordClient discordClient, string memberNameOrId)
            => discordClient == null ? throw new ArgumentNullException("The DiscordClient can't be null!") : string.IsNullOrWhiteSpace(memberNameOrId) ?
                                       throw new ArgumentNullException("The member name or Id can't be null!") : StringExtensions.ToDiscordMember(memberNameOrId);

        public static async Task AddOverwriteOnMultipleChannelsAsync(this DiscordClient discordClient, DiscordMember member, Permissions allow = Permissions.None,
                                                                     Permissions deny = Permissions.None, string reason = null, params DiscordChannel[] channels)
        {
            if (discordClient == null)
                throw new ArgumentNullException("The DiscordClient can't be null!");

            foreach (var channel in channels)
                await channel.AddOverwriteAsync(member, allow, deny, reason);
        }

        public static async Task AddOverwriteOnMultipleChannelsAsync(this DiscordClient discordClient, DiscordRole role, Permissions allow = Permissions.None,
                                                                     Permissions deny = Permissions.None, string reason = null, params DiscordChannel[] channels)
        {
            if (discordClient == null)
                throw new ArgumentNullException("The DiscordClient can't be null!");

            foreach (var channel in channels)
                await channel.AddOverwriteAsync(role, allow, deny, reason);
        }
    }
}