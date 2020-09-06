using DSharpPlus.Entities;
using System;

namespace Tars.Classes
{
    /// <summary>
    /// Class to manage the colors of an embed in Discord.
    /// </summary>
    public static class DiscordEmbedColor
    {
        /// <summary>
        /// Generates a random color for the embed.
        /// </summary>
        /// <returns>The <see cref="DiscordColor"/>.</returns>
        public static DiscordColor RandomColorEmbed()
        {
            var rgbColor = new Random();
            return new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255));
        }
    }
}