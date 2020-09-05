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
    public class TarsBaseTest
    {
        private TarsBase _bot;

        [TestInitialize]
        public async Task TestInitialize()
        {
            this._bot = new TarsBase(this);

            // This was done to escape Discord's warning that the bot's token is "unprotected".
            this._bot.DiscordSetup(Encoding.UTF8.GetString(new byte[] { 78, 122, 81, 53, 78, 122, 69, 51, 78, 106, 107, 48, 77, 122, 81, 53, 77, 84, 103, 119, 79, 84,
                                                                              85, 52, 46, 88, 48, 119, 68, 65, 119, 46, 80, 77, 110, 55, 51, 99, 71, 122, 118, 101, 83, 54,
                                                                              56, 122, 113, 66, 74, 79, 117, 90, 98, 49, 105, 66, 48, 80, 107 }));
            this._bot.CommandsSetup(new string[] { "tars" });

            _ = Task.Run(async () => await this._bot.StartAsync(new DiscordActivity { Name = "Running all tests..." }, UserStatus.DoNotDisturb));

            await Task.Delay(TimeSpan.FromSeconds(15));
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await this._bot.Discord.UpdateStatusAsync(new DiscordActivity { Name = "Done!" }, UserStatus.Online);

            await Task.Delay(TimeSpan.FromSeconds(5));

            this._bot.Dispose();
        }

        [TestMethod]
        public void TestExtensions_IsNotNull()
        {
            DiscordMember luizMemberId = 322745409074102282.ToDiscordMember(),
                          luizMemberIdOnString = "322745409074102282".ToDiscordMember(),
                          luizMemberString = "Luiz Fernando".ToDiscordMember();
            DiscordChannel supportChannelId = 749718492781215757.ToDiscordChannel(),
                           supportChannelIdOnString = "749718492781215757".ToDiscordChannel(),
                           supportChannelString = "support".ToDiscordChannel();
            DiscordGuild luizStudiosGuildId = 749718492781215754.ToDiscordGuild(),
                         luizStudiosGuildIdOnString = "749718492781215754".ToDiscordGuild(),
                         luizStudiosGuildString = "Luiz Studios".ToDiscordGuild();
            DiscordRole libraryRoleId = 749722451151290519.ToDiscordRole(),
                        libraryRoleIdOnString = "749722451151290519".ToDiscordRole(),
                        libraryRoleString = "Tars Library".ToDiscordRole();
            DiscordEmoji lulEmojiId = 749745553025269767.ToDiscordEmoji(),
                         lulEmojiIdOnString = "749745553025269767".ToDiscordEmoji(),
                         lulEmojiString = "lul".ToDiscordEmoji();

            Assert.IsNotNull(luizMemberId);
            Assert.IsNotNull(luizMemberIdOnString);
            Assert.IsNotNull(luizMemberString);
            Assert.IsNotNull(supportChannelId);
            Assert.IsNotNull(supportChannelIdOnString);
            Assert.IsNotNull(supportChannelString);
            Assert.IsNotNull(luizStudiosGuildId);
            Assert.IsNotNull(luizStudiosGuildIdOnString);
            Assert.IsNotNull(luizStudiosGuildString);
            Assert.IsNotNull(libraryRoleId);
            Assert.IsNotNull(libraryRoleIdOnString);
            Assert.IsNotNull(libraryRoleString);
            Assert.IsNotNull(lulEmojiId);
            Assert.IsNotNull(lulEmojiIdOnString);
            Assert.IsNotNull(lulEmojiString);
        }
    }
}