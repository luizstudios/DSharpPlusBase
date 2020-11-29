using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tars.Classes;
using Tars.Lavalink.Entities;
using Tars.Lavalink.Extensions;

namespace Tars.Tests.Services.Lavalink
{
    public sealed class MusicService
    {
        #region Private properties
        private readonly DiscordClient _discord;
        private readonly LavalinkExtension _lavalinkExtension;
        private LavalinkNodeConnection _lavalinkNodeConnection;

        private bool _isPaused, _isPlaying, _isStopped;
        private int _reconnections;
        #endregion

        #region Public properties
        public Dictionary<LavalinkTrack, CommandContext> Playlist { get; private set; }
        public LavalinkGuildConnection LavalinkGuildConnection { get; private set; }
        #endregion

        public MusicService(LavalinkExtension lavalink, DiscordClient discord)
        {
            this._lavalinkExtension = lavalink;
            this._discord = discord;

            this._discord.Ready += this.Ready;
        }

        private Task Ready(DiscordClient client, ReadyEventArgs e)
        {
            this._lavalinkNodeConnection = this._lavalinkExtension?.ConnectedNodes?.FirstOrDefault().Value;

            return Task.CompletedTask;
        }

        public async Task PlayAsync(CommandContext ctx, string searchQuery)
        {
            if (this._isPaused && !this._isStopped)
            {
                await this.LavalinkGuildConnection.ResumeAsync();

                return;
            }

            DiscordChannel commandChannel = ctx.Channel;
            DiscordMember memberCommand = ctx.Member;
            string memberCommandMention = memberCommand.Mention;

            this.LavalinkGuildConnection = this._lavalinkNodeConnection.GetGuildConnection(ctx.Guild);
            if (this.LavalinkGuildConnection?.IsConnected != true)
            {
                DiscordChannel memberVoiceChannel = ctx.Member.VoiceState?.Channel;
                if (memberVoiceChannel is null)
                {
                    await commandChannel.SendMessageAsync($"{memberCommandMention}, join a voice channel!!");

                    return;
                }

                this.LavalinkGuildConnection = await this._lavalinkNodeConnection.ConnectAsync(memberVoiceChannel);
                await this.LavalinkGuildConnection.SetVolumeAsync(50);
                this.LavalinkGuildConnection.PlaybackFinished += this.PlaybackFinished;
                this.LavalinkGuildConnection.PlaybackStarted += this.PlaybackStarted;
                this.LavalinkGuildConnection.TrackException += this.TrackError;
                this.LavalinkGuildConnection.TrackStuck += this.TrackStuck;
            }

            LavalinkLoadResult resultTracks = !Uri.TryCreate(searchQuery, UriKind.Absolute, out Uri resultUri) ?
                                              await this.LavalinkGuildConnection.GetTracksAsync(searchQuery) :
                                              await this.LavalinkGuildConnection.GetTracksAsync(resultUri);

            if (this.Playlist is null)
                this.Playlist = new Dictionary<LavalinkTrack, CommandContext>();

            switch (resultTracks.LoadResultType)
            {
                case LavalinkLoadResultType.TrackLoaded:
                case LavalinkLoadResultType.SearchResult:
                    {
                        var trackResult = resultTracks.Tracks.FirstOrDefault();
                        this.Playlist.Add(trackResult, ctx);

                        if (this._isPlaying)
                        {
                            await commandChannel.SendMessageAsync($"{memberCommandMention}, `{trackResult.Author}'s` - `{trackResult.Title}` song was successfully added to the playlist.");

                            return;
                        }

                        break;
                    }
                case LavalinkLoadResultType.PlaylistLoaded:
                    {
                        IEnumerable<LavalinkTrack> listOfTracks = resultTracks.Tracks;
                        foreach (LavalinkTrack trackForeach in listOfTracks)
                            this.Playlist.Add(trackForeach, ctx);

                        if (this._isPlaying)
                        {
                            await commandChannel.SendMessageAsync($"{memberCommandMention}, `{listOfTracks.Count()}` songs have been successfully added to the playlist.");

                            return;
                        }

                        break;
                    }
                case LavalinkLoadResultType.NoMatches:
                    {
                        await commandChannel.SendMessageAsync($"{memberCommandMention}, no songs were found, please search again later.");

                        return;
                    }
                case LavalinkLoadResultType.LoadFailed:
                    {
                        if (this._reconnections == 3)
                        {
                            await commandChannel.SendMessageAsync($"{memberCommandMention}, an error occurred while trying to load this song, please try again.");

                            return;
                        }

                        ++this._reconnections;

                        await this.PlayAsync(ctx, searchQuery);

                        return;
                    }
            }

            LavalinkTrack track = this.Playlist.Keys.FirstOrDefault();
            await this.LavalinkGuildConnection.PlayAsync(track);

            this._isPlaying = true;
        }

