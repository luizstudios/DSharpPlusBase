using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Base.Core.Settings
{
    public sealed class EntityBaseConfiguration
    {
        /// <summary>
        /// Set whether the bot will use EntityBase's AutoReconnect. Attention! DSharpPlus AutoReconnect must be set to false.
        /// The default is true, so the DSharpPlus DiscordConfiguration comes standard with AutoReconnect as false.
        /// </summary>
        public bool AutoReconnect { get; set; } = true;
    }
}