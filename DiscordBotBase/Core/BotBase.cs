using DiscordBotBase.Core.Settings;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Globalization;
using System.Diagnostics;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Entities;
using System.Net;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.Net.Udp;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Attributes;
using DiscordBotBase.Extensions;
using DiscordBotBase.Classes;
using System.Threading;
using TimerClass = System.Timers.Timer;

namespace DiscordBotBase.Core
{
    /// <summary>
    /// Class where the bot is instantiated using DiscordBotBase.
    /// </summary>
    public sealed class BotBase
    {
        /// <summary>
        /// Get the current settings from the base.
        /// </summary>
        public BaseConfiguration BaseConfiguration { get; private set; }

        /// <summary>
        /// Get the DSharpPlus <see cref="DSharpPlus.DiscordClient"/>.
        /// </summary>
        public DiscordClient DiscordClient => _discordClient ?? throw new NullReferenceException("The DiscordClient can't be null! Call the DiscordClientSetup!");
        internal static DiscordClient _discordClient;

        /// <summary>
        /// Get the DSharpPlus <see cref="CommandsNextExtension"/>.
        /// </summary>
        public CommandsNextExtension CommandsNext 
            => this._commandsNext ?? throw new NullReferenceException("The CommandsNext can't be null! Call the CommandsNextSetup!");
        private CommandsNextExtension _commandsNext;

        /// <summary>
        /// Get the DSharpPlus <see cref="InteractivityExtension"/>.
        /// </summary>
        public InteractivityExtension Interactivity 
            => this._interactivity ?? throw new NullReferenceException("The Interactivity can't be null! Call the InteractivitySetup!");
        private InteractivityExtension _interactivity;

        /// <summary>
        /// Get a list of all scheduled events.
        /// </summary>
        public IReadOnlyList<Event> ScheduledEvents => _scheduledEvents;
        private static List<Event> _scheduledEvents;

        private readonly DateTimeFormatInfo _dateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
        private object _instance;
        private readonly object _botClassOrAssembly;
        private const string discordBotBaseApplication = "DiscordBotBase";

        internal static DiscordConfiguration _discordConfiguration;
        internal static string _dateTimeFormat;

        /// <summary>
        /// Constructor of the DiscordBotBase class.
        /// </summary>
        /// <param name="botClassOrAssembly">Use the keyword <see langword="this"/> or call the <see cref="Assembly.GetEntryAssembly()"/> method.</param>
        public BotBase(object botClassOrAssembly) => this._botClassOrAssembly = botClassOrAssembly;

        #region BaseSetup
        /// <summary>
        /// Method for configuring the base settings.
        /// </summary>
        /// <param name="baseConfiguration">Class of DiscordBotBase configurations.</param>
        public void BaseSetup(BaseConfiguration baseConfiguration) => this.BaseConfiguration = baseConfiguration;

        /// <summary>
        /// Method for configuring <see cref="Settings.BaseConfiguration"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="autoReconnect">Set whether the bot will use DiscordBotBase's AutoReconnect. Attention! DSharpPlus AutoReconnect must be set to false. The default is true, so the DSharpPlus <see cref="DiscordConfiguration"/> comes standard with AutoReconnect as false.</param>
        public void BaseSetup(bool autoReconnect = true) => this.BaseConfiguration = new BaseConfiguration
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
            _discordConfiguration.AutoReconnect = !(this.BaseConfiguration ?? new BaseConfiguration()).AutoReconnect;
            
            _discordConfiguration.DateTimeFormat = _dateTimeFormat = $"{this._dateTimeFormatInfo.ShortDatePattern} {this._dateTimeFormatInfo.ShortTimePattern}";
            _discordConfiguration.HttpTimeout = TimeSpan.FromSeconds(10);
            _discordConfiguration.LargeThreshold = 1000;
            _discordConfiguration.UseInternalLogHandler = true;
            _discordConfiguration.LogLevel = Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info;