        private async Task PlaybackStarted(LavalinkGuildConnection guildConnection, TrackStartEventArgs e)
        {
            LavalinkTrack track = e.Track;
            var kv = this.Playlist.FirstOrDefault(kv => string.Equals(kv.Key.Title, track.Title));
            DiscordMember memberCommand = kv.Value.Member;

            await kv.Value.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor { Name = "Playing now:" },
                Description = $"Song: {Formatter.MaskedUrl(track.Title, track.Uri)}\n" +
                              $"Author: {track.Author}\n" +
                              $"Length: {track.Length.Humanize()}\n" +
                              $"Requested by: {memberCommand.DisplayName}#{memberCommand.Discriminator}",
                Color = DiscordEmbedColor.RandomColorEmbed(),
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = track.GetThumbnailLink(Thumbnail.HqDefault).AbsoluteUri }
            });

            if (!this._isPlaying)
                this._isPlaying = true;

            if (this._isPaused)
                this._isPaused = false;

            if (this._isStopped)
                this._isStopped = false;
        }

        private async Task PlaybackFinished(LavalinkGuildConnection guildConnection, TrackFinishEventArgs e)
        {
            var trackFind = this.Playlist.FirstOrDefault(t => string.Equals(t.Key?.Title, e.Track.Title));
            if (!(trackFind.Key is null && trackFind.Value is null))
                this.Playlist.Remove(trackFind.Key);

            if (this.Playlist.Any())
                await this.LavalinkGuildConnection.PlayAsync(this.Playlist.Keys.FirstOrDefault());
            else
            {
                await trackFind.Value.RespondAsync("There are no more songs in the playlist, disconnecting me from the voice channel.");
                await this.LavalinkGuildConnection.DisconnectAsync(false);

                if (this._isPlaying)
                    this._isPlaying = false;

                if (!this._isPaused)
                    this._isPaused = true;

                if (!this._isStopped)
                    this._isStopped = true;
            }
        }

        private async Task TrackError(LavalinkGuildConnection guildConnection, TrackExceptionEventArgs e)
        {
            LavalinkTrack track = e.Track;
            var kv = this.Playlist.FirstOrDefault(kv => string.Equals(kv.Key.Title, track.Title));
            CommandContext ctx = kv.Value;

            await ctx.RespondAsync($"{ctx.Member.Mention}, it was not possible to play the song `{track.Title}` by `{track.Author}`.");
        }

        private async Task TrackStuck(LavalinkGuildConnection guildConnection, TrackStuckEventArgs e)
        {
            LavalinkTrack track = e.Track;
            string trackTitle = track.Title;

            var kv = this.Playlist.FirstOrDefault(kv => string.Equals(kv.Key.Title, trackTitle));
            CommandContext ctx = kv.Value;

            await ctx.RespondAsync($"{ctx.Member.Mention}, it was not possible to play the song `{track.Title}` by `{track.Author}`.");
        }

        public async Task StopAsync(CommandContext ctx)
        {
            await this.LavalinkGuildConnection.StopAsync();
            await this.LavalinkGuildConnection.DisconnectAsync(false);

            this.Playlist.Clear();

            if (this._isPlaying)
                this._isPlaying = false;

            if (!this._isPaused)
                this._isPaused = true;

            if (!this._isStopped)
                this._isStopped = true;

            await ctx.RespondAsync($"{ctx.Member.Mention}, the music was stopped and I disconnected from the channel.");
        }

        public async Task PauseAsync(CommandContext ctx)
        {
            await this.LavalinkGuildConnection.PauseAsync();

            if (this._isPlaying)
                this._isPlaying = false;

            if (!this._isPaused)
                this._isPaused = true;

            if (!this._isStopped)
                this._isStopped = true;

            await ctx.RespondAsync($"{ctx.Member.Mention}, the music has been paused.");
        }

        public async Task SetVolumeAsync(CommandContext ctx, int? volume)
        {
            string memberCommandMention = ctx.Member.Mention;

            if (!volume.HasValue)
            {
                await ctx.RespondAsync($"{memberCommandMention}, enter the new volume of the song!");

                return;
            }

            int realVolume = volume.Value;
            await this.LavalinkGuildConnection.SetVolumeAsync(realVolume);
            await ctx.RespondAsync($"{memberCommandMention}, the music volume was set to `{realVolume}%`");
        }

        public async Task SkipAsync(CommandContext ctx)
        {
            LavalinkTrack atualTrack = this.LavalinkGuildConnection?.CurrentState?.CurrentTrack;
            await this.LavalinkGuildConnection.StopAsync();

            await ctx.RespondAsync($"{ctx.Member.Mention}, the song `{atualTrack.Title}` by `{atualTrack.Author}` was skipped.");
        }
    }
}