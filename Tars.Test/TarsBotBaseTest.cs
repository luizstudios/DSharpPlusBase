using DSharpPlus.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;
using Tars.Core;
using Tars.Extensions;

namespace Tars.Test
{
    [TestClass]
    public class TarsBotBaseTest
    {
        private TarsBase _bot;

        public TestContext TestContext { get; set; }

        #region Initialize and Cleanup
        [TestInitialize]
        public async Task TestInitialize()
        {
            this._bot = new TarsBase(this);

            // This was done to escape Discord's warning that the bot's token is "unprotected".
            this._bot.DiscordClientSetup(Encoding.UTF8.GetString(new byte[] { 78, 122, 81, 53, 78, 122, 69, 51, 78, 106, 107, 48, 77, 122, 81, 53, 77, 84, 103, 119, 79, 84,
                                                                              85, 52, 46, 88, 48, 119, 68, 65, 119, 46, 80, 77, 110, 55, 51, 99, 71, 122, 118, 101, 83, 54,
                                                                              56, 122, 113, 66, 74, 79, 117, 90, 98, 49, 105, 66, 48, 80, 107 }));
            this._bot.CommandsNextSetup(new string[] { "tars" });

            _ = Task.Run(async () => await this._bot.StartAsync(new DiscordActivity { Name = "Running all tests..." }, UserStatus.DoNotDisturb));

            await Task.Delay(TimeSpan.FromSeconds(15));
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            if (this.TestContext.TestName == "TestExtensions_String_IsNotNull")
            {
                await this._bot.DiscordClient.UpdateStatusAsync(new DiscordActivity { Name = "Done!" }, UserStatus.Online);

                return;
            }

            this._bot.Dispose();
        }
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