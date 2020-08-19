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

        /// <summary>
        /// Set if the bot will connect to Discord after the execution of the "await InitializeAsync();".
        /// Attention! Only set this property to true if you wanted to manually implement the "await Task.Delay (-1);" method, if you wanted to leave it as false, just call the "await InitializeAsync ()" method in the bot's main.
        /// The default is false.
        /// </summary>
        public bool ConnectAfterInitialize { get; set; } = false;
    }
}