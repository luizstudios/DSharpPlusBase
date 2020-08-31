using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Net.Udp;
using DSharpPlus.Net.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Tars.Core.Settings;
using Tars.Extensions;

namespace Tars.Core
{
    /// <summary>
    /// Class where the bot is instantiated using Tars.
    /// </summary>
    public sealed class TarsBase
    {
        #region Public Properties
        /// <summary>
        /// Get the current settings from the base.
        /// </summary>
        public TarsBaseConfiguration BaseConfiguration { get; private set; }

        /// <summary>
        /// Get the DSharpPlus <see cref="DSharpPlus.DiscordClient"/>.
        /// </summary>
        public DiscordClient DiscordClient => _discordClient ?? throw new NullReferenceException("The DiscordClient can't be null! Call the DiscordClientSetup!");
        internal static DiscordClient _discordClient;

        /// <summary>
        /// Get the DSharpPlus <see cref="CommandsNextExtension"/>.
        /// </summary>
        public CommandsNextExtension CommandsNext => this._commandsNext ?? throw new NullReferenceException("The CommandsNext can't be null! Call the CommandsNextSetup!");
        private CommandsNextExtension _commandsNext;

        /// <summary>
        /// Get the DSharpPlus <see cref="InteractivityExtension"/>.
        /// </summary>
        public InteractivityExtension Interactivity => this._interactivity ?? throw new NullReferenceException("The Interactivity can't be null! Call the InteractivitySetup!");
        private InteractivityExtension _interactivity;

        /// <summary>
        /// Get <see cref="TarsBase"/> version.
        /// </summary>
        public string Version
        {
            get
            {
                var assemblyVersion = typeof(TarsBase).Assembly.GetName().Version;
                return $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
            }
        }
        #endregion

        private readonly DateTimeFormatInfo _dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
        private object _botClassOrAssembly;
        private bool _disposed;

        internal static DiscordConfiguration _discordConfiguration;
        internal static string _logTimestampFormat;

        /// <summary>
        /// Constructor of the Tars class.
        /// </summary>
        /// <param name="botClassOrAssembly">Use the keyword <see langword="this"/> or call the <see cref="Assembly.GetEntryAssembly()"/> method.</param>
        public TarsBase(object botClassOrAssembly)
        {
            if (_discordClient != null)
                throw new InvalidOperationException("A bot instance has already been instantiated!");

            this._botClassOrAssembly = botClassOrAssembly;
        }

        #region BaseSetup
        /// <summary>
        /// Method for configuring the base settings.
        /// </summary>
        /// <param name="baseConfiguration">Class of Tars configurations.</param>
        public void BaseSetup(TarsBaseConfiguration baseConfiguration) => this.BaseConfiguration = baseConfiguration;

        /// <summary>
        /// Method for configuring <see cref="Settings.TarsBaseConfiguration"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="autoReconnect">Set whether the bot will use Tars's AutoReconnect. Attention! DSharpPlus AutoReconnect must be set to false in <see cref="DiscordConfiguration"/> for Tars's AutoReconnect to take effect.</param>
        public void BaseSetup(bool autoReconnect = true) => this.BaseConfiguration = new TarsBaseConfiguration
        {
            AutoReconnect = autoReconnect
        };
        #endregion

        #region DiscordClientSetup
        /// <summary>
        /// Method for configuring the base settings in relation to the DSharpPlus <see cref="DSharpPlus.DiscordClient"/>.
        /// </summary>
        /// <param name="discordConfiguration"><see cref="DiscordConfiguration"/> class for configuring <see cref="DSharpPlus.DiscordClient"/> settings.</param>
        public void DiscordClientSetup(DiscordConfiguration discordConfiguration)
        {
            _discordConfiguration = discordConfiguration;

            _discordClient = new DiscordClient(_discordConfiguration);

            //_discordConfiguration.AutoReconnect = !(this.BaseConfiguration ?? new BaseConfiguration()).AutoReconnect;
            //_discordConfiguration.DateTimeFormat = _dateTimeFormat = $"{this._dateTimeFormatInfo.ShortDatePattern} {this._dateTimeFormatInfo.ShortTimePattern}";
            //_discordConfiguration.HttpTimeout = TimeSpan.FromSeconds(10);
            //_discordConfiguration.LargeThreshold = 1000;
            //_discordConfiguration.UseInternalLogHandler = true;
            //_discordConfiguration.LogLevel = Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info;
        }

