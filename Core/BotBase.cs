using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using DSharpPlus.Net.Udp;
using DSharpPlus.Net.WebSocket;
using DSharpPlusBase.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DSharpPlusBase.Core
{
    /// <summary>
    /// Class where the bot is instantiated using DSharpPlusBase.
    /// </summary>
    public sealed class BotBase
    {
        /// <summary>
        /// Class where the DSharpPlusBase is configured.
        /// </summary>
        public BotBaseConfiguration BotBaseConfiguration { get; set; }

        /// <summary>
        /// Class where the Discord settings are configured.
        /// </summary>
        public BotBaseDiscordConfiguration BotBaseDiscordConfiguration { get; set; }

        /// <summary>
        /// Class where the CommandsNext settings are configured.
        /// </summary>
        public BotBaseCommandsNextConfiguration BotBaseCommandsNextConfiguration { get; set; }

        /// <summary>
        /// Class where the Interactivity settings are configured.
        /// </summary>
        public BotBaseInteractivityConfiguration BotBaseInteractivityConfiguration { get; set; }

        private DiscordClient _discordClient;
        private CommandsNextExtension _commandsNextExtension;
        private InteractivityExtension _interactivityExtension;
        private readonly LavalinkExtension _lavalinkExtension;

        /// <summary>
        /// Constructor of DSharpPlusBase.
        /// </summary>
        public BotBase() { }

        /// <summary>
        /// Method where the bot settings previously defined are applied.
        /// </summary>
        /// <param name="botClassOrAssembly">Bot class (Use the "this" keyword if the class where you are instantiating the bot is not static) or the assembly (Use Assembly.GetEntryAssembly()) to register commands in CommandsNext.</param>
        /// <returns>Nothing.</returns>
        public async Task InitializeAsync(object botClassOrAssembly)
        {
            #region DiscordClient
            var autoReconnect = this.BotBaseDiscordConfiguration.AutoReconnect;
            var udpClientFactory = this.BotBaseDiscordConfiguration.UdpClientFactory;
            var webSocketClientFactory = this.BotBaseDiscordConfiguration.WebSocketClientFactory;
            var discordConfiguration = new DiscordConfiguration
            {
                AutoReconnect = autoReconnect,
                DateTimeFormat = this.BotBaseDiscordConfiguration.DateTimeFormat,
                GatewayCompressionLevel = this.BotBaseDiscordConfiguration.GatewayCompressionLevel,
                HttpTimeout = this.BotBaseDiscordConfiguration.HttpTimeout,
                LargeThreshold = this.BotBaseDiscordConfiguration.LargeThreshold,
                LogLevel = this.BotBaseDiscordConfiguration.LogLevel,
                MessageCacheSize = this.BotBaseDiscordConfiguration.MessageCacheSize,
                Proxy = this.BotBaseDiscordConfiguration.WebProxy,
                ReconnectIndefinitely = this.BotBaseDiscordConfiguration.ReconnectIndefinitely,
                ShardCount = this.BotBaseDiscordConfiguration.ShardCount,
                ShardId = this.BotBaseDiscordConfiguration.ShardId,
                Token = this.BotBaseDiscordConfiguration.Token,
                TokenType = this.BotBaseDiscordConfiguration.TokenType,
                UseInternalLogHandler = this.BotBaseDiscordConfiguration.UseInternalLogHandler,
                UseRelativeRatelimit = this.BotBaseDiscordConfiguration.UseRelativeRatelimit,
            };

            if (udpClientFactory != null)
                discordConfiguration.UdpClientFactory = udpClientFactory;
            if (webSocketClientFactory != null)
                discordConfiguration.WebSocketClientFactory = webSocketClientFactory;

            this._discordClient = new DiscordClient(discordConfiguration);

            if (this.BotBaseConfiguration == null)
                this.BotBaseConfiguration = new BotBaseConfiguration();
            if (this.BotBaseConfiguration.AutoReconnect)
            {
                if (autoReconnect)
                    throw new InvalidOperationException("To use the base AutoReconnect, set the DSharpPlus AutoReconnect to false!");

                this._discordClient.SocketClosed += async e => await e.Client.ConnectAsync();
            }
            #endregion
            
            #region CommandsNext
            var commandsNextConfiguration = new CommandsNextConfiguration
            {
                CaseSensitive = this.BotBaseCommandsNextConfiguration.CaseSensitive,
                DefaultHelpChecks = this.BotBaseCommandsNextConfiguration.DefaultHelpChecks,
                DmHelp = this.BotBaseCommandsNextConfiguration.DirectMessageHelp,
                IgnoreExtraArguments = this.BotBaseCommandsNextConfiguration.IgnoreExtraArguments,
                EnableDefaultHelp = this.BotBaseCommandsNextConfiguration.EnableDefaultHelp,
                EnableDms = this.BotBaseCommandsNextConfiguration.EnableDirectMessages,
                EnableMentionPrefix = this.BotBaseCommandsNextConfiguration.EnableMentionPrefix,
                PrefixResolver = this.BotBaseCommandsNextConfiguration.PrefixResolver,
                StringPrefixes = this.BotBaseCommandsNextConfiguration.Prefixes,
                UseDefaultCommandHandler = this.BotBaseCommandsNextConfiguration.UseDefaultCommandHandler,
            };

            var objectType = botClassOrAssembly.GetType();
            bool checkClassAndName = objectType.IsClass && objectType.Name != "RuntimeAssembly";
            var typeClass = checkClassAndName ? objectType : null;

            var services = this.BotBaseCommandsNextConfiguration.Services;
            commandsNextConfiguration.Services = services == null ? (typeClass != null ? new ServiceCollection().AddSingleton(typeClass)
                                                                                                                .BuildServiceProvider(true) :
                                                                                         new ServiceCollection().BuildServiceProvider(true)) : 
                                                                    (typeClass != null ? new ServiceCollection().AddSingleton(services)
                                                                                                                .AddSingleton(typeClass)
                                                                                                                .BuildServiceProvider(true) :
                                                                                         new ServiceCollection().AddSingleton(services)
                                                                                                                .BuildServiceProvider(true));
            this._commandsNextExtension = this._discordClient.UseCommandsNext(commandsNextConfiguration);

            this._commandsNextExtension.RegisterCommands(checkClassAndName ? objectType.Assembly : (Assembly)botClassOrAssembly);
            #endregion

            #region InteractivityExtension
            if (this.BotBaseInteractivityConfiguration == null)
                this.BotBaseInteractivityConfiguration = new BotBaseInteractivityConfiguration();
            this._interactivityExtension = this._discordClient.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = this.BotBaseInteractivityConfiguration.PaginationBehaviour,
                PaginationDeletion = this.BotBaseInteractivityConfiguration.PaginationDeletion,
                PaginationEmojis = this.BotBaseInteractivityConfiguration.PaginationEmojis,
                PollBehaviour = this.BotBaseInteractivityConfiguration.PollBehaviour,
                Timeout = this.BotBaseInteractivityConfiguration.Timeout
            });
            #endregion

            this._discordClient.DebugLogger.LogMessage(LogLevel.Info, "DSharpPlusBase", "The DSharpPlusBase was started successfully! By: luizfernandonb", DateTime.Now);

            if (this.BotBaseConfiguration.ConnectAfterInitialize)
                await this._discordClient.ConnectAsync();
        }

        /// <summary>
        /// Method where the bot connects to Discord and executes the "await Task.Delay(-1);" method to keep the console open. Attention! Only call this method if the ConnectAfterInitialize property is set to false.
        /// </summary>
        /// <returns></returns>
        public async Task ConnectToDiscordAsync()
        {
            await this._discordClient.ConnectAsync();

            if (this.BotBaseConfiguration.ConnectAfterInitialize)
                throw new InvalidOperationException("To use this method, set the ConnectAfterInitialize property of BotBaseConfiguration to false!");

            await Task.Delay(-1);
        }

        /// <summary>
        /// Get the DiscordClient.
        /// </summary>
        /// <returns>The DiscordClient.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public DiscordClient GetDiscordClient() => this._discordClient ?? throw new NullReferenceException("The DiscordClient is null, initialize the bot!");

        /// <summary>
        /// Get the CommandsNext.
        /// </summary>
        /// <returns>The DiscordClient.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public CommandsNextExtension GetCommandsNext() => this._commandsNextExtension ?? throw new NullReferenceException("The CommandsNext is null, initialize the bot!");

        /// <summary>
        /// Get the Interactivity.
        /// </summary>
        /// <returns>The DiscordClient.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public InteractivityExtension GetInteractivity() => this._interactivityExtension ?? throw new NullReferenceException("The Interactivity is null, initialize the bot!");

        /// <summary>
        /// Get the Lavalink.
        /// </summary>
        /// <returns>The DiscordClient.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public LavalinkExtension GetLavalink() => this._lavalinkExtension ?? throw new NotImplementedException("The Lavalink has not yet been implemented!");
    }
}