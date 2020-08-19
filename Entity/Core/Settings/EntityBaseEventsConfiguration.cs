using DSharpPlus;
using DSharpPlus.EventArgs;
using Entity.Base.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Base.Core.Settings
{
    public sealed class EntityBaseEventsConfiguration
    {
        public ChannelCreated ChannelCreated { get; set; }
        public GuildMemberAdded GuildMemberAdded { get; set; }
    }
}