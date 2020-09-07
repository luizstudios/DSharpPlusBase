﻿using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tars.Core;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see langword="string"/> methods.
    /// </summary>
    public static class StringExtensions
    {
        #region Public methods
        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordMember"/>. Example: "Member id", "Member nickname"
        /// </summary>
        /// <param name="stringMemberOrId"></param>
        /// <returns>A <see cref="DiscordMember"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordMember ToDiscordMember(this string stringMemberOrId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            stringMemberOrId.IsNotNull();

            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;

            ulong.TryParse(GetStringNumbers(stringMemberOrId), out ulong memberId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                foreach (DiscordMember member in guild.Members.Values)
                {
                    string memberDisplayName = member.DisplayName,
                           memberNickname = member.Nickname,
                           memberUsername = member.Username,
                           memberDiscriminator = member.Discriminator;

                    if (string.Equals($"{memberDisplayName}#{memberDiscriminator}", stringMemberOrId, stringComparison) ||
                        string.Equals($"{memberNickname}#{memberDiscriminator}", stringMemberOrId, stringComparison) ||
                        string.Equals($"{memberUsername}#{memberDiscriminator}", stringMemberOrId, stringComparison) ||
                        string.Equals(memberDisplayName, stringMemberOrId, stringComparison) ||
                        string.Equals(memberNickname, stringMemberOrId, stringComparison) ||
                        string.Equals(memberUsername, stringMemberOrId, stringComparison) ||
                        member.Id == memberId)
                        return member;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordEmoji"/>. Example: "Emoji id", "Emoji name"
        /// </summary>
        /// <param name="stringEmojiOrId"></param>
        /// <returns>A <see cref="DiscordEmoji"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordEmoji ToDiscordEmoji(this string stringEmojiOrId) => TarsBaseUtilities.FindEmoji(stringEmojiOrId);

        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordRole"/>. Example: "Role id", "Role name"
        /// </summary>
        /// <param name="stringRoleOrId"></param>
        /// <returns>A <see cref="DiscordRole"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordRole ToDiscordRole(this string stringRoleOrId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            stringRoleOrId.IsNotNull();

            ulong.TryParse(GetStringNumbers(stringRoleOrId), out ulong resultRoleId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var role = guild.Roles.Values.FirstOrDefault(r => string.Equals(r.Name, stringRoleOrId, StringComparison.CurrentCultureIgnoreCase) || r.Id == resultRoleId);
                if (role != null)
                    return role;
            }

            return null;
        }

        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordChannel"/>. Example: "Channel id", "Channel name (without "_" or "-")"
        /// </summary>
        /// <param name="stringChannelOrId"></param>
        /// <returns>A <see cref="DiscordChannel"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordChannel ToDiscordChannel(this string stringChannelOrId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            stringChannelOrId.IsNotNull();

            ulong.TryParse(GetStringNumbers(stringChannelOrId), out ulong channelId);

            if (channelId != 0)
            {
                try
                {
                    return discordClient.GetChannelAsync(channelId).Result;
                }
                catch { }
            }

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                var channel = guild.Channels.Values.FirstOrDefault(c => string.Equals(Regex.Replace(c.Name, @"[^\w]", "").Replace("-", " ").Replace("_", " "),
                                                                                      stringChannelOrId, StringComparison.CurrentCultureIgnoreCase) || c.Id == channelId);
                if (channel != null)
                    return channel;
            }

            return null;
        }

        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordGuild"/>. Example: "Guild id", "Guild name"
        /// </summary>
        /// <param name="stringGuildOrId"></param>
        /// <returns>A <see cref="DiscordGuild"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordGuild ToDiscordGuild(this string stringGuildOrId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            stringGuildOrId.IsNotNull();

            ulong.TryParse(GetStringNumbers(stringGuildOrId), out ulong guildId);

            return discordClient.Guilds.Values.FirstOrDefault(g => string.Equals(g.Name, stringGuildOrId, StringComparison.CurrentCulture) || g.Id == guildId);
        }

        /// <summary>
        /// Checks whether the string starts with numbers.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        //public static bool StartWithNumber(this string stringValue) => Regex.IsMatch(stringValue, @"^\d");

        /// <summary>
        /// Checks <code>string.IsNullOrEmpty()</code> and <code>string.IsNullOrWhiteSpace()</code> and returns a <see langword="bool"/>.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string stringValue) => string.IsNullOrEmpty(stringValue) || string.IsNullOrWhiteSpace(stringValue);

        /// <summary>
        /// Convert a <see langword="string"/> to <see cref="DiscordMessage"/>. <c>Attention!</c> Just use the message id, it is not yet possible to search for a message other than its id. This method is a little slow to search for the message, so wait and let it finish the search.
        /// </summary>
        /// <param name="stringMessageId"></param>
        /// <returns>A <see cref="DiscordMessage"/> or <see langword="null"/> if the bot finds nothing.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordMessage ToDiscordMessage(this string stringMessageId)
        {
            DiscordClient discordClient = TarsBase._discordClient;
            discordClient.IsNotNull();
            stringMessageId.IsNotNull();

            var messageId = ulong.Parse(stringMessageId);

            foreach (DiscordGuild guild in discordClient.Guilds.Values)
            {
                foreach (DiscordChannel channel in guild.Channels.Values)
                {
                    try
                    {
                        var message = channel.GetMessageAsync(messageId).Result;
                        if (message != null)
                            return message;
                    }
                    catch { }
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the string is empty or contains a white space.
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns>A <see langword="bool"/>.</returns>
        public static bool IsEmptyOrWhiteSpace(this string stringValue) => stringValue?.Length == 0 || stringValue == " ";
        #endregion

        #region Private methods
        /// <summary>
        /// Gets only numbers on <see langword="string"/>.
        /// </summary>
        /// <param name="stringValue"><see langword="string"/> to return only numbers.</param>
        /// <returns>The numbers in the form of a <see langword="string"/>.</returns>
        private static string GetStringNumbers(string stringValue) => Regex.Replace(stringValue, @"[^\d]", string.Empty);
        #endregion
    }
}