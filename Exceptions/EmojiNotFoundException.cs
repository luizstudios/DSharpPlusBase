using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlusBase.Exceptions
{
    public sealed class EmojiNotFoundException : Exception
    {
        public string EmojiName { get; private set; }

        internal EmojiNotFoundException(string emojiName) : base($"The {emojiName} emoji was not found!") => this.EmojiName = emojiName;
    }
}