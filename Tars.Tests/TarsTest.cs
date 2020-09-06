using Microsoft.Extensions.DependencyInjection;
using Tars.Core;

namespace Tars.Tests
{
    public sealed class TarsTest
    {
        /// <summary>
        /// Constructor of Tars.Test.
        /// </summary>
        public TarsTest()
        {
            var botBase = new TarsBase(this);
            botBase.DiscordSetup("YOUR TOKEN HERE");
            botBase.CommandsSetup(new string[] { "tars" }, services: new ServiceCollection().AddSingleton(this));
            botBase.StartAsync().GetAwaiter().GetResult();
        }
    }
}