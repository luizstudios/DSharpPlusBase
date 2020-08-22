using DiscordBotBase.Core;
using System;

namespace DiscordBotBase.Extensions
{
    public static class DateTimeExtensions
    {
        public static string FormatWithBaseSettings(this DateTime dateTime) => dateTime.ToString(BotBase._discordBotBaseDiscordConfiguration.DateTimeFormat);
    }
}