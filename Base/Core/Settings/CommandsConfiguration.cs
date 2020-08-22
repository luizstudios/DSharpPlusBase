using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DiscordBotBase.Core.Settings
{
    /// <summary>
    /// Class where CommandsNext settings are set.
    /// </summary>
    public sealed class CommandsConfiguration
    {
        /// <summary>
        /// <para>Sets whether strings should be matched in a case-sensitive manner.</para>
        /// <para>This switch affects the behaviour of default prefix resolver, command searching, and argument conversion.</para>
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool CaseSensitive { get; set; } = false;

        /// <summary>
        /// <para>Sets the default pre-execution checks for the built-in help command.</para>
        /// <para>Only applicable if default help is enabled.</para>
        /// <para>Defaults to null.</para>
        /// </summary>
        public IEnumerable<CheckBaseAttribute> DefaultHelpChecks { get; set; } = null;

        /// <summary>
        /// <para>Controls whether the default help will be sent via DMs or not.</para>
        /// <para>Enabling this will make the bot respond with help via direct messages.</para>
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool DirectMessageHelp { get; set; } = false;

        /// <summary>
        /// <para>Sets whether to enable default help command.</para>
        /// <para>Disabling this will allow you to make your own help command.</para>
        /// <para>
        /// Modifying default help can be achieved via custom help formatters (see <see cref="BaseHelpFormatter"/> and <see cref="CommandsNextExtension.SetHelpFormatter{T}()"/> for more details). 
        /// It is recommended to use help formatter instead of disabling help.
        /// </para>
        /// <para>Defaults to true.</para>
        /// </summary>
        public bool EnableDefaultHelp { get; set; } = true;

        /// <summary>
        /// <para>Sets whether commands sent via direct messages should be processed.</para>
        /// <para>Defaults to true.</para>
        /// </summary>
        public bool EnableDirectMessages { get; set; } = true;

        /// <summary>
        /// <para>Sets whether to allow mentioning the bot to be used as command prefix.</para>
        /// <para>Defaults to true.</para>
        /// </summary>
        public bool EnableMentionPrefix { get; set; } = true;

        /// <summary>
        /// <para>Gets whether any extra arguments passed to commands should be ignored or not. If this is set to false, extra arguments will throw, otherwise they will be ignored.</para>
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool IgnoreExtraArguments { get; set; } = false;

        /// <summary>
        /// <para>Sets the string prefixes used for commands.</para>
        /// <para>Defaults to no value (disabled).</para>
        /// </summary>
        public string[] Prefixes { get; set; } = null;

        /// <summary>
        /// <para>Sets the custom prefix resolver used for commands.</para>
        /// <para>Defaults to none (disabled).</para>
        /// </summary>
        public PrefixResolverDelegate PrefixResolver { get; set; } = null;

        /// <summary>
        /// <para>Sets the service provider for this CommandsNext instance.</para>
        /// <para>Objects in this provider are used when instantiating command modules. This allows passing data around without resorting to static members.</para>
        /// <para>Defaults to null.</para>
        /// </summary>
        public IServiceProvider Services { get; set; } = new ServiceCollection().BuildServiceProvider(true);

        /// <summary>
        /// <para>Gets or sets whether to automatically enable handling commands.</para>
        /// <para>If this is set to false, you will need to manually handle each incoming message and pass it to CommandsNext.</para>
        /// <para>Defaults to true.</para>
        /// </summary>
        public bool UseDefaultCommandHandler { get; set; } = true;
    }
}