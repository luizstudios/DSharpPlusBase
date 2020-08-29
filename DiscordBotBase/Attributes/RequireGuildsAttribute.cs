﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotBase.Attributes
{
    /// <summary>
    /// Defines whether the command is restricted to these servers only.
    /// </summary>
    public sealed class RequireGuildsAttribute : CheckBaseAttribute
    {
        private readonly ulong[] _serversId;

        /// <summary>
        /// Defines whether the command is restricted to these servers only.
        /// </summary>
        /// <param name="guildsId">Servers id.</param>
        public RequireGuildsAttribute(params ulong[] guildsId) => this._serversId = guildsId;

        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
            => !this._serversId.Any() ? throw new InvalidOperationException("Put the id of a server!") : await Task.FromResult(this._serversId.Contains(ctx.Guild.Id));
    }
}