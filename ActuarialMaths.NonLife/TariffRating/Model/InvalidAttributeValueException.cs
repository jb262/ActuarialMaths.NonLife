using System;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// Exception thrown when an attribute value that is not a legal value for an attribute is tried to be accessed.
    /// </summary>
    public class InvalidAttributeValueException : Exception
    {
        /// <summary>
        /// Default message shown when no other exception message is given.
        /// </summary>
        private const string _defaultMessage = "The given value is not a legal value for this attribute.";

        /// <summary>
        /// Basic constructor with the default message.
        /// </summary>
        public InvalidAttributeValueException() : base(_defaultMessage) { }

        /// <summary>
        /// Constructor with a given exception message.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        public InvalidAttributeValueException(string message) : base(message) { }

        /// <summary>
        /// Constructor with a given exception message and a given inner exception.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        /// <param name="inner">Inner exception to be thrown.</param>
        public InvalidAttributeValueException(string message, Exception inner) : base(message, inner) { }
    }
}
