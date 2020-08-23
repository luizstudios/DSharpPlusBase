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
        /// 
        /// </summary>
        public BaseConfiguration BaseConfiguration { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DiscordClient DiscordClient
        {
            get => _discordClient ?? throw new NullReferenceException("The DiscordClient can't be null! Call the DiscordClientSetup!");
            private set { }
        }
        internal static DiscordClient _discordClient;
        
        /// <summary>
        /// 
        /// </summary>
        public CommandsNextExtension CommandsNext
        {
            get => this._commandsNext ?? throw new NullReferenceException("The CommandsNext can't be null! Call the CommandsNextSetup!");
            private set { }
        }
        private CommandsNextExtension _commandsNext;

        /// <summary>
        /// 
        /// </summary>
        public InteractivityExtension Interactivity
        {
            get => this._interactivity ?? throw new NullReferenceException("The Interactivity can't be null! Call the InteractivitySetup!");
            private set { }
        }
        private InteractivityExtension _interactivity;
        
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyList<Event> ScheduledEvents { get; private set; }

        private CommandsNextConfiguration _commandsNextConfiguration;
        private InteractivityConfiguration _interactivityConfiguration;
        private readonly object _botClassOrAssembly;

        internal static DiscordConfiguration _discordConfiguration;
        internal static string _dateTimeFormat;

        private const string discordBotBaseApplication = "DiscordBotBase";

        public BotBase(object botClassOrAssembly) => this._botClassOrAssembly = botClassOrAssembly;

        #region BaseSetup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseConfiguration"></param>
        public void BaseSetup(BaseConfiguration baseConfiguration) => this.BaseConfiguration = baseConfiguration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoReconnect"></param>
        public void BaseSetup(bool autoReconnect = true) => this.BaseConfiguration = new BaseConfiguration
        {
            AutoReconnect = autoReconnect
        };
        #endregion

        #region DiscordClientSetup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="discordConfiguration"></param>
        public void DiscordClientSetup(DiscordConfiguration discordConfiguration)
        {
            _discordConfiguration = discordConfiguration;
            _discordConfiguration.AutoReconnect = !(this.BaseConfiguration ?? new BaseConfiguration()).AutoReconnect;
            
            var dateTimeFormatPC = CultureInfo.CurrentCulture.DateTimeFormat;
            _discordConfiguration.DateTimeFormat = _dateTimeFormat = $"{dateTimeFormatPC.ShortDatePattern} {dateTimeFormatPC.ShortTimePattern}";
            _discordConfiguration.HttpTimeout = TimeSpan.FromSeconds(10);
            _discordConfiguration.LargeThreshold = 1000;
            _discordConfiguration.UseInternalLogHandler = true;
            _discordConfiguration.LogLevel = Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info;

            _discordClient = new DiscordClient(_discordConfiguration);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        /// <param name="logLevel"></param>
        /// <param name="logLevelDebugOnDebugging"></param>
        /// <param name="useInternalLogHandler"></param>
        /// <param name="useRelativeRateLimit"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="largeThreshold"></param>
        /// <param name="autoReconnect"></param>
        /// <param name="shardId"></param>
        /// <param name="shardCount"></param>
        /// <param name="gatewayCompressionLevel"></param>
        /// <param name="messageCacheSize"></param>
        /// <param name="webProxy"></param>
        /// <param name="httpTimeout"></param>
        /// <param name="reconnectIndefinitely"></param>
        /// <param name="webSocketClientFactory"></param>
        /// <param name="udpClientFactory"></param>
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

            var dateTimeFormatPC = CultureInfo.CurrentCulture.DateTimeFormat;
            _discordConfiguration.DateTimeFormat = string.IsNullOrWhiteSpace(dateTimeFormat) ? $"{dateTimeFormatPC.ShortDatePattern} " +
                                                                                               $"{dateTimeFormatPC.ShortTimePattern}" : string.Empty;
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
        /// 
        /// </summary>
        /// <param name="commandsNextConfiguration"></param>
        public void CommandsNextSetup(CommandsNextConfiguration commandsNextConfiguration)
        {
            this._commandsNextConfiguration = commandsNextConfiguration;

            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                 .UseCommandsNext(this._commandsNextConfiguration);

            this.RegisterCommands();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixes"></param>
        /// <param name="prefixResolver"></param>
        /// <param name="enableMentionPrefix"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="enableDefaultHelp"></param>
        /// <param name="directMessageHelp"></param>
        /// <param name="defaultHelpChecks"></param>
        /// <param name="directMessageCommands"></param>
        /// <param name="services"></param>
        /// <param name="ignoreExtraArguments"></param>
        /// <param name="useDefaultCommandHandler"></param>
        public void CommandsNextSetup(IEnumerable<string> prefixes, PrefixResolverDelegate prefixResolver = null, bool enableMentionPrefix = true,
                                      bool caseSensitive = false, bool enableDefaultHelp = true, bool directMessageHelp = true, 
                                      IEnumerable<CheckBaseAttribute> defaultHelpChecks = null, bool directMessageCommands = false, 
                                      IServiceCollection services = null, bool ignoreExtraArguments = false, bool useDefaultCommandHandler = true)
        {
            this._commandsNextConfiguration = new CommandsNextConfiguration
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
            };

            this._commandsNext = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                 .UseCommandsNext(this._commandsNextConfiguration);

            this.RegisterCommands();
        }
        
        private void RegisterCommands()
        {
            var botClassOrAssembly = this._botClassOrAssembly;
            var objectType = botClassOrAssembly.GetType();
            this._commandsNext.RegisterCommands(objectType.IsClass && objectType.Name != "RuntimeAssembly" ? objectType.Assembly : (Assembly)botClassOrAssembly);
        }
        #endregion

        #region InteractivitySetup
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactivityConfiguration"></param>
        public void InteractivitySetup(InteractivityConfiguration interactivityConfiguration) 
            => this.Interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                    .UseInteractivity(this._interactivityConfiguration = interactivityConfiguration);

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
        {
            this._interactivityConfiguration = new InteractivityConfiguration
            {
                Timeout = timeout ?? TimeSpan.FromMinutes(5),
                PollBehaviour = pollBehaviour,
                PaginationEmojis = paginationEmojis ?? new PaginationEmojis(),
                PaginationBehaviour = paginationBehaviour,
                PaginationDeletion = paginationDeletion
            };

            this._interactivity = (_discordClient ?? throw new NullReferenceException("Call first the DiscordClientSetup!"))
                                  .UseInteractivity(this._interactivityConfiguration);
        }
        #endregion

        #region ScheduledEventsSetup
        public void ScheduledEventsSetup(params Event[] events)
        {
            var scheduledEvents = new List<Event>();
            
            int iEvent = 0;

            foreach (var scheduledEvent in events)
            {
                if (!scheduledEvents.Contains(scheduledEvent))
                    scheduledEvents.Add(scheduledEvent);

                ++iEvent;
            }
            
            this.ScheduledEvents = scheduledEvents;

            _discordClient.LogMessage(discordBotBaseApplication, $"{(iEvent > 1 ? $"A total of {iEvent} scheduled events were recorded." : $"A total of {iEvent} scheduled event were recorded.")}", LogLevel.Debug);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public async Task StartAsync(DiscordActivity discordActivity = null, UserStatus? userStatus = null, DateTime? idleSince = null)
        {
            if (this._interactivityConfiguration == null)
                this.InteractivitySetup(new InteractivityConfiguration { PollBehaviour = PollBehaviour.DeleteEmojis, Timeout = TimeSpan.FromMinutes(5) });

            _discordClient.LogMessage(discordBotBaseApplication, $"The DiscordBotBase was started successfully! By: luizfernandonb | Version: " +
                                                                 $"{typeof(BotBase).Assembly.GetName().Version}");
            _discordClient.LogMessage(discordBotBaseApplication, $"Connecting do Discord...");
            
            await _discordClient.ConnectAsync(discordActivity, userStatus, idleSince);
            
            _discordClient.LogMessage(discordBotBaseApplication, "Connection successful!");
            
            await Task.Delay(-1);
        }
    }
}