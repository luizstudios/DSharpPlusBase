<p align="center">
<a href="https://discord.gg/Z9BcKua">
	<img src="https://cdn.discordapp.com/attachments/478612177511645212/846806601272786954/TarsLogoGitHubWithoutBlank.png"/>
</a>

<a href="https://ci.appveyor.com/project/luizfernandonb/tars">
	<img src="https://ci.appveyor.com/api/projects/status/3683mub26sthphjd?svg=true" />	
</a>
<a href="https://www.codacy.com/gh/luizstudios/Tars?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=luizstudios/Tars&amp;utm_campaign=Badge_Grade">
	<img src="https://app.codacy.com/project/badge/Grade/158b9c525e2a48a88aef91c2d5448aa8"/>
</a>
<a href="https://discord.gg/Z9BcKua">
	<img alt="Discord" src="https://img.shields.io/discord/749718492781215754.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2">
</a>
<a href="https://www.nuget.org/packages?packagetype=&sortby=relevance&q=Tars&prerel=false">
	<img src="https://img.shields.io/nuget/vpre/Tars.svg">
</a>
</p>

A simple library to facilitate the construction of bots for Discord using the [C#](https://github.com/dotnet/csharplang) language, [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) library and [.NET Standard 2.0](https://github.com/dotnet/standard).

# Installing

- Install the library through [NuGet](https://www.nuget.org/packages?packagetype=&sortby=relevance&q=Tars&prerel=false), if you only wanted the basics to start the bot, download only the "Tars" package.

# Getting Started

- Create a project in Visual Studio, preferably in .NET Core, version 2.0 or later.
- Change the *Main* method from __void__ to __async Task__.
- If red lines appear in the code, click on it and press ```Ctrl``` + ```.``` and add the missing usings, do it the other times you need.
- In your bot's main, just write this:
```C#
static async Task Main(string[] args)
{
    var botBase = new TarsBase(Assembly.GetEntryAssembly());
    botBase.DiscordSetup("Your bot's token");
    botBase.CommandsSetup(new string[] { "A prefix of your choice" });

    await botBase.StartAsync();
}
```
- If you start the bot by another class and methods that are not static, you can put the bot class as a service, to access it via commands, this is how it is added:
```C#
botBase.CommandsSetup(new string[] { "A prefix of your choice" }, services: new ServiceCollection().AddSingleton(this));
```
- And ready! If everything goes as expected, your bot will go online :)

- This tutorial is just the basics to create a bot, to see more Tars functions, [click here](https://github.com/luizstudios/Tars/wiki).

# Doubts? Questions?

- Open an issue or enter our Discord:

    [![Tars Chat](https://discord.com/api/guilds/749718492781215754/embed.png?style=banner2)](https://discord.gg/Z9BcKua)

# Credits

- [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)