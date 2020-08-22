using DiscordBotBase.Core.Settings;
using DiscordClientConfiguration = DiscordBotBase.Core.Settings.DiscordConfiguration;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus;
using DSharpPlusDiscordConfiguration = DSharpPlus.DiscordConfiguration;
using DSharpPlusInteractivity = DSharpPlus.Interactivity.InteractivityConfiguration;
using InteractivityExtensionConfiguration = DiscordBotBase.Core.Settings.InteractivityConfiguration;
using System.Reflection;
using System.Threading.Tasks;
using System;

namespace DiscordBotBase.Core
{
    /// <summary>
    /// Class where the bot is instantiated using DiscordBotBase.
    /// </summary>
    public sealed class BotBase
    {
        /// <summary>
        /// Class where the DiscordBotBase is configured.
        /// </summary>
        public BaseConfiguration BaseConfiguration { get; set; }

        /// <summary>
        /// Class where the Discord settings are configured.
        /// </summary>
        public DiscordClientConfiguration DiscordConfiguration { get; set; }

        /// <summary>
        /// Class where the CommandsNext settings are configured.
        /// </summary>
        public CommandsConfiguration CommandsConfiguration { get; set; }

        /// <summary>
        /// Class where the Interactivity settings are configured.
        /// </summary>
        public InteractivityExtensionConfiguration InteractivityConfiguration { get; set; }

        internal static DiscordClientConfiguration _discordBotBaseDiscordConfiguration;

        internal static DiscordClient _discordClient;
        private CommandsNextExtension _commandsNextExtension;
        private InteractivityExtension _interactivityExtension;

        private readonly object _botClassOrAssembly;

        /// <summary>
        /// Constructor of DiscordBotBase.
        /// </summary>
        /// <param name="botClassOrAssembly">Bot class (Use the "this" keyword if the class where you are instantiating the bot is not static) or the assembly (Use Assembly.GetEntryAssembly()) to register commands in CommandsNext.</param>
        public BotBase(object botClassOrAssembly) => this._botClassOrAssembly = botClassOrAssembly;

