using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Base.Utilities.Classes
{
    public static class DiscordEmbedColor
    {
        public static DiscordColor RandomColor()
        {
            var rgbColor = new Random();
            return new DiscordColor((byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255), (byte)rgbColor.Next(0, 255));
        }

    }
}