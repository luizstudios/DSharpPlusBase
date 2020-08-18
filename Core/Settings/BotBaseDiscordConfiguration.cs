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

namespace DSharpPlusBase.Core.Settings
{
    public sealed class BotBaseDiscordConfiguration 
    {
        public bool AutoReconnect { get; set; } = false;
        public string DateTimeFormat
        {
            get
            {
                var pcDateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

                return $"{pcDateTimeFormat.ShortDatePattern} {pcDateTimeFormat.ShortTimePattern}";
            }
            set => this.DateTimeFormat = value;
        }
        public GatewayCompressionLevel GatewayCompressionLevel { get; set; } = GatewayCompressionLevel.Stream;
        public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(10);
        public int LargeThreshold { get; set; } = 1000;
        public LogLevel LogLevel { get; set; } = Debugger.IsAttached ? LogLevel.Debug : LogLevel.Info;
        public int MessageCacheSize { get; set; } = 1024;
        public bool ReconnectIndefinitely { get; set; } = false;
        public int ShardCount { get; set; } = 1;
        public int ShardId { get; set; } = 0;
        public string Token { get; set; } = string.Empty;
        public TokenType TokenType { get; set; } = TokenType.Bot;
        public UdpClientFactoryDelegate UdpClientFactory { get; set; } = null;
        public bool UseRelativeRatelimit { get; set; } = true;
        public bool UseInternalLogHandler { get; set; } = true;
        public IWebProxy WebProxy { get; set; } = null;
        public WebSocketClientFactoryDelegate WebSocketClientFactory { get; set; } = null;
    }
}