using DSharpPlus;
using DiscordBotBase.Core;
using System;

namespace DiscordBotBase.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DateTime"/> methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns the <see cref="DateTime"/> formatted according to DSharpPlus, the base takes the format of your PC automatically if you have not defined a different format.  <c>Attention!</c>  This method only works if you instantiated the bot without using <see langword="new"/> <see cref="DiscordConfiguration"/>.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatWithBaseSettings(this DateTime dateTime) => dateTime.ToString(BotBase._logTimestampFormat);
    }
}