            _discordClient = new DiscordClient(_discordConfiguration);
        }

        /// <summary>
        /// Method for configuring <see cref="DSharpPlus.DiscordClient"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="token">Sets the token used to identify the client.</param>
        /// <param name="tokenType">Sets the type of the token used to identify the client. Defaults to <see cref="TokenType.Bot"/>.</param>
        /// <param name="logLevel">Sets the maximum logging level for messages. If left as <see langword="null"/>, and the <paramref name="logLevelDebugOnDebugging"/> property as <see langword="true"/>, the bot will use <see cref="LogLevel.Debug"/> when debugging Visual Studio and will <see cref="LogLevel.Info"/> when it starts without debugging.</param>
        /// <param name="logLevelDebugOnDebugging">Set if the bot will start using <see cref="LogLevel.Debug"/> when debugging Visual Studio, if <see langword="false"/>, the bot will always start at <see cref="LogLevel.Info"/></param>
        /// <param name="useInternalLogHandler">Set if the bot will use the C# console as a log. Defaults to <see langword="true"/>.</param>
        /// <param name="useRelativeRateLimit">Sets whether to rely on Discord for NTP (Network Time Protocol) synchronization with the "X-Ratelimit-Reset-After" header. If the system clock is unsynced, setting this to true will ensure ratelimits are synced with Discord and reduce the risk of hitting one. This should only be set to <see langword="false"/> if the system clock is synced with NTP. Defaults to <see langword="true"/>.</param>
        /// <param name="dateTimeFormat">Allows you to overwrite the time format used by the internal debug logger. Only applicable when <paramref name="useInternalLogHandler"/> is set to true. The default is the format of your PC's date.</param>
        /// <param name="largeThreshold">Sets the member count threshold at which guilds are considered large. Defaults to 1000.></param>
        /// <param name="autoReconnect">Sets whether to automatically reconnect in case a connection is lost. Defaults to <see langword="false"/>.</param>
        /// <param name="shardId">Sets the ID of the shard to connect to. If not sharding, or sharding automatically, this value should be left with the default value of 0.</param>
        /// <param name="shardCount">Sets the total number of shards the bot is on. If not sharding, this value should be left with a default value of 1. If sharding automatically, this value will indicate how many shards to boot. If left default for automatic sharding, the client will determine the shard count automatically.</param>
        /// <param name="gatewayCompressionLevel">Sets the level of compression for WebSocket traffic. Disabling this option will increase the amount of traffic sent via WebSocket. Setting <see cref="GatewayCompressionLevel.Payload"/> will enable compression for READY and GUILD_CREATE payloads. Setting <see cref="GatewayCompressionLevel.Stream"/> will enable compression for the entire WebSocket stream, drastically reducing amount of traffic. Defaults to <see cref="GatewayCompressionLevel.Stream"/>.</param>
        /// <param name="messageCacheSize">Sets the size of the global message cache. Setting this to 0 will disable message caching entirely. Defaults to 1024.</param>
        /// <param name="webProxy">Sets the proxy to use for HTTP and WebSocket connections to Discord. Defaults to <see langword="null"/>.</param>
        /// <param name="httpTimeout">Sets the timeout for HTTP requests. Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable timeouts. Defaults to 10 seconds.</param>
        /// <param name="reconnectIndefinitely">Defines that the client should attempt to reconnect indefinitely. This is typically a very bad idea to set to <see langword="true"/>, as it will swallow all connection errors. Defaults to <see langword="false"/>.</param>
        /// <param name="webSocketClientFactory">Sets the factory method used to create instances of WebSocket clients. Use <see cref="WebSocketClient.CreateNew(IWebProxy)"/> and equivalents on other implementations to switch out client implementations. Defaults to <see cref="WebSocketClient.CreateNew(IWebProxy)"/></param>
        /// <param name="udpClientFactory">Sets the factory method used to create instances of UDP clients. Use <see cref="DspUdpClient.CreateNew"/> and equivalents on other implementations to switch out client implementations. Defaults to <see cref="DspUdpClient.CreateNew"/>.</param>
        public void DiscordClientSetup(string token, TokenType tokenType = TokenType.Bot, LogLevel? logLevel = null, bool logLevelDebugOnDebugging = true,
                                       bool useInternalLogHandler = true, bool useRelativeRateLimit = true, string dateTimeFormat = null, 
                                       int largeThreshold = 1000, bool autoReconnect = false, int shardId = 0, int shardCount = 1, 
                                       GatewayCompressionLevel gatewayCompressionLevel = GatewayCompressionLevel.Stream, int messageCacheSize = 1024, 
                                       IWebProxy webProxy = null, TimeSpan? httpTimeout = null, bool reconnectIndefinitely = false,
                                       WebSocketClientFactoryDelegate webSocketClientFactory = null, UdpClientFactoryDelegate udpClientFactory = null)

