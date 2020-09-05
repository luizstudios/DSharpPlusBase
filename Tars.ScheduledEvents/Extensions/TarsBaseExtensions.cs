using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tars.Core;
using Tars.Extensions;
using Tars.ScheduledEvents.Classes;

namespace Tars.ScheduledEvents.Extensions
{
    public static class TarsBaseExtensions
    {
        private static ConcurrentDictionary<Event, byte> _scheduledEvents;

        private static void AddScheduledEventAsService(TarsBase botBase)
            => ((IServiceCollection)typeof(TarsBase).GetField("_services", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(botBase))
                .AddSingleton(GetScheduledEvents(botBase));

        /// <summary>
        /// Instantiates the scheduled events class.
        /// </summary>
        /// <param name="botBase"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ScheduledEventsSetup(this TarsBase botBase)
        {
            if (botBase is null)
                throw new NullReferenceException("The Tars can't be null! Instantiate the framework!");

            if (!(_scheduledEvents is null))
                throw new InvalidOperationException("The scheduled events has already been instantiated!");

            _scheduledEvents = new ConcurrentDictionary<Event, byte>();

            AddScheduledEventAsService(botBase);
        }

        /// <summary>
        /// Method for configuring scheduled events.
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="events">Instantiate the scheduled events here.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ScheduledEventsSetup(this TarsBase botBase, params Event[] events)
        {
            if (!(_scheduledEvents is null))
                throw new InvalidOperationException("The event list has already been instantiated!");

            _scheduledEvents = new ConcurrentDictionary<Event, byte>();

            var iEvent = 0;

            foreach (Event scheduledEvent in events)
            {
                if (!_scheduledEvents.Keys.Any(e => e.Name == scheduledEvent.Name))
                    _scheduledEvents.TryAdd(scheduledEvent, 0);

                ++iEvent;
            }

            AddScheduledEventAsService(botBase);

            botBase.Discord.LogMessage($"{(iEvent > 1 ? $"A total of {iEvent} scheduled events were recorded." : $"A total of {iEvent} scheduled event were recorded.")}", logLevel: LogLevel.Debug);
        }

        /// <summary>
        /// Static method for adding a scheduled event.
        /// </summary>
        /// <param name="scheduledEvents">Instantiate the scheduled events here.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void AddScheduledEvents(this TarsBase _, params Event[] scheduledEvents)
        {
            if (scheduledEvents is null)
                throw new NullReferenceException("The scheduled events list can't be null!");

            if (scheduledEvents.Any(e => e is null))
                throw new NullReferenceException("An event scheduled in the list can't be null!");

            if (_scheduledEvents is null)
                _scheduledEvents = new ConcurrentDictionary<Event, byte>();

            foreach (Event scheduledEvent in scheduledEvents)
            {
                if (!_scheduledEvents.Keys.Any(e => e.Name == scheduledEvent.Name))
                    _scheduledEvents.TryAdd(scheduledEvent, 0);
            }
        }

        /// <summary>
        /// Static method for removing a scheduled event.
        /// </summary>
        /// <param name="scheduledEvents">The scheduled events you want to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void RemoveScheduledEvents(this TarsBase _, params Event[] scheduledEvents)
        {
            if (scheduledEvents is null)
                throw new NullReferenceException("The scheduled events list can't be null!");

            if (scheduledEvents.Any(e => e is null))
                throw new NullReferenceException("The scheduled event can't be null!");

            if (_scheduledEvents is null)
                throw new NullReferenceException("Add an scheduled event before removing!");

            foreach (Event scheduledEvent in scheduledEvents)
            {
                var scheduledEventFind = _scheduledEvents.Keys.FirstOrDefault(e => e.Name == scheduledEvent.Name);
                if (!(scheduledEventFind is null))
                {
                    scheduledEvent.Deactivate();

                    _scheduledEvents.TryRemove(scheduledEvent, out byte _);
                }
                else
                    throw new InvalidOperationException("An event was not found in the list of events scheduled to be removed.");
            }
        }

        /// <summary>
        /// Get a list of all scheduled events.
        /// </summary>
        public static IReadOnlyList<Event> GetScheduledEvents(this TarsBase _) => _scheduledEvents.Keys.ToList();
    }
}