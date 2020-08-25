using DSharpPlus.Entities;
using System;

namespace DiscordBotBase.Extensions
{
    public static class IntegralNumbersExtensions
    {
        #region Ulong
        public static DiscordMember ToDiscordMember(this ulong memberId)
            => memberId == 0 ? throw new ArgumentException("The member id can't be 0!") : StringExtensions.ToDiscordMember(memberId.ToString());

        public static DiscordRole ToDiscordRole(this ulong roleId)
            => roleId == 0 ? throw new ArgumentException("The role id can't be 0!") : StringExtensions.ToDiscordRole(roleId.ToString());

        public static DiscordEmoji ToDiscordEmoji(this ulong emojiId)
            => emojiId == 0 ? throw new ArgumentException("The emoji id can't be 0!") : StringExtensions.ToDiscordEmoji(emojiId.ToString());

        public static DiscordChannel ToDiscordChannel(this ulong channelId)
            => channelId == 0 ? throw new ArgumentException("The channel id can't be 0!") : StringExtensions.ToDiscordChannel(channelId.ToString());
        #endregion

        #region Long
        public static DiscordMember ToDiscordMember(this long memberId) => ToDiscordMember((ulong)memberId);

        public static DiscordRole ToDiscordRole(this long roleId) => ToDiscordRole((ulong)roleId);

        public static DiscordEmoji ToDiscordEmoji(this long emojiId) => ToDiscordEmoji((ulong)emojiId);

        public static DiscordChannel ToDiscordChannel(this long channelId) => ToDiscordChannel((ulong)channelId);
        #endregion
    }
}