using Tars.Tests.Services.Lavalink;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tars.Core;
using Tars.Lavalink.Extensions;
using Tars.Tests.Core.Settings;

namespace Tars.Tests.Core
{
    public sealed class TarsTest
    {
        /// <summary>
        /// Constructor of Tars.Test.
        /// </summary>
        public TarsTest() => this.InitializeAsync().GetAwaiter().GetResult();

        /// <summary>
        /// Method where the bot is instatiated and initialized.
        /// </summary>
        public async Task InitializeAsync()
        {
            var botBase = new TarsBase(this);
            botBase.DiscordSetup((await this.GetOrCreateJsonConfig()).Token);
            botBase.LavalinkSetup();
            botBase.CommandsSetup(new string[] { "tars" }, services: new ServiceCollection().AddSingleton(this).AddSingleton(new MusicService(botBase.GetLavalink(), botBase.Discord)));
            await botBase.StartAsync();
        }

        /// <summary>
        /// Method where the json which contains the config of bot is instantied.
        /// </summary>
        /// <returns>The <see cref="BotConfig"/>.</returns>
        public async Task<BotConfig> GetOrCreateJsonConfig()
        {
            string jsonPath = Directory.GetCurrentDirectory() + @"\Config.json";
            if (!File.Exists(jsonPath))
            {
                using (StreamWriter sw = new StreamWriter(File.Create(jsonPath), Encoding.UTF8))
                    await sw.WriteLineAsync(JsonConvert.SerializeObject(new BotConfig(), Formatting.Indented));

                Console.WriteLine("The json has been created, put your token of your bot on Config.json and enjoy.\nClosing the bot in 30 seconds...");

                Thread.Sleep(TimeSpan.FromSeconds(30));

                Environment.Exit(0);
            }

            return JsonConvert.DeserializeObject<BotConfig>(await File.ReadAllTextAsync(jsonPath));
        }
    }
}