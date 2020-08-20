using DSharpPlus;
using Entity.Base.Core.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Base.Events.Entities
{
    public interface IEvent
    {
        /// <summary>
        /// Name of the event. Attention! When this property is not used, the property name is the same name as the class that contains the event name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the event.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Bool that indicates whether the event is activated.
        /// </summary>
        bool IsActived { get; }

        /// <summary>
        /// Method that activates the event.
        /// </summary>
        /// <param name="discordClient">DiscordClient to set the event internally.</param>
        void Activate(DiscordClient discordClient);

        /// <summary>
        /// Method that disables the event.
        /// </summary>
        void Deactivate();
    }
}