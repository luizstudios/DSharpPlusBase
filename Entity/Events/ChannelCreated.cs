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
    public delegate Task ChannelCreatedEventArgs(ChannelCreateEventArgs e);

    public sealed class ChannelCreated : IEvent
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActived { get; private set; } = false;

        /// <summary>
        /// Fired when a new channel is created.
        /// </summary>
        public ChannelCreatedEventArgs Delegate { get; set; } = null;

        private DiscordClient _discordClient;

        public void Activate(EntityBaseEventsConfiguration entityBaseEventsConfiguration, DiscordClient discordClient)
        {
            var channelCreatedEvent = entityBaseEventsConfiguration.ChannelCreated;
            if (channelCreatedEvent == null)
                return;

            this.Name = channelCreatedEvent.Name;
            this.Description = channelCreatedEvent.Description;
            this.Delegate = channelCreatedEvent.Delegate;

            this._discordClient = discordClient;

            if (string.IsNullOrWhiteSpace(this.Name))
                this.Name = this.GetType().Name;

            discordClient.ChannelCreated += new AsyncEventHandler<ChannelCreateEventArgs>(this.Delegate);

            if (!this.IsActived)
                this.IsActived = true;
        }

        public void Deactivate()
        {
            this._discordClient.ChannelCreated -= new AsyncEventHandler<ChannelCreateEventArgs>(this.Delegate);

            if (this.IsActived)
                this.IsActived = false;
        }
    }
}