        {
            _discordConfiguration = new DiscordConfiguration();

            if (udpClientFactory != null)
                _discordConfiguration.UdpClientFactory = udpClientFactory;

            _discordConfiguration.Token = token;
            _discordConfiguration.TokenType = tokenType;
            _discordConfiguration.LogLevel = logLevelDebugOnDebugging ? (Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info) : (logLevel ?? LogLevel.Info);
            _discordConfiguration.UseInternalLogHandler = useInternalLogHandler;
            _discordConfiguration.UseRelativeRatelimit = useRelativeRateLimit;

            _discordConfiguration.DateTimeFormat = string.IsNullOrWhiteSpace(dateTimeFormat) ? $"{this._dateTimeFormatInfo.ShortDatePattern} " +
                                                                                               $"{this._dateTimeFormatInfo.ShortTimePattern}" : string.Empty;
            _discordConfiguration.LargeThreshold = largeThreshold;
            _discordConfiguration.AutoReconnect = autoReconnect;
            _discordConfiguration.ShardId = shardId;
            _discordConfiguration.ShardCount = shardCount;
            _discordConfiguration.GatewayCompressionLevel = gatewayCompressionLevel;
            _discordConfiguration.MessageCacheSize = messageCacheSize;
            _discordConfiguration.Proxy = webProxy;
            _discordConfiguration.HttpTimeout = httpTimeout ?? TimeSpan.FromSeconds(10);
            _discordConfiguration.ReconnectIndefinitely = reconnectIndefinitely;
            _discordConfiguration.WebSocketClientFactory = webSocketClientFactory ?? WebSocketClient.CreateNew;

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
            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                 .UseCommandsNext(commandsNextConfiguration);

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
            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                 .UseCommandsNext(new CommandsNextConfiguration
                                 {
                                     StringPrefixes = prefixes,
                                     PrefixResolver = prefixResolver,
                                     EnableMentionPrefix = enableMentionPrefix,
                                     CaseSensitive = caseSensitive,
                                     EnableDefaultHelp = enableDefaultHelp,
                                     DmHelp = directMessageHelp,
                                     DefaultHelpChecks = defaultHelpChecks,
                                     EnableDms = directMessageCommands,
                                     Services = services?.BuildServiceProvider(true) ?? new ServiceCollection().BuildServiceProvider(true),
                                     IgnoreExtraArguments = ignoreExtraArguments,
                                     UseDefaultCommandHandler = useDefaultCommandHandler
                                 });

            this.RegisterCommands();
        }
        
        private void RegisterCommands()
        {
            object botClassOrAssembly = this._botClassOrAssembly;
            Type objectType = botClassOrAssembly.GetType();
            this._commandsNext.RegisterCommands(objectType.IsClass && objectType.Name != "RuntimeAssembly" ? objectType.Assembly : (Assembly)botClassOrAssembly);
        }
        #endregion

