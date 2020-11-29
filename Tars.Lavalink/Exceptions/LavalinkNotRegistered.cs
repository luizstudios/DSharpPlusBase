using System;

namespace Tars.Lavalink.Exceptions
{
    /// <summary>
    /// Represents errors which occur when the Lavalink isn't registered as service on Dependency Injection.
    /// </summary>
    public sealed class LavalinkNotRegistered : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LavalinkNotRegistered"/> class.
        /// </summary>
        public LavalinkNotRegistered() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LavalinkNotRegistered"/> class with a specified error message.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public LavalinkNotRegistered(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LavalinkNotRegistered"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        /// <param name="innerException"><inheritdoc/></param>
        public LavalinkNotRegistered(string message, Exception innerException) : base(message, innerException) { }
    }
}