        /// <summary>
        /// Method for configuring <see cref="DSharpPlus.DiscordClient"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="token">Sets the token used to identify the client.</param>
        /// <param name="tokenType">Sets the type of the token used to identify the client. Defaults to <see cref="TokenType.Bot"/>.</param>
        /// <param name="minimumLogLevel">Sets the maximum logging level for messages. If left as <see langword="null"/>, and the <paramref name="logLevelDebugOnDebugging"/> property as <see langword="true"/>, the bot will use <see cref="LogLevel.Debug"/> when debugging Visual Studio and will <see cref="LogLevel.Information"/> when it starts without debugging.</param>
        /// <param name="logLevelDebugOnDebugging">Set if the bot will start using <see cref="LogLevel.Debug"/> when debugging Visual Studio, if <see langword="false"/>, the bot will always start at <see cref="LogLevel.Info"/></param>
        /// <param name="useRelativeRateLimit">Sets whether to rely on Discord for NTP (Network Time Protocol) synchronization with the "X-Ratelimit-Reset-After" header. If the system clock is unsynced, setting this to true will ensure ratelimits are synced with Discord and reduce the risk of hitting one. This should only be set to <see langword="false"/> if the system clock is synced with NTP. Defaults to <see langword="true"/>.</param>
        /// <param name="logTimestampFormat">Allows you to overwrite the time format used by the internal debug logger. The default is the format of your PC's date.</param>
        /// <param name="largeThreshold">Sets the member count threshold at which guilds are considered large. Defaults to 1000.></param>
        /// <param name="autoReconnect">Sets whether to automatically reconnect in case a connection is lost. Defaults to <see langword="false"/>.</param>
        /// <param name="shardId">Sets the ID of the shard to connect to. If not sharding, or sharding automatically, this value should be left with the default value of 0.</param>
        /// <param name="shardCount">Sets the total number of shards the bot is on. If not sharding, this value should be left with a default value of 1. If sharding automatically, this value will indicate how many shards to boot. If left default for automatic sharding, the client will determine the shard count automatically.</param>
        /// <param name="gatewayCompressionLevel">Sets the level of compression for WebSocket traffic. Disabling this option will increase the amount of traffic sent via WebSocket. Setting <see cref="GatewayCompressionLevel.Payload"/> will enable compression for READY and GUILD_CREATE payloads. Setting <see cref="GatewayCompressionLevel.Stream"/> will enable compression for the entire WebSocket stream, drastically reducing amount of traffic. Defaults to <see cref="GatewayCompressionLevel.Stream"/>.</param>
        /// <param name="messageCacheSize">Sets the size of the global message cache. Setting this to 0 will disable message caching entirely. Defaults to 1024.</param>
        /// <param name="webProxy">Sets the proxy to use for HTTP and WebSocket connections to Discord. Defaults to <see langword="null"/>.</param>
        /// <param name="httpTimeout">Sets the timeout for HTTP requests. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable timeouts. Defaults to 10 seconds.</param>
        /// <param name="reconnectIndefinitely">Defines that the client should attempt to reconnect indefinitely. This is typically a very bad idea to set to <see langword="true"/>, as it will swallow all connection errors. Defaults to <see langword="false"/>.</param>
        /// <param name="discordIntents">Sets the gateway intents for this client. If set, the client will only receive events that they specify with intents. Defaults to <see langword="null"/>.</param>
        /// <param name="webSocketClientFactory">Sets the factory method used to create instances of WebSocket clients. Use <see cref="WebSocketClient.CreateNew(IWebProxy)"/> and equivalents on other implementations to switch out client implementations. Defaults to <see cref="WebSocketClient.CreateNew(IWebProxy)"/></param>
        /// <param name="udpClientFactory">Sets the factory method used to create instances of UDP clients. Use <see cref="DspUdpClient.CreateNew"/> and equivalents on other implementations to switch out client implementations. Defaults to <see cref="DspUdpClient.CreateNew"/>.</param>
        /// <param name="loggerFactory">Sets the logger implementation to use. To create your own logger, implement the <see cref="ILoggerFactory"/> instance. Defaults to built-in implementation.</param>
        public void DiscordClientSetup(string token, TokenType tokenType = TokenType.Bot, LogLevel? minimumLogLevel = null, bool logLevelDebugOnDebugging = true,
                                       bool useRelativeRateLimit = true, string logTimestampFormat = null, int largeThreshold = 1000, bool autoReconnect = false,
                                       int shardId = 0, int shardCount = 1, GatewayCompressionLevel gatewayCompressionLevel = GatewayCompressionLevel.Stream,
                                       int messageCacheSize = 1024, IWebProxy webProxy = null, TimeSpan? httpTimeout = null, bool reconnectIndefinitely = false,
                                       DiscordIntents? discordIntents = null, WebSocketClientFactoryDelegate webSocketClientFactory = null,
                                       UdpClientFactoryDelegate udpClientFactory = null, ILoggerFactory loggerFactory = null)