        #region InteractivitySetup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactivityConfiguration"></param>
        public void InteractivitySetup(InteractivityConfiguration interactivityConfiguration)
            => this._interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                     .UseInteractivity(interactivityConfiguration);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="pollBehaviour"></param>
        /// <param name="paginationEmojis"></param>
        /// <param name="paginationBehaviour"></param>
        /// <param name="paginationDeletion"></param>
        public void InteractivitySetup(TimeSpan? timeout = null, PollBehaviour pollBehaviour = PollBehaviour.DeleteEmojis,
                                       PaginationEmojis paginationEmojis = null, PaginationBehaviour paginationBehaviour = PaginationBehaviour.WrapAround,
                                       PaginationDeletion paginationDeletion = PaginationDeletion.DeleteEmojis) 
            => this._interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                     .UseInteractivity(new InteractivityConfiguration
                                     {
                                         Timeout = timeout ?? TimeSpan.FromMinutes(5),
                                         PollBehaviour = pollBehaviour,
                                         PaginationEmojis = paginationEmojis ?? new PaginationEmojis(),
                                         PaginationBehaviour = paginationBehaviour,
                                         PaginationDeletion = paginationDeletion
                                     });
        #endregion

        #region ScheduledEventsSetup and static methods of scheduled events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        public void ScheduledEventsSetup(params Event[] events)
        {
            _scheduledEvents = new List<Event>();

            var iEvent = 0;

            foreach (Event scheduledEvent in events)
            {
                if (!_scheduledEvents.Contains(scheduledEvent))
                    _scheduledEvents.Add(scheduledEvent);

                ++iEvent;
            }
            
            _discordClient.LogMessage(discordBotBaseApplication, $"{(iEvent > 1 ? $"A total of {iEvent} scheduled events were recorded." : $"A total of {iEvent} scheduled event were recorded.")}", LogLevel.Debug);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduledEvents"></param>
        public static void AddScheduledEvents(params Event[] scheduledEvents)
        {
            if (scheduledEvents.Any(e => e == null))
                throw new NullReferenceException("The scheduled event can't be null!");
            
            if (_scheduledEvents == null)
                _scheduledEvents = new List<Event>();

            _scheduledEvents.AddRange(scheduledEvents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduledEvents"></param>
        public static void RemoveScheduledEvents(params Event[] scheduledEvents)
        {
            if (scheduledEvents.Any(e => e == null))
                throw new NullReferenceException("The scheduled event can't be null!");

            if (_scheduledEvents == null)
                throw new NullReferenceException("Add an event before removing!");

            foreach (Event scheduledEvent in scheduledEvents)
            {
                _scheduledEvents.Remove(_scheduledEvents.FirstOrDefault(e => e == scheduledEvent) ?? 
                                        throw new InvalidOperationException("An event was not found in the list of events scheduled to be removed."));
            }
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="discordActivity"></param>
        /// <param name="userStatus"></param>
        /// <param name="idleSince"></param>
        /// <returns></returns>
        public async Task StartAsync(DiscordActivity discordActivity = null, UserStatus? userStatus = null, DateTime? idleSince = null)
        {
            if (this._instance != null)
                throw new InvalidOperationException("An instance of a bot is already running!");

            this._instance = new object();

            if (this._interactivity == null)
                this.InteractivitySetup();

            _discordClient.LogMessage(discordBotBaseApplication, $"The DiscordBotBase was started successfully! By: luizfernandonb | Version: " +
                                                                 $"{typeof(BotBase).Assembly.GetName().Version}");
            _discordClient.LogMessage(discordBotBaseApplication, $"Connecting do Discord...");
            
            await _discordClient.ConnectAsync(discordActivity, userStatus, idleSince);
            
            _discordClient.LogMessage(discordBotBaseApplication, "Connection successful!");
            
            await Task.Delay(-1);
        }
    }
}