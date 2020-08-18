using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
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
    public sealed class BotBase
    {
        public BotBaseConfiguration BotBaseConfiguration { get; set; }
        public BotBaseDiscordConfiguration BotBaseDiscordConfiguration { get; set; }
        public BotBaseCommandsNextConfiguration BotBaseCommandsNextConfiguration { get; set; }
        public BotBaseInteractivityConfiguration BotBaseInteractivityConfiguration { get; set; }

        private DiscordClient _discordClient;
        private CommandsNextExtension _commandsNextExtension;

        public async Task InitializeAsync(object botClassOrAssembly)
        {
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

            if (this.BotBaseInteractivityConfiguration == null)
                this.BotBaseInteractivityConfiguration = new BotBaseInteractivityConfiguration();
            this._discordClient.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = this.BotBaseInteractivityConfiguration.PaginationBehaviour,
                PaginationDeletion = this.BotBaseInteractivityConfiguration.PaginationDeletion,
                PaginationEmojis = this.BotBaseInteractivityConfiguration.PaginationEmojis,
                PollBehaviour = this.BotBaseInteractivityConfiguration.PollBehaviour,
                Timeout = this.BotBaseInteractivityConfiguration.Timeout
            });

            this._discordClient.DebugLogger.LogMessage(LogLevel.Info, "DSharpPlusBase", "DSharpPlusBase started successfully", DateTime.Now);

            if (this.BotBaseConfiguration.ConnectAfterInitialize)
                await this._discordClient.ConnectAsync();
        }
    }
}
