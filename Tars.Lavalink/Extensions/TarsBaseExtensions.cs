using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Tars.Core;
using Tars.Extensions;

namespace Tars.Lavalink.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="TarsBase"/> methods.
    /// </summary>
    public static class TarsBaseExtensions
    {
        private static TarsBase _botBase;

        private static LavalinkExtension _lavalink;
        private static LavalinkConfiguration _lavalinkConfiguration;
        private static LavalinkNodeConnection _lavalinkNodeConnection;

        private static bool _wasConnectedToLavalink, _socketAutoReconnect;

        internal static HttpClient _httpClient;

        /// <summary>
        /// Method to configure <see cref="LavalinkExtension"/> and connect to it using the default values of DSharpPlus.
        /// <para>Default values:</para>
        /// <para>- Hostname: 127.0.0.1 | Port: 2333 | Password: youshallnotpass</para>
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="secured">Sets the secured status associated with the Lavalink.</param>
        /// <param name="region">Sets the voice region ID for the Lavalink connection. This should be used if nodes should be filtered by region with <see cref="LavalinkExtension.GetIdealNodeConnection(DiscordVoiceRegion)"/>.</param>
        /// <param name="resumeKey">Sets the resume key for the Lavalink connection. This will allow existing voice sessions to continue for a certain time after the client is disconnected.</param>
        /// <param name="resumeTimeout">Sets the time in seconds when all voice sessions are closed after the client disconnects. Defaults to 1 minute.</param>
        /// <param name="webSocketCloseTimeout">Sets the time in miliseconds to wait for Lavalink's voice WebSocket to close after leaving a voice channel. This will be the delay before the guild connection is removed. Defaults to 3 minutes.</param>
        public static void LavalinkSetup(this TarsBase botBase, bool secured = false, DiscordVoiceRegion region = null, string resumeKey = null, TimeSpan? resumeTimeout = null,
                                         TimeSpan? webSocketCloseTimeout = null, bool socketAutoReconnect = true)
        {
            _botBase = botBase;

            var connectionEndpoint = new ConnectionEndpoint("127.0.0.1", 2333, secured);
            _lavalinkConfiguration = new LavalinkConfiguration
            {
                SocketEndpoint = connectionEndpoint,
                RestEndpoint = connectionEndpoint,
                Password = "youshallnotpass",
                Region = region,
                ResumeKey = resumeKey,
                ResumeTimeout = (int)(resumeTimeout?.TotalSeconds ?? TimeSpan.FromMinutes(1).TotalSeconds),
                WebSocketCloseTimeout = (int)(webSocketCloseTimeout?.TotalMilliseconds ?? TimeSpan.FromSeconds(3).TotalMilliseconds),
                SocketAutoReconnect = _socketAutoReconnect = socketAutoReconnect
            };
            _lavalink = botBase.Discord.UseLavalink();

            botBase.Discord.Heartbeated += DiscordHeartbeated;

            RegisterExitEvent();
            RegisterLavalinkAsService(botBase);
            HttpClientSetup();
        }

        /// <summary>
        /// Method for configuring <see cref="LavalinkExtension"/>, accessing each configuration individually.
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="hostname">Sets the hostname associated with the Lavalink.</param>
        /// <param name="port">Sets the port associated with the Lavalink.</param>
        /// <param name="password">Sets the password associated with the Lavalink.</param>
        /// <param name="secured">Sets the secured status associated with the Lavalink.</param>
        /// <param name="region">Sets the voice region ID for the Lavalink connection. This should be used if nodes should be filtered by region with <see cref="LavalinkExtension.GetIdealNodeConnection(DiscordVoiceRegion)"/>.</param>
        /// <param name="resumeKey">Sets the resume key for the Lavalink connection. This will allow existing voice sessions to continue for a certain time after the client is disconnected.</param>
        /// <param name="resumeTimeout">Sets the time in seconds when all voice sessions are closed after the client disconnects. Defaults to 1 minute.</param>
        /// <param name="webSocketCloseTimeout">Sets the time in miliseconds to wait for Lavalink's voice WebSocket to close after leaving a voice channel. This will be the delay before the guild connection is removed. Defaults to 3 minutes.</param>
        public static void LavalinkSetup(this TarsBase botBase, string hostname, int port, string password, bool secured = false, DiscordVoiceRegion region = null,
                                         string resumeKey = null, TimeSpan? resumeTimeout = null, TimeSpan? webSocketCloseTimeout = null, bool socketAutoReconnect = true)
        {
            _botBase = botBase;

            var connectionEndpoint = new ConnectionEndpoint(hostname, port, secured);
            _lavalinkConfiguration = new LavalinkConfiguration
            {
                SocketEndpoint = connectionEndpoint,
                RestEndpoint = connectionEndpoint,
                Password = password,
                Region = region,
                ResumeKey = resumeKey,
                ResumeTimeout = (int)(resumeTimeout?.TotalSeconds ?? TimeSpan.FromMinutes(1).TotalSeconds),
                WebSocketCloseTimeout = (int)(webSocketCloseTimeout?.TotalMilliseconds ?? TimeSpan.FromSeconds(3).TotalMilliseconds),
                SocketAutoReconnect = _socketAutoReconnect = socketAutoReconnect
            };
            _lavalink = botBase.Discord.UseLavalink();

            botBase.Discord.Heartbeated += DiscordHeartbeated;

            RegisterExitEvent();
            RegisterLavalinkAsService(botBase);
            HttpClientSetup();
        }

        /// <summary>
        /// Get the <see cref="LavalinkExtension"/>.
        /// </summary>
        /// <param name="_"></param>
        /// <returns>The <see cref="LavalinkExtension"/>.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static LavalinkExtension GetLavalink(this TarsBase _) => _lavalink ?? throw new NullReferenceException("Connect the bot to Lavalink!");

        #region Private methods
        private static async Task DiscordHeartbeated(DiscordClient client, HeartbeatEventArgs _)
        {
            if (_lavalinkNodeConnection?.IsConnected == true)
                return;

            if (_socketAutoReconnect && _wasConnectedToLavalink)
                return;

            _lavalinkNodeConnection = await _lavalink.ConnectAsync(_lavalinkConfiguration);

            if (!_wasConnectedToLavalink)
                client.LogMessage("Successfully connected to Lavalink.");
            else
                client.LogMessage("The connection to Lavalink has been re-established.");

            if (!_wasConnectedToLavalink)
                _wasConnectedToLavalink = true;
        }

        private static void RegisterLavalinkAsService(TarsBase botBase)
            => (typeof(TarsBase).GetField("_services", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(botBase) as IServiceCollection).AddSingleton(_lavalink);

        private static void RegisterExitEvent() => AppDomain.CurrentDomain.ProcessExit += Exit;

        // This method is a little slow :p
        // I don't know how I can improve it
        private static void Exit(object sender, EventArgs e)
        {
            var tasks = new List<Task>();

            foreach (LavalinkGuildConnection guild in _lavalinkNodeConnection?.ConnectedGuilds?.Values)
            {
                tasks.Add(guild?.StopAsync());
                tasks.Add(guild?.DisconnectAsync());
            }

            foreach (LavalinkNodeConnection nodes in _lavalink?.ConnectedNodes?.Values)
                tasks.Add(nodes?.StopAsync());

            Task.WhenAll(tasks).GetAwaiter().GetResult();
        }

        private static void HttpClientSetup() => _httpClient = new HttpClient();
        #endregion
    }
}