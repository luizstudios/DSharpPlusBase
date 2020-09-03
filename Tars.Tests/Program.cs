using System.Reflection;
using System.Threading.Tasks;
using Tars.Core;

namespace Tars.Tests
{
    public static class Program
    {
        private static async Task Main(string[] _)
        {
            var botBase = new TarsBase(Assembly.GetEntryAssembly());
            botBase.DiscordClientSetup("nothing here, go out");
            botBase.CommandsNextSetup(new string[] { "tars" });

            await botBase.StartAsync();
        }
    }
}