        {
            _discordConfiguration = new DiscordConfiguration();

            if (udpClientFactory != null)
                _discordConfiguration.UdpClientFactory = udpClientFactory;

            _discordConfiguration.Token = token;
            _discordConfiguration.TokenType = tokenType;

#if DEBUG
            _discordConfiguration.MinimumLogLevel = logLevelDebugOnDebugging ? LogLevel.Debug : minimumLogLevel ?? LogLevel.Information;
#else       // (Debugger.IsAttached ? LogLevel.Debug : LogLevel.Information) : (minimumLogLevel ?? LogLevel.Information);
            _discordConfiguration.MinimumLogLevel = LogLevel.Information;
#endif
            _discordConfiguration.UseRelativeRatelimit = useRelativeRateLimit;
            _discordConfiguration.LogTimestampFormat = _logTimestampFormat = string.IsNullOrWhiteSpace(logTimestampFormat) ? $"{this._dateTimeFormatInfo.ShortDatePattern} " +
                                                                                                                             $"{this._dateTimeFormatInfo.ShortTimePattern}" :
                                                                                                                             logTimestampFormat;
            _discordConfiguration.LargeThreshold = largeThreshold;

            _discordConfiguration.AutoReconnect = autoReconnect;
            if (!autoReconnect)
                this.BaseConfiguration = new TarsBaseConfiguration();

            _discordConfiguration.ShardId = shardId;
            _discordConfiguration.ShardCount = shardCount;
            _discordConfiguration.GatewayCompressionLevel = gatewayCompressionLevel;
            _discordConfiguration.MessageCacheSize = messageCacheSize;
            _discordConfiguration.Proxy = webProxy;
            _discordConfiguration.HttpTimeout = httpTimeout ?? TimeSpan.FromSeconds(10);
            _discordConfiguration.ReconnectIndefinitely = reconnectIndefinitely;
            _discordConfiguration.Intents = discordIntents;
            _discordConfiguration.WebSocketClientFactory = webSocketClientFactory ?? WebSocketClient.CreateNew;
            _discordConfiguration.LoggerFactory = loggerFactory;

            _discordClient = new DiscordClient(_discordConfiguration);
        }
        #endregion

