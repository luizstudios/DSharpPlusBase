namespace DiscordBotBase.Core.Settings
{
    public sealed class BaseConfiguration
    {
        /// <summary>
        /// Set whether the bot will use DiscordBotBase's AutoReconnect. Attention! DSharpPlus AutoReconnect must be set to false.
        /// The default is true, so the DSharpPlus DiscordConfiguration comes standard with AutoReconnect as false.
        /// </summary>
        public bool AutoReconnect { get; set; } = true;
    }
}