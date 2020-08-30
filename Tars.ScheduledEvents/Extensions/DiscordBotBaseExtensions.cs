using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tars.Core;
using Tars.Extensions;
using Tars.ScheduledEvents.Classes;

namespace Tars.ScheduledEvents.Extensions
{
    public static class DiscordBotBaseExtensions
    {
        private static List<Event> _scheduledEvents;

        /// <summary>
        /// Method for configuring scheduled events.
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="events">Instantiate the scheduled events here.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ScheduledEventsSetup(this TarsBotBase botBase, params Event[] events)
        {
            if (_scheduledEvents != null)
                throw new InvalidOperationException("The event list has already been instantiated!");

            _scheduledEvents = new List<Event>();

            var iEvent = 0;

            foreach (Event scheduledEvent in events)
            {
                if (!_scheduledEvents.Any(e => e.Name == scheduledEvent.Name))
                    _scheduledEvents.Add(scheduledEvent);

                ++iEvent;
            }

            botBase.DiscordClient.LogMessage($"{(iEvent > 1 ? $"A total of {iEvent} scheduled events were recorded." : $"A total of {iEvent} scheduled event were recorded.")}", logLevel: LogLevel.Debug);
        }

        /// <summary>
        /// Static method for adding a scheduled event.
        /// </summary>
        /// <param name="scheduledEvents">Instantiate the scheduled events here.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void AddScheduledEvents(this TarsBotBase _, params Event[] scheduledEvents)
        {
            if (scheduledEvents == null)
                throw new NullReferenceException("The scheduled events list can't be null!");

            if (scheduledEvents.Any(e => e == null))
                throw new NullReferenceException("An event scheduled in the list can't be null!");

            if (_scheduledEvents == null)
                _scheduledEvents = new List<Event>();

            foreach (Event scheduledEvent in scheduledEvents)
            {
                if (!_scheduledEvents.Contains(scheduledEvent))
                    _scheduledEvents.Add(scheduledEvent);
                else
                    throw new InvalidOperationException("This scheduled event already exists on the list!");
            }
        }

        /// <summary>
        /// Static method for removing a scheduled event.
        /// </summary>
        /// <param name="scheduledEvents">The scheduled events you want to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void RemoveScheduledEvents(this TarsBotBase _, params Event[] scheduledEvents)
        {
            if (scheduledEvents == null)
                throw new NullReferenceException("The scheduled events list can't be null!");

            if (scheduledEvents.Any(e => e == null))
                throw new NullReferenceException("The scheduled event can't be null!");

            if (_scheduledEvents == null)
                throw new NullReferenceException("Add an scheduled event before removing!");

            foreach (Event scheduledEvent in scheduledEvents)
            {
                var scheduledEventFind = _scheduledEvents.Find(e => e.Name == scheduledEvent.Name);
                if (scheduledEventFind != null)
                {
                    scheduledEvent.Deactivate();

                    _scheduledEvents.Remove(scheduledEvent);
                }
                else
                    throw new InvalidOperationException("An event was not found in the list of events scheduled to be removed.");
            }
        }

        /// <summary>
        /// Get a list of all scheduled events.
        /// </summary>
        public static IReadOnlyList<Event> GetScheduledEvents(this TarsBotBase _) => _scheduledEvents;
    }
}