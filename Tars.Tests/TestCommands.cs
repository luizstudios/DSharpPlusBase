using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Tars.Tests
{
    public sealed class TestCommands : BaseCommandModule
    {
        public MongoClient MongoClient { get; set; }

        [Command("mongodb")]
        public async Task MongoDBAsync(CommandContext ctx) => await ctx.TriggerTypingAsync();
    }
}