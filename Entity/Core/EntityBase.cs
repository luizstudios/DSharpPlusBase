using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink;
using DSharpPlus.Net.Udp;
using DSharpPlus.Net.WebSocket;
using Entity.Base.Core.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Entity.Base.Core
{
    /// <summary>
    /// Class where the bot is instantiated using EntityBase.
    /// </summary>
    public sealed class EntityBase
    {
        /// <summary>
        /// Class where the EntityBase is configured.
        /// </summary>
        public EntityBaseConfiguration EntityBaseConfiguration { get; set; }

        /// <summary>
        /// Class where the Discord settings are configured.
        /// </summary>
        public EntityBaseDiscordConfiguration EntityBaseDiscordConfiguration { get; set; }

        /// <summary>
        /// Class where the CommandsNext settings are configured.
        /// </summary>
        public EntityBaseCommandsNextConfiguration EntityBaseCommandsNextConfiguration { get; set; }

        /// <summary>
        /// Class where the Interactivity settings are configured.
        /// </summary>
        public EntityBaseInteractivityConfiguration EntityBaseInteractivityConfiguration { get; set; }

        internal static EntityBaseDiscordConfiguration _entityBaseDiscordConfiguration;

        internal static DiscordClient _discordClient;
        private CommandsNextExtension _commandsNextExtension;
        private InteractivityExtension _interactivityExtension;

        private readonly object _botClassOrAssembly;

        /// <summary>
        /// Constructor of EntityBase.
        /// </summary>
        /// <param name="botClassOrAssembly">Bot class (Use the "this" keyword if the class where you are instantiating the bot is not static) or the assembly (Use Assembly.GetEntryAssembly()) to register commands in CommandsNext.</param>
        public EntityBase(object botClassOrAssembly) => this._botClassOrAssembly = botClassOrAssembly;

        /// <summary>
        /// Method where the bot settings previously defined are applied.
        /// </summary>
        public void Initialize()
        { 
            #region DiscordClient
            var entityBaseDiscordConfiguration = this.EntityBaseDiscordConfiguration;
            _entityBaseDiscordConfiguration = entityBaseDiscordConfiguration ?? throw new NullReferenceException("Instantiate the EntityBaseDiscordConfiguration class!");
            var autoReconnect = entityBaseDiscordConfiguration.AutoReconnect;
            var udpClientFactory = entityBaseDiscordConfiguration.UdpClientFactory;
            var discordConfiguration = new DiscordConfiguration
            {
                AutoReconnect = autoReconnect,
                DateTimeFormat = entityBaseDiscordConfiguration.DateTimeFormat,
                GatewayCompressionLevel = entityBaseDiscordConfiguration.GatewayCompressionLevel,
                HttpTimeout = entityBaseDiscordConfiguration.HttpTimeout,
                LargeThreshold = entityBaseDiscordConfiguration.LargeThreshold,
                LogLevel = entityBaseDiscordConfiguration.LogLevel,
                MessageCacheSize = entityBaseDiscordConfiguration.MessageCacheSize,
                Proxy = entityBaseDiscordConfiguration.WebProxy,
                ReconnectIndefinitely = entityBaseDiscordConfiguration.ReconnectIndefinitely,
                ShardCount = entityBaseDiscordConfiguration.ShardCount,
                ShardId = entityBaseDiscordConfiguration.ShardId,
                Token = entityBaseDiscordConfiguration.Token,
                TokenType = entityBaseDiscordConfiguration.TokenType,
                UseInternalLogHandler = entityBaseDiscordConfiguration.UseInternalLogHandler,
                UseRelativeRatelimit = entityBaseDiscordConfiguration.UseRelativeRatelimit,
                WebSocketClientFactory = entityBaseDiscordConfiguration.WebSocketClientFactory 
            };

            if (udpClientFactory != null)
                discordConfiguration.UdpClientFactory = udpClientFactory;

            _discordClient = new DiscordClient(discordConfiguration);

            if (this.EntityBaseConfiguration == null)
                this.EntityBaseConfiguration = new EntityBaseConfiguration();
            var entityBaseConfiguration = this.EntityBaseConfiguration;
            if (entityBaseConfiguration.AutoReconnect)
            {
                if (autoReconnect)
                    throw new InvalidOperationException("To use the base AutoReconnect, set the DSharpPlus AutoReconnect to false!");

                _discordClient.SocketClosed += async e => await e.Client.ConnectAsync();
            }
            #endregion

            #region CommandsNext
            var entityBaseCommandsNextConfiguration = this.EntityBaseCommandsNextConfiguration;
            var commandsNextConfiguration = new CommandsNextConfiguration
            {
                CaseSensitive = entityBaseCommandsNextConfiguration.CaseSensitive,
                DefaultHelpChecks = entityBaseCommandsNextConfiguration.DefaultHelpChecks,
                DmHelp = entityBaseCommandsNextConfiguration.DirectMessageHelp,
                IgnoreExtraArguments = entityBaseCommandsNextConfiguration.IgnoreExtraArguments,
                EnableDefaultHelp = entityBaseCommandsNextConfiguration.EnableDefaultHelp,
                EnableDms = entityBaseCommandsNextConfiguration.EnableDirectMessages,
                EnableMentionPrefix = entityBaseCommandsNextConfiguration.EnableMentionPrefix,
                PrefixResolver = entityBaseCommandsNextConfiguration.PrefixResolver,
                StringPrefixes = entityBaseCommandsNextConfiguration.Prefixes,
                UseDefaultCommandHandler = entityBaseCommandsNextConfiguration.UseDefaultCommandHandler,
                Services = entityBaseCommandsNextConfiguration.Services
            };

            this._commandsNextExtension = _discordClient.UseCommandsNext(commandsNextConfiguration);

            var botClassOrAssembly = this._botClassOrAssembly;
            var objectType = botClassOrAssembly.GetType();
            this._commandsNextExtension.RegisterCommands(objectType.IsClass && objectType.Name != "RuntimeAssembly" ? objectType.Assembly : (Assembly)botClassOrAssembly);
            #endregion

            #region InteractivityExtension
            if (this.EntityBaseInteractivityConfiguration == null)
                this.EntityBaseInteractivityConfiguration = new EntityBaseInteractivityConfiguration();

            var entityBaseInteractivityConfiguration = this.EntityBaseInteractivityConfiguration;
            this._interactivityExtension = _discordClient.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = entityBaseInteractivityConfiguration.PaginationBehaviour,
                PaginationDeletion = entityBaseInteractivityConfiguration.PaginationDeletion,
                PaginationEmojis = entityBaseInteractivityConfiguration.PaginationEmojis,
                PollBehaviour = entityBaseInteractivityConfiguration.PollBehaviour,
                Timeout = entityBaseInteractivityConfiguration.Timeout
            });
            #endregion

            _discordClient.DebugLogger.LogMessage(LogLevel.Info, "EntityBase", $"The EntityBase was started successfully! By: luizfernandonb | Version: {typeof(EntityBase).Assembly.GetName().Version}", DateTime.Now);
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

        /// <summary>
        /// Get the Lavalink.
        /// </summary>
        /// <returns>The Lavalink.</returns>
        /// <exception cref="NotImplementedException"></exception>
        //public LavalinkExtension GetLavalink() => this._lavalinkExtension ?? throw new NotImplementedException("The Lavalink has not yet been implemented!");
    }
}