        #region CommandsNextSetup
        /// <summary>
        /// Method for configuring the base settings in relation to the DSharpPlus <see cref="CommandsNextConfiguration"/>.
        /// </summary>
        /// <param name="commandsNextConfiguration"><see cref="CommandsNextConfiguration"/> class for configuring <see cref="CommandsNextExtension"/> settings.</param>
        public void CommandsNextSetup(CommandsNextConfiguration commandsNextConfiguration)
        {
            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!")).UseCommandsNext(commandsNextConfiguration);

            this.RegisterCommands();
        }

        /// <summary>
        /// Method for configuring <see cref="CommandsNextConfiguration"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="prefixes">Sets the string prefixes used for commands.</param>
        /// <param name="prefixResolver">Sets the custom prefix resolver used for commands. Defaults to none (disabled).</param>
        /// <param name="enableMentionPrefix">Sets whether to allow mentioning the bot to be used as command prefix. Defaults to <see langword="true"/>.</param>
        /// <param name="caseSensitive">Sets whether strings should be matched in a case-sensitive manner. This switch affects the behaviour of default prefix resolver, command searching, and argument conversion. Defaults to <see langword="false"/>.</param>
        /// <param name="enableDefaultHelp">Sets whether to enable default help command. Disabling this will allow you to make your own help command. Modifying default help can be achieved via custom help formatters (see <see cref="BaseHelpFormatter"/> and <see cref="CommandsNextExtension.SetHelpFormatter{T}()"/> for more details). It is recommended to use help formatter instead of disabling help. Defaults to <see langword="true"/>.</param>
        /// <param name="directMessageHelp">Controls whether the default help will be sent via Direct Message or not. Enabling this will make the bot respond with help via direct messages. Defaults to <see langword="true"/>.</param>
        /// <param name="defaultHelpChecks">Sets the default pre-execution checks for the built-in help command. Only applicable if default help is enabled. Defaults to <see langword="null"/>.</param>
        /// <param name="directMessageCommands">Sets whether commands sent via direct messages should be processed. Defaults to <see langword="true"/>.</param>
        /// <param name="services">Sets the service provider for this <see cref="CommandsNextExtension"/> instance. Objects in this provider are used when instantiating command modules. This allows passing data around without resorting to static members. Defaults to <see langword="null"/>.</param>
        /// <param name="ignoreExtraArguments">Sets whether any extra arguments passed to commands should be ignored or not. If this is set to <see langword="false"/>, extra arguments will throw, otherwise they will be ignored. Defaults to <see langword="false"/>.</param>
        /// <param name="useDefaultCommandHandler">Sets whether to automatically enable handling commands. If this is set to <see langword="false"/>, you will need to manually handle each incoming message and pass it to <see cref="CommandsNextExtension"/>. Defaults to <see langword="true"/>.</param>
        public void CommandsNextSetup(IEnumerable<string> prefixes, PrefixResolverDelegate prefixResolver = null, bool enableMentionPrefix = true,
                                      bool caseSensitive = false, bool enableDefaultHelp = true, bool directMessageHelp = true,
                                      IEnumerable<CheckBaseAttribute> defaultHelpChecks = null, bool directMessageCommands = false,
                                      IServiceCollection services = null, bool ignoreExtraArguments = false, bool useDefaultCommandHandler = true)
        {
            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!")).UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = prefixes,
                PrefixResolver = prefixResolver,
                EnableMentionPrefix = enableMentionPrefix,
                CaseSensitive = caseSensitive,
                EnableDefaultHelp = enableDefaultHelp,
                DmHelp = directMessageHelp,
                DefaultHelpChecks = defaultHelpChecks,
                EnableDms = directMessageCommands,
                Services = services?.AddSingleton(this).BuildServiceProvider(true) ?? new ServiceCollection().AddSingleton(this).BuildServiceProvider(true),
                IgnoreExtraArguments = ignoreExtraArguments,
                UseDefaultCommandHandler = useDefaultCommandHandler
            });

