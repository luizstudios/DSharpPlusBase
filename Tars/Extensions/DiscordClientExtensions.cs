using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tars.Exceptions;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordClient"/> methods.
    /// </summary>
    public static class DiscordClientExtensions
    {
        /// <summary>
        /// Search for a emoji on all the servers the bot is on, this method is compatible with standard <see cref="DiscordEmoji"/>.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="emojiNameOrId">Emoji name (can be with ":" or without) or id of it, it can also be the name of a <see cref="DiscordEmoji"/>, the method will also look for.</param>
        /// <returns>A <see cref="DiscordEmoji"/> with the found emoji or <see langword="null"/> if the bot found nothing..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordEmoji FindEmoji(this DiscordClient _, string emojiNameOrId) => TarsBaseUtilities.FindEmoji(emojiNameOrId);

        /// <summary>
        /// Search for a role on all the servers the bot is on.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="roleNameOrId">Role name or id.</param>
        /// <returns>A <see cref="DiscordRole"/> with the found role or <see langword="null"/> if the bot found nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordRole FindRole(this DiscordClient _, string roleNameOrId) => roleNameOrId.ToDiscordRole();

        /// <summary>
        /// Sends the same message to different channels.
        /// </summary>
        /// <param name="discordClient"></param>
        /// <param name="content">Content of the message to send.</param>
        /// <param name="tts">Whether the message is to be read using TTS.</param>
        /// <param name="embed">Embed to attach to the message.</param>
        /// <param name="mentions">Allowed mentions in the message.</param>
        /// <param name="channels">Channels for the message to be sent.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ElementIsNullException"></exception>
        /// <returns></returns>
        public static async Task SendSameMessageToMultipleChannelsAsync(this DiscordClient discordClient, string content = null, bool tts = false, DiscordEmbed embed = null,
                                                                        IEnumerable<IMention> mentions = null, params DiscordChannel[] channels)
        {
            discordClient.IsNotNull();
            channels.IsNotNull();

            var tasks = new List<Task<DiscordMessage>>();
            foreach (var channel in channels)
                tasks.Add(channel.SendMessageAsync(content, tts, embed, mentions));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Searches for a member on all servers the bot is on.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="memberNameOrId">Member name or id.</param>
        /// <returns>A <see cref="DiscordMember"/> with the found member or <see langword="null"/> if the bot found nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordMember FindMember(this DiscordClient _, string memberNameOrId) => memberNameOrId.ToDiscordMember();

        /// <summary>
        /// Adds the same permission on different channels.
        /// </summary>
        /// <param name="discordClient"></param>
        /// <param name="member">Member to be added to permission.</param>
        /// <param name="allow">Permissions to be released.</param>
        /// <param name="deny">Permissions to be denied.</param>
        /// <param name="reason">Reason for audit logs.</param>
        /// <param name="channels">Discord channels for adding permissions.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ElementIsNullException"></exception>
        /// <returns></returns>
        public static async Task AddOverwriteOnMultipleChannelsAsync(this DiscordClient discordClient, DiscordMember member, Permissions allow = Permissions.None,
                                                                     Permissions deny = Permissions.None, string reason = null, params DiscordChannel[] channels)
        {
            discordClient.IsNotNull();
            channels.IsNotNull();

            var tasks = new List<Task>();
            foreach (var channel in channels)
                tasks.Add(channel.AddOverwriteAsync(member, allow, deny, reason));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Adds the same permission on different channels.
        /// </summary>
        /// <param name="discordClient"></param>
        /// <param name="role">Role to be added to permission.</param>
        /// <param name="allow">Permissions to be released.</param>
        /// <param name="deny">Permissions to be denied.</param>
        /// <param name="reason">Reason for audit logs.</param>
        /// <param name="channels">Discord channels for adding permissions.</param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ElementIsNullException"></exception>
        /// <returns></returns>
        public static async Task AddOverwriteOnMultipleChannelsAsync(this DiscordClient discordClient, DiscordRole role, Permissions allow = Permissions.None,
                                                                     Permissions deny = Permissions.None, string reason = null, params DiscordChannel[] channels)
        {
            discordClient.IsNotNull();
            role.IsNotNull();
            channels.IsNotNull();

            var tasks = new List<Task>();
            foreach (var channel in channels)
                tasks.Add(channel.AddOverwriteAsync(role, allow, deny, reason));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Sends a log message to the console using the DSharpPlus logger.
        /// </summary>
        /// <param name="discordClient"></param>
        /// <param name="message">Message to be displayed on the console.</param>
        /// <param name="exception">Exception if the message is an error.</param>
        /// <param name="logLevel">The message level, the default is <see cref="LogLevel.Information"/>.</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void LogMessage(this DiscordClient discordClient, string message, Exception exception = null, LogLevel logLevel = LogLevel.Information, EventId? eventId = null)
        {
            discordClient.IsNotNull();
            message.IsNotNull();

            discordClient.Logger.Log(logLevel, eventId ?? default, exception, message);
        }

        /// <summary>
        /// Searches for a message on all servers the bot is on.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="messageId">Message id.</param>
        /// <returns>A <see cref="DiscordMessage"/> with the found message or <see langword="null"/> if the bot found nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordMessage FindMessage(this DiscordClient _, string messageId) => messageId.ToDiscordMessage();

        /// <summary>
        /// Get the bot as a <see cref="DiscordMember"/>.
        /// </summary>
        /// <param name="discordClient"></param>
        /// <returns>The bot in <see cref="DiscordMember"/> format.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordMember GetBotMember(this DiscordClient discordClient)
        {
            discordClient.IsNotNull();

            return discordClient.CurrentUser.ToDiscordMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_"></param>
        /// <param name="member"></param>
        /// <param name="role"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public static async Task GrantRoleAsync(this DiscordClient _, DiscordMember member, DiscordRole role, string reason = null) => await member.GrantRoleAsync(role, reason);

        public static async Task RevokeRoleAsync(this DiscordClient _, DiscordMember member, DiscordRole role, string reason = null) => await member.RevokeRoleAsync(role, reason);

        public static async Task ReplaceRolesAsync(this DiscordClient _, DiscordMember member, IEnumerable<DiscordRole> roles, string reason = null) => await member.ReplaceRolesAsync(roles, reason);
    }
}