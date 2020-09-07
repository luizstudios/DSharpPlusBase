using System;

namespace Tars.Exceptions
{
    /// <summary>
    /// Represents errors which occur when a element is null on array or list.
    /// </summary>
    public sealed class ElementIsNullException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementIsNullException"/> class.
        /// </summary>
        public ElementIsNullException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementIsNullException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        public ElementIsNullException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementIsNullException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message"><inheritdoc/></param>
        /// <param name="innerException"><inheritdoc/></param>
        public ElementIsNullException(string message, Exception innerException) : base(message, innerException) { }
    }
}