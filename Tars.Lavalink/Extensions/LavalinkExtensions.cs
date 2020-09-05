using DSharpPlus.Lavalink;
using System;
using System.Linq;

namespace Tars.Lavalink.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="LavalinkExtension"/> methods.
    /// </summary>
    public static class LavalinkExtensions
    {
        /// <summary>
        /// Get the first node connected to <see cref="LavalinkExtension"/>.
        /// </summary>
        /// <param name="lavalink"></param>
        /// <returns>The first <see cref="LavalinkNodeConnection"/>.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static LavalinkNodeConnection GetFirstNodeConnectedToLavalink(this LavalinkExtension lavalink) => lavalink?.ConnectedNodes?.Values?.FirstOrDefault() ??
                                                                                                                 throw new NullReferenceException("Connect the bot to Lavalink!");
    }
}