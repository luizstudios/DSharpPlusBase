using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace Tars.Tests
{
    public sealed class TestCommands : BaseCommandModule
    {
        public TarsTest Bot { get; set; }

        [Command("hey")]
        public async Task HelloAsync(CommandContext ctx) => await ctx.RespondAsync("Hello!");
    }
}