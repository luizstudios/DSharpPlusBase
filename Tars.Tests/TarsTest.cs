using System.Text;
using Tars.Core;

namespace Tars.Tests
{
    public sealed class TarsTest
    {
        public TarsTest()
        {
            var botBase = new TarsBase(this);
            //"nothing here, go out"
            botBase.DiscordClientSetup(Encoding.UTF8.GetString(new byte[] { 78, 122, 81, 53, 78, 122, 69, 51, 78, 106, 107, 48, 77, 122, 81, 53, 77, 84, 103, 119, 79, 84,
                                                                            85, 52, 46, 88, 48, 119, 68, 65, 119, 46, 80, 77, 110, 55, 51, 99, 71, 122, 118, 101, 83, 54,
                                                                            56, 122, 113, 66, 74, 79, 117, 90, 98, 49, 105, 66, 48, 80, 107 }));
            botBase.CommandsNextSetup(new string[] { "tars" });
            botBase.StartAsync().GetAwaiter().GetResult();
        }
    }
}