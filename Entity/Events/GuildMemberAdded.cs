using DSharpPlus;
using DSharpPlus.EventArgs;
using Entity.Base.Core.Settings;
using Entity.Base.Events.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Base.Events
{
    public delegate Task GuildMemberAddedEventArgs(GuildMemberAddEventArgs e);
    
    public sealed class GuildMemberAdded : IEvent
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActived { get; private set; } = false;

        /// <summary>
        /// Fired when a new user joins a guild.
        /// </summary>
        public GuildMemberAddedEventArgs Delegate { get; set; } = null;

        private DiscordClient _discordClient;

        public void Activate(EntityBaseEventsConfiguration entityBaseEventsConfiguration, DiscordClient discordClient)
        {
            var guildMemberAddedEvent = entityBaseEventsConfiguration.GuildMemberAdded;
            if (guildMemberAddedEvent == null)
                return;

            this.Name = guildMemberAddedEvent.Name;
            this.Description = guildMemberAddedEvent.Description;
            this.Delegate = guildMemberAddedEvent.Delegate;

            this._discordClient = discordClient;

            if (string.IsNullOrWhiteSpace(this.Name))
                this.Name = this.GetType().Name;

            discordClient.GuildMemberAdded += new AsyncEventHandler<GuildMemberAddEventArgs>(this.Delegate);

            if (!this.IsActived)
                this.IsActived = true;
        }

        public void Deactivate()
        {
            this._discordClient.GuildMemberAdded -= new AsyncEventHandler<GuildMemberAddEventArgs>(this.Delegate);

            if (this.IsActived)
                this.IsActived = false;
        }
    }
}