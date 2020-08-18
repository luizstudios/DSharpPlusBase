using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlusBase.Core.Settings
{
    public sealed class BotBaseCommandsNextConfiguration
    {
        public bool CaseSensitive { get; set; } = false;
        public IEnumerable<CheckBaseAttribute> DefaultHelpChecks { get; set; } = null;
        public bool DirectMessageHelp { get; set; } = false;
        public bool EnableDefaultHelp { get; set; } = true;
        public bool EnableDirectMessages { get; set; } = true;
        public bool EnableMentionPrefix { get; set; } = true;
        public bool IgnoreExtraArguments { get; set; } = false;
        public string[] Prefixes { get; set; } = null;
        public PrefixResolverDelegate PrefixResolver { get; set; } = null;
        public IServiceProvider Services { get; set; } = null;
        public bool UseDefaultCommandHandler { get; set; } = true;
    }
}