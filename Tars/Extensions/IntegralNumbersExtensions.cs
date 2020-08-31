using Tars.Core;
using DSharpPlus.Entities;
using System;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see langword="ulong"/> and <see langword="long"/> methods.
    /// </summary>
    public static class IntegralNumbersExtensions
    {
        #region Ulong
        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordMember"/>.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordMember"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordMember ToDiscordMember(this ulong memberId)
            => memberId == 0 ? throw new ArgumentException("The member id can't be 0!") : memberId.ToString().ToDiscordMember();

        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordRole"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordRole ToDiscordRole(this ulong roleId)
            => roleId == 0 ? throw new ArgumentException("The role id can't be 0!") : roleId.ToString().ToDiscordRole();

        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordEmoji"/>.
        /// </summary>
        /// <param name="emojiId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordEmoji"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordEmoji ToDiscordEmoji(this ulong emojiId)
            => emojiId == 0 ? throw new ArgumentException("The emoji id can't be 0!") : emojiId.ToString().ToDiscordEmoji();

        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordChannel"/>.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordChannel"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordChannel ToDiscordChannel(this ulong channelId)
        {
            if (channelId == 0)
                throw new ArgumentException("The channel id can't be 0!");

            try
            {
                return TarsBase._discordClient.GetChannelAsync(channelId).Result;
            }
            catch
            {
                return channelId.ToString().ToDiscordChannel();
            }
        }

        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordGuild"/>.
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordGuild"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordGuild ToDiscordGuild(this ulong guildId)
        {
            if (guildId == 0)
                throw new ArgumentException("The guild id can't be 0!");

            try
            {
                return TarsBase._discordClient.GetGuildAsync(guildId).Result;
            }
            catch
            {
                return guildId.ToString().ToDiscordGuild();
            }
        }

        /// <summary>
        /// Converts an <see langword="ulong"/> to <see cref="DiscordMessage"/>.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>An <see langword="ulong"/> with <see cref="DiscordMessage"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordMessage ToDiscordMessage(this ulong messageId)
            => messageId == 0 ? throw new ArgumentException("The message id can't be 0!") : messageId.ToString().ToDiscordMessage();
        #endregion

        #region Long
        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordMember"/>.
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordMember"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordMember ToDiscordMember(this long memberId) => ToDiscordMember((ulong)memberId);

        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordRole"/>.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordRole"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordRole ToDiscordRole(this long roleId) => ToDiscordRole((ulong)roleId);

        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordEmoji"/>.
        /// </summary>
        /// <param name="emojiId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordEmoji"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordEmoji ToDiscordEmoji(this long emojiId) => ToDiscordEmoji((ulong)emojiId);

        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordChannel"/>.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordChannel"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordChannel ToDiscordChannel(this long channelId) => ToDiscordChannel((ulong)channelId);

        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordGuild"/>.
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordGuild"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordGuild ToDiscordGuild(this long guildId) => ToDiscordGuild((ulong)guildId);

        /// <summary>
        /// Converts an <see langword="long"/> to <see cref="DiscordMessage"/>.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>An <see langword="long"/> with <see cref="DiscordMessage"/> or <see langword="null"/> if the bot can't find something.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static DiscordMessage ToDiscordMessage(this long messageId) => ToDiscordMessage((ulong)messageId);
        #endregion
    }
}