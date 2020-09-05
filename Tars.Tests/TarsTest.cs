using Tars.Core;

namespace Tars.Tests
{
    public sealed class TarsTest
    {
        public TarsTest()
        {
            var botBase = new TarsBase(this);
            botBase.DiscordSetup(":p");
            botBase.CommandsSetup(new string[] { "tars" });
            botBase.StartAsync().GetAwaiter().GetResult();
        }
    }
}