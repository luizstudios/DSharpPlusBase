using DSharpPlus;

namespace Tars.Core.Settings
{
    /// <summary>
    /// Class where the Tars settings are defined.
    /// </summary>
    public sealed class BaseConfiguration
    {
        /// <summary>
        /// Set whether the bot will use Tars's AutoReconnect. Attention! DSharpPlus AutoReconnect must be set to false.
        /// The default is true, so the DSharpPlus <see cref="DiscordConfiguration"/> comes standard with AutoReconnect as false.
        /// </summary>
        public bool AutoReconnect { get; set; } = true;
    }
}