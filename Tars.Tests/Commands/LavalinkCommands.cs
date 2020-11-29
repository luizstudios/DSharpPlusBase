using Tars.Tests.Services.Lavalink;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using Humanizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tars.Classes;
using Tars.Extensions;
using Tars.Lavalink.Attributes;
using Tars.Lavalink.Entities;
using Tars.Lavalink.Extensions;

namespace Tars.Tests.Commands
{
    [Group("lavalink")]
    public sealed class LavalinkCommands : BaseCommandModule
    {
        public MusicService MusicService { get; set; }

        [Command("play"), Aliases("p")]
        public async Task PlayAsync(CommandContext ctx, [RemainingText] string musicNameOrLink)
        {
            await ctx.TriggerTypingAsync();

            if (musicNameOrLink.IsNullOrEmptyOrWhiteSpace())
            {
                await ctx.RespondAsync($"{ctx.Member.Mention}, type the name of the song or type a link!");

                return;
            }

            await this.MusicService.PlayAsync(ctx, musicNameOrLink);
        }

        [Command("pause"), BotIsConnectedOnVoiceChannel]
        public async Task PauseAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await this.MusicService.PauseAsync(ctx);
        }

        [Command("stop"), BotIsConnectedOnVoiceChannel]
        public async Task StopAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await this.MusicService.StopAsync(ctx);
        }

        [Command("volume"), BotIsConnectedOnVoiceChannel]
        public async Task SetVolumeAsync(CommandContext ctx, int volume = 0)
        {
            await ctx.TriggerTypingAsync();

            if (volume == 0)
            {
                await ctx.RespondAsync($"{ctx.Member.Mention}, enter the new volume of the song!");

                return;
            }

            await this.MusicService.SetVolumeAsync(ctx, volume);
        }

        [Command("skip"), BotIsConnectedOnVoiceChannel]
        public async Task SkipAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            await this.MusicService.SkipAsync(ctx);
        }

        [Command("playlist"), BotIsConnectedOnVoiceChannel]
        public async Task PlaylistAsync(CommandContext ctx)
        {
            var i = 0;
            var embed = new DiscordEmbedBuilder();
            Dictionary<LavalinkTrack, CommandContext> playlist = this.MusicService.Playlist;

            foreach (KeyValuePair<LavalinkTrack, CommandContext> kv in playlist)
            {
                LavalinkTrack track = kv.Key;
                DiscordMember ctxForeach = kv.Value.Member;
                embed.AddField($"{++i}. - {track.Title}", $"Author: {track.Author}\nLength: {track.Length.Humanize()}\nRequested by: {ctxForeach.DisplayName}#{ctxForeach.Discriminator}");
            }

            embed.WithThumbnail(playlist.Keys.FirstOrDefault()?.GetThumbnailLink(Thumbnail.HqDefault)?.AbsoluteUri);

            await ctx.RespondAsync(embed: embed.WithAuthor("Current playlist:")
                                               .WithColor()
                                               .Build());
        }

        [Command("nowplaying"), BotIsConnectedOnVoiceChannel]
        public async Task NowPlayingAsync(CommandContext ctx)
        {
            var kv = this.MusicService.Playlist.FirstOrDefault(kv => string.Equals(kv.Key.Title, this.MusicService.LavalinkGuildConnection.CurrentState.CurrentTrack.Title));

            LavalinkTrack track = kv.Key;
            DiscordMember member = kv.Value.Member,
                          ctxMember = ctx.Member;

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor { Name = "Current song:" },
                Color = DiscordEmbedColor.RandomColorEmbed(),
                Description = $"Title: {Formatter.MaskedUrl(track.Title, track.Uri)}\n" +
                              $"Author: {track.Author}\n" +
                              $"Length: {track.Length.Humanize()}\n" +
                              $"Requested by: {member.DisplayName}#{member.Discriminator}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = track.GetThumbnailLink(Thumbnail.HqDefault).AbsoluteUri }
            }.Build());
        }

        [Command("lyrics"), BotIsConnectedOnVoiceChannel]
        public async Task LyricsAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            string lyrics = await this.MusicService.Playlist.FirstOrDefault(kv => string.Equals(kv.Key.Title, this.MusicService.LavalinkGuildConnection.CurrentState.CurrentTrack.Title)).Key?.GetLyricsAsync();
            await ctx.RespondAsync(lyrics ?? "The lyrics wasn't find.");
        }
    }
}