using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using Tars.Extensions;
using Tars.Tests.Core;

namespace Tars.Tests.Commands
{
    public sealed class BasicCommands : BaseCommandModule
    {
        public TarsTest Bot { get; set; }

        [Command("hey")]
        public async Task HelloAsync(CommandContext ctx) => await ctx.RespondAsync("Hello!");
    }
}