using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Base.Utilities.Attributes
{
    /// <summary>
    /// Defines whether the command is restricted to these servers only.
    /// </summary>
    public sealed class EntityBaseRequireGuildsAttribute : CheckBaseAttribute
    {
        private readonly ulong[] _serversId;

        /// <summary>
        /// Defines whether the command is restricted to these servers only.
        /// </summary>
        /// <param name="guildsId">Servers id.</param>
        public EntityBaseRequireGuildsAttribute(params ulong[] guildsId) => this._serversId = guildsId;

        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) 
            => this._serversId.Count() == 0 ? throw new InvalidOperationException("When using this attribute, put the id of a server!") :
                                              await Task.FromResult(this._serversId.Contains(ctx.Guild.Id));
    }
}