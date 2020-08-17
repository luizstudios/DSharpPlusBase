using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Net.Udp;
using DSharpPlus.Net.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace DSharpPlusBase.Core
{
    public class DSharpPlusBase
    {
        #region DiscordClient
        public bool AutoReconnect { get; private set; } = true;
        public string DateTimeFormat { get; private set; } = DateTime.Now.ToString(CultureInfo.CurrentCulture);
        public bool ConnectAfterInitialize { get; private set; } = true;
        public GatewayCompressionLevel GatewayCompressionLevel { get; private set; } = GatewayCompressionLevel.Stream;
        public TimeSpan HttpTimeout { get; private set; } = TimeSpan.FromSeconds(10);
        public int LargeThreshold { get; private set; } = 1000;
        public LogLevel LogLevel { get; private set; } = Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info;
        public int MessageCacheSize { get; private set; } = 1024;
        public bool ReconnectIndefinitely { get; private set; } = false;
        public int ShardCount { get; private set; } = 1;
        public int ShardId { get; private set; }
        public string Token { get; private set; }
        public TokenType TokenType { get; private set; } = TokenType.Bot;
        public UdpClientFactoryDelegate UdpClientFactory { get; private set; }
        public bool UseRelativeRatelimit { get; private set; } = true;
        public bool UseInternalLogHandler { get; private set; } = true;
        public IWebProxy WebProxy { get; private set; }
        public WebSocketClientFactoryDelegate WebSocketClientFactory { get; private set; }
        #endregion

        #region CommandsNextExtension
        public bool CaseSensitive { get; private set; } = false;
        public IEnumerable<CheckBaseAttribute> DefaultHelpChecks { get; private set; }
        public bool DirectMessageHelp { get; private set; } = false;
        public bool EnableDefaultHelp { get; private set; } = true;
        public bool EnableDirectMessages { get; private set; } = true;
        public bool EnableMentionPrefix { get; private set; } = true;
        public bool IgnoreExtraArguments { get; private set; } = false;
        public string[] Prefixes { get; private set; }
        public PrefixResolverDelegate PrefixResolver { get; private set; }
        public IServiceProvider Services { get; private set; }
        public bool UseDefaultCommandHandler { get; private set; } = true;
        #endregion

        #region InteractivityExtension

        #endregion

        private DiscordClient _discordClient;
        private CommandsNextConfiguration _commandsNextConfiguration;

        public async Task InitializeAsync(Type bot)
        {
            this._discordClient = new DiscordClient(new DiscordConfiguration 
            { 
                AutoReconnect = this.AutoReconnect,
                DateTimeFormat = this.DateTimeFormat,
                GatewayCompressionLevel = this.GatewayCompressionLevel,
                HttpTimeout = this.HttpTimeout,
                LargeThreshold = this.LargeThreshold,
                LogLevel = this.LogLevel,
                MessageCacheSize = this.MessageCacheSize,
                Proxy = this.WebProxy,
                ReconnectIndefinitely = this.ReconnectIndefinitely,
                ShardCount = this.ShardCount,
                ShardId = this.ShardId,
                Token = this.Token,
                TokenType = this.TokenType,
                UdpClientFactory = this.UdpClientFactory,
                UseInternalLogHandler = this.UseInternalLogHandler,
                UseRelativeRatelimit = this.UseRelativeRatelimit,
                WebSocketClientFactory = this.WebSocketClientFactory
            });

            var commandsNextConfiguration = new CommandsNextConfiguration
            {
                CaseSensitive = this.CaseSensitive,
                DefaultHelpChecks = this.DefaultHelpChecks,
                DmHelp = this.DirectMessageHelp,
                IgnoreExtraArguments = this.IgnoreExtraArguments,
                EnableDefaultHelp = this.EnableDefaultHelp,
                EnableDms = this.EnableDirectMessages,
                EnableMentionPrefix = this.EnableMentionPrefix,
                PrefixResolver = this.PrefixResolver,
                StringPrefixes = this.Prefixes,
                UseDefaultCommandHandler = this.UseDefaultCommandHandler
            };
            commandsNextConfiguration.Services = this.Services != null 
                                                 ? new ServiceCollection().AddSingleton(this.Services.GetServices<Type>()).AddSingleton(bot).BuildServiceProvider(true)
                                                 : new ServiceCollection().AddSingleton(bot).BuildServiceProvider(true);

            this._discordClient.UseCommandsNext(commandsNextConfiguration);

            this._discordClient.UseInteractivity(new InteractivityConfiguration
            {


            });

            if (this.ConnectAfterInitialize)
                await this._discordClient.ConnectAsync();
        }
    }
}
