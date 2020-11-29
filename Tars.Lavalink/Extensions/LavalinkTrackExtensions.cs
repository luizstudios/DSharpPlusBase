using DSharpPlus.Lavalink;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tars.Lavalink.Entities;

namespace Tars.Lavalink.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="LavalinkTrack"/> methods.
    /// </summary>
    public static class LavalinkTrackExtensions
    {
        /// <summary>
        /// Get the <see cref="Uri"/> for the <see cref="LavalinkTrack"/> thumbnail.
        /// </summary>
        /// <param name="track"></param>
        /// <param name="thumbnailResolution">Thumbnail resolution, the default is <see cref="Thumbnail.Default"/>.</param>
        /// <returns>A <see cref="Uri"/> with the thumbnail link</returns>
        public static Uri GetThumbnailLink(this LavalinkTrack track, Thumbnail thumbnailResolution = Thumbnail.Default)
        {
            var youtubeLink = $"https://img.youtube.com/vi/{track.Identifier}/";

            UriKind uriKind = UriKind.Absolute;

            switch (thumbnailResolution)
            {
                case Thumbnail.Default:
                    {
                        Uri.TryCreate(youtubeLink + "default.jpg", uriKind, out Uri resultLink);

                        return resultLink;
                    }
                case Thumbnail.SdDefault:
                    {
                        Uri.TryCreate(youtubeLink + "sddefault.jpg", uriKind, out Uri resultLink);

                        return resultLink;
                    }
                case Thumbnail.MqDefault:
                    {
                        Uri.TryCreate(youtubeLink + "mqdefault.jpg", uriKind, out Uri resultLink);

                        return resultLink;
                    }
                case Thumbnail.HqDefault:
                    {
                        Uri.TryCreate(youtubeLink + "hqdefault.jpg", uriKind, out Uri resultLink);

                        return resultLink;
                    }
                case Thumbnail.MaxResDefault:
                    {
                        Uri.TryCreate(youtubeLink + "maxresdefault.jpg", uriKind, out Uri resultLink);

                        return resultLink;
                    }
                default:
                    throw new InvalidOperationException("This thumbnail quality does not exist!");
            }
        }

        /// <summary>
        /// Get the lyrics for this song.
        /// </summary>
        /// <param name="track"></param>
        /// <returns>A <see langword="string"/> with the lyrics of this song or <see langword="null"/> if doesn't exist or wasn't found.</returns>
        public static async Task<string> GetLyricsAsync(this LavalinkTrack track)
        {
            string trackAuthor = track.Author;
            HttpResponseMessage response = await TarsBaseExtensions._httpClient.GetAsync($"https://api.lyrics.ovh/v1/{trackAuthor.Replace(" - Topic", string.Empty)}/{Regex.Replace(track.Title, @"(\().*(\))|(\[).*(\])|:|'|-", string.Empty).Replace(trackAuthor, string.Empty)}");
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync())["lyrics"].Value<string>().Replace("\n\n", "\n") : null;
        }
    }
}