            this.RegisterCommands();
        }

        private void RegisterCommands()
        {
            object botClassOrAssembly = this._botClassOrAssembly;
            Type objectType = botClassOrAssembly.GetType();
            _commandsNext.RegisterCommands(objectType.IsClass && objectType.Name != "RuntimeAssembly" ? objectType.Assembly : (Assembly)botClassOrAssembly);
        }
        #endregion

        #region InteractivitySetup
        /// <summary>
        /// Method for configuring the base settings in relation to the DSharpPlus <see cref="InteractivityExtension"/>.
        /// </summary>
        /// <param name="interactivityConfiguration"></param>
        public void InteractivitySetup(InteractivityConfiguration interactivityConfiguration)
            => this._interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!")).UseInteractivity(interactivityConfiguration);

        /// <summary>
        /// Method for configuring <see cref="InteractivityExtension"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="timeout">Sets the default interactivity action timeout. Defaults to 5 minutes.</param>
        /// <param name="pollBehaviour">What to do after the poll ends. Defaults to <see cref="PollBehaviour.DeleteEmojis"/>.</param>
        /// <param name="paginationEmojis">Emojis to use for pagination. Defaults to <see langword="null"/>.</param>
        /// <param name="paginationBehaviour">How to handle pagination. Defaults to <see cref="PaginationBehaviour.WrapAround"/>.</param>
        /// <param name="paginationDeletion">How to handle pagination deletion. Defaults to <see cref="PaginationDeletion.DeleteEmojis"/>.</param>
        public void InteractivitySetup(TimeSpan? timeout = null, PollBehaviour pollBehaviour = PollBehaviour.DeleteEmojis,
                                       PaginationEmojis paginationEmojis = null, PaginationBehaviour paginationBehaviour = PaginationBehaviour.WrapAround,
                                       PaginationDeletion paginationDeletion = PaginationDeletion.DeleteEmojis)
            => this._interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!")).UseInteractivity(new InteractivityConfiguration
            {
                Timeout = timeout ?? TimeSpan.FromMinutes(5),
                PollBehaviour = pollBehaviour,
                PaginationEmojis = paginationEmojis ?? new PaginationEmojis(),
                PaginationBehaviour = paginationBehaviour,
                PaginationDeletion = paginationDeletion
            });
        #endregion

        #region Base methods
        /// <summary>
        /// Dispose Tars and DSharpPlus.
        /// </summary>
        public void Dispose()
        {
            if (this._disposed)
                throw new InvalidOperationException("The Tars is already disposed!");

            _discordClient.Dispose();
            _discordConfiguration = null;
            _commandsNext = null;
            _interactivity = null;
            _discordClient = null;
            _botClassOrAssembly = null;
            _logTimestampFormat = string.Empty;

            this._disposed = true;
        }
        #endregion

        /// <summary>
        /// Method to start the base and connect the bot to Discord.
        /// </summary>
        /// <param name="discordActivity">Represents a game that a user is playing. When the bot connects to the gateway, it will go online with the presence passed in that parameter, if it is null, it will only go online. Defaults to <see langword="null"/>.</param>
        /// <param name="userStatus">Represents user status. Defaults to <see cref="UserStatus.Online"/>.</param>
        /// <param name="idleSince">Since when is the client performing the specified activity. Defaults to <see langword="null"/>.</param>
        public async Task StartAsync(DiscordActivity discordActivity = null, UserStatus userStatus = UserStatus.Online, DateTime? idleSince = null)
        {
            if (this._interactivity == null)
                this.InteractivitySetup();

            if (this.BaseConfiguration?.AutoReconnect == true)
                _discordClient.SocketClosed += async e => await e.Client.ConnectAsync(discordActivity, userStatus, idleSince);

            _discordClient.LogMessage($"The Tars was started successfully! Version: {typeof(TarsBase).Assembly.GetName().Version}");
            _discordClient.LogMessage("Connecting to Discord...");

            await _discordClient.ConnectAsync(discordActivity, userStatus, idleSince);

            _discordClient.LogMessage("Connection successful!");

            await Task.Delay(-1);
        }
    }
}