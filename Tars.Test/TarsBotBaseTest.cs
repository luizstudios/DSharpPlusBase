using DSharpPlus.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Tars.Core;
using Tars.Extensions;

namespace Tars.Test
{
    [TestClass]
    public class TarsBotBaseTest
    {
        private TarsBotBase _bot;

        #region Initialize and Cleanup
        [TestInitialize]
        public async Task TestInitialize()
        {
            this._bot = new TarsBotBase(this);
            this._bot.DiscordClientSetup("NzQ5NzE3Njk0MzQ5MTgwOTU4.X0wDAw.hzMBVSS2XshngNHqBZoCR4Q_eOc");
            this._bot.CommandsNextSetup(new string[] { "tars" });

            _ = Task.Run(async () => await this._bot.StartAsync(new DiscordActivity { Name = "Running all tests..." }, UserStatus.DoNotDisturb));

            await Task.Delay(TimeSpan.FromSeconds(15));
        }

        [TestCleanup]
        public void TestCleanup() => this._bot.Dispose();
        #endregion

        [TestMethod]
        public void TestExtensions_Number_IsNotNull()
        {
            DiscordMember luizMember = 322745409074102282.ToDiscordMember();
            DiscordChannel supportChannel = 749718492781215757.ToDiscordChannel();
            DiscordGuild tarsGuild = 749718492781215754.ToDiscordGuild();
            DiscordRole libraryRole = 749722451151290519.ToDiscordRole();
            DiscordEmoji lulEmoji = 749745553025269767.ToDiscordEmoji();

            Assert.IsNotNull(luizMember);
            Assert.IsNotNull(supportChannel);
            Assert.IsNotNull(tarsGuild);
            Assert.IsNotNull(libraryRole);
            Assert.IsNotNull(lulEmoji);
        }

        [TestMethod]
        public void TestExtensions_String_IsNotNull()
        {
            DiscordMember luizMember = "Luiz Fernando".ToDiscordMember();
            DiscordChannel supportChannel = "support".ToDiscordChannel();
            DiscordGuild tarsGuild = "Tars".ToDiscordGuild();
            DiscordRole libraryRole = "Library".ToDiscordRole();
            DiscordEmoji lulEmoji = "lul".ToDiscordEmoji();

            Assert.IsNotNull(luizMember);
            Assert.IsNotNull(supportChannel);
            Assert.IsNotNull(tarsGuild);
            Assert.IsNotNull(libraryRole);
            Assert.IsNotNull(lulEmoji);
        }
    }
}