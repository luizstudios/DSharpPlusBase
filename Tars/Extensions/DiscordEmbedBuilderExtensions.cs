using DSharpPlus.Entities;
using System;
using Tars.Utilities;

namespace Tars.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="DiscordEmbedBuilder"/> methods.
    /// </summary>
    public static class DiscordEmbedBuilderExtensions
    {
        /// <summary>
        /// Generates a random color for the embed.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns>This embed builder.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordEmbedBuilder WithColor(this DiscordEmbedBuilder embed)
        {
            embed.IsNotNull();

            var rgbColor = new Random();
            return embed.WithColor(new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255)));
        }

        /// <summary>
        /// Adds a blank field to the embed.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns>This embed builder.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static DiscordEmbedBuilder AddField(this DiscordEmbedBuilder embed)
        {
            embed.IsNotNull();

            return embed.AddField("\u200b", "\u200b");
        }
    }
}