        /// <summary>
        /// Method where the bot settings previously defined are applied.
        /// </summary>
        public void Initialize()
        {
            #region DiscordClient
            var discordBotBaseDiscordConfiguration = _discordBotBaseDiscordConfiguration = this.DiscordConfiguration ??
                                                     throw new NullReferenceException("Instantiate the DiscordConfiguration class!");
            var autoReconnect = discordBotBaseDiscordConfiguration.AutoReconnect;
            var udpClientFactory = discordBotBaseDiscordConfiguration.UdpClientFactory;
            var discordConfiguration = new DSharpPlusDiscordConfiguration
            {
                AutoReconnect = autoReconnect,
                DateTimeFormat = discordBotBaseDiscordConfiguration.DateTimeFormat,
                GatewayCompressionLevel = discordBotBaseDiscordConfiguration.GatewayCompressionLevel,
                HttpTimeout = discordBotBaseDiscordConfiguration.HttpTimeout,
                LargeThreshold = discordBotBaseDiscordConfiguration.LargeThreshold,
                LogLevel = discordBotBaseDiscordConfiguration.LogLevel,
                MessageCacheSize = discordBotBaseDiscordConfiguration.MessageCacheSize,
                Proxy = discordBotBaseDiscordConfiguration.WebProxy,
                ReconnectIndefinitely = discordBotBaseDiscordConfiguration.ReconnectIndefinitely,
                ShardCount = discordBotBaseDiscordConfiguration.ShardCount,
                ShardId = discordBotBaseDiscordConfiguration.ShardId,
                Token = discordBotBaseDiscordConfiguration.Token,
                TokenType = discordBotBaseDiscordConfiguration.TokenType,
                UseInternalLogHandler = discordBotBaseDiscordConfiguration.UseInternalLogHandler,
                UseRelativeRatelimit = discordBotBaseDiscordConfiguration.UseRelativeRatelimit,
                WebSocketClientFactory = discordBotBaseDiscordConfiguration.WebSocketClientFactory
            };

            if (udpClientFactory != null)
                discordConfiguration.UdpClientFactory = udpClientFactory;

            _discordClient = new DiscordClient(discordConfiguration);

            var discordBotBaseConfiguration = this.BaseConfiguration ?? new BaseConfiguration();
            if (discordBotBaseConfiguration.AutoReconnect)
            {
                if (autoReconnect)
                    throw new InvalidOperationException("To use the base AutoReconnect, set the DSharpPlus AutoReconnect to false!");

                _discordClient.SocketClosed += async e => await e.Client.ConnectAsync();
            }
            #endregion

            #region CommandsNext
            var discordBotBaseCommandsNextConfiguration = this.CommandsConfiguration ?? throw new NullReferenceException("Instantiate the CommandsConfiguration class!");
            var commandsNextConfiguration = new CommandsNextConfiguration
            {
                CaseSensitive = discordBotBaseCommandsNextConfiguration.CaseSensitive,
                DefaultHelpChecks = discordBotBaseCommandsNextConfiguration.DefaultHelpChecks,
                DmHelp = discordBotBaseCommandsNextConfiguration.DirectMessageHelp,
                IgnoreExtraArguments = discordBotBaseCommandsNextConfiguration.IgnoreExtraArguments,
                EnableDefaultHelp = discordBotBaseCommandsNextConfiguration.EnableDefaultHelp,
                EnableDms = discordBotBaseCommandsNextConfiguration.EnableDirectMessages,
                EnableMentionPrefix = discordBotBaseCommandsNextConfiguration.EnableMentionPrefix,
                PrefixResolver = discordBotBaseCommandsNextConfiguration.PrefixResolver,
                StringPrefixes = discordBotBaseCommandsNextConfiguration.Prefixes,
                UseDefaultCommandHandler = discordBotBaseCommandsNextConfiguration.UseDefaultCommandHandler,
                Services = discordBotBaseCommandsNextConfiguration.Services
            };

            this._commandsNextExtension = _discordClient.UseCommandsNext(commandsNextConfiguration);

            var botClassOrAssembly = this._botClassOrAssembly;
            var objectType = botClassOrAssembly.GetType();
            this._commandsNextExtension.RegisterCommands(objectType.IsClass && objectType.Name != "RuntimeAssembly" ? objectType.Assembly : (Assembly)botClassOrAssembly);
            #endregion

            #region InteractivityExtension
            var discordBotBaseInteractivityConfiguration = this.InteractivityConfiguration ?? new InteractivityExtensionConfiguration();
            this._interactivityExtension = _discordClient.UseInteractivity(new DSharpPlusInteractivity
            {
                PaginationBehaviour = discordBotBaseInteractivityConfiguration.PaginationBehaviour,
                PaginationDeletion = discordBotBaseInteractivityConfiguration.PaginationDeletion,
                PaginationEmojis = discordBotBaseInteractivityConfiguration.PaginationEmojis,
                PollBehaviour = discordBotBaseInteractivityConfiguration.PollBehaviour,
                Timeout = discordBotBaseInteractivityConfiguration.Timeout
            });
            #endregion

            _discordClient.DebugLogger.LogMessage(LogLevel.Info, "DiscordBotBase", $"The DiscordBotBase was started successfully! By: luizfernandonb | Version: " +
                                                                                   $"{typeof(BotBase).Assembly.GetName().Version}", DateTime.Now);
        }

        /// <summary>
        /// Method where the bot connects to Discord and executes the "await Task.Delay(-1);" method to keep the console open. Attention! Only call this method if the ConnectAfterInitialize property is set to false.
        /// </summary>
        public async Task ConnectToDiscordAsync()
        {
            await _discordClient.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Get the DiscordClient.
        /// </summary>
        /// <returns>The DiscordClient.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public DiscordClient GetDiscordClient() => _discordClient ?? throw new NullReferenceException("The DiscordClient is null, initialize the bot!");

        /// <summary>
        /// Get the CommandsNext.
        /// </summary>
        /// <returns>The CommandsNext.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public CommandsNextExtension GetCommandsNext() => this._commandsNextExtension ?? throw new NullReferenceException("The CommandsNext is null, initialize the bot!");

        /// <summary>
        /// Get the Interactivity.
        /// </summary>
        /// <returns>The Interactivity.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public InteractivityExtension GetInteractivity() => this._interactivityExtension ?? throw new NullReferenceException("The Interactivity is null, initialize the bot!");
    }
}