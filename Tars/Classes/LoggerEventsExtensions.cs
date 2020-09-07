using Microsoft.Extensions.Logging;
using Tars.Core;

namespace Tars.Classes
{
    /// <summary>
    /// Contains well-defined event IDs used by core of <see cref="TarsBase"/>.
    /// </summary>
    public static class LoggerEventsExtensions
    {
        /// <summary>
        /// Events that have been successfully completed.
        /// </summary>
        public static EventId Success { get; } = new EventId(124, "Success");

        /// <summary>
        /// Events that are trying something.
        /// </summary>
        public static EventId Trying { get; } = new EventId(125, "Trying");
    }
}