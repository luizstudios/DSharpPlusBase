using DSharpPlus.Entities;
using System;

namespace Tars.Classes
{
    /// <summary>
    /// Color of embeds.
    /// </summary>
    public static class DiscordEmbedColor
    {
        /// <summary>
        /// Generates a random color for an embed.
        /// </summary>
        /// <returns>A <see cref="DiscordColor"/> with color.</returns>
        public static DiscordColor RandomColor()
        {
            var rgbColor = new Random();
            return new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255));
        }
    }
}