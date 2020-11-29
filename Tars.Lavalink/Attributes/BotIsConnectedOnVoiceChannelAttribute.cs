using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Tars.Lavalink.Exceptions;

namespace Tars.Lavalink.Attributes
{
    /// <summary>
    /// Checks whether the bot is connected to a voice channel via Lavalink.
    /// </summary>
    public sealed class BotIsConnectedOnVoiceChannelAttribute : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            var lavalink = ctx.Services.GetService<LavalinkExtension>();
            if (lavalink is null)
                throw new LavalinkNotRegistered("The Lavalink isn't registered as service, call the LavalinkSetup before the CommandsSetup!");

            foreach (LavalinkNodeConnection node in lavalink.ConnectedNodes?.Values)
                return Task.FromResult(true);

            return Task.FromResult(false);
        }
    }
}