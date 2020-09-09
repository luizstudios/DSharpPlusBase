using DSharpPlus.Lavalink;
using System;
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
            string youtubeLink = $"https://img.youtube.com/vi/{track.Identifier}/";

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
    }
}