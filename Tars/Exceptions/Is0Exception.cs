using System;

namespace Tars.Tars.Exceptions
{
    /// <summary>
    /// Represents errors which occur when a number is 0.
    /// </summary>
    public sealed class Is0Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Is0Exception"/> class.
        /// </summary>
        public Is0Exception() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Is0Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public Is0Exception(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Is0Exception"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        /// <param name="innerException"><inheritdoc/></param>
        public Is0Exception(string message, Exception innerException) : base(message, innerException) { }
    }
}