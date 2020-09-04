using Tars.Core;

namespace Tars.Tests
{
    public sealed class TarsTest
    {
        public TarsTest()
        {
            var botBase = new TarsBase(this);
            botBase.DiscordClientSetup("nothing here, go out");
            botBase.CommandsNextSetup(new string[] { "tars" });
            botBase.StartAsync().GetAwaiter().GetResult();
        }
    }
}