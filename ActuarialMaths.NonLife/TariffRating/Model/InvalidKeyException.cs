using System;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// Exception thrown when a key that is not a combination of all possible values of the tariff's attributes is tried to be accessed.
    /// </summary>
    public class InvalidKeyException : Exception
    {
        /// <summary>
        /// Default message shown when no other exception message is given.
        /// </summary>
        private const string _defaultMessage = "The given key does not match a combination of the tariff model's possible attribute values.";

        /// <summary>
        /// Basic constructor with the default message.
        /// </summary>
        public InvalidKeyException() : base(_defaultMessage) { }

        /// <summary>
        /// Constructor with a given exception message.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        public InvalidKeyException(string message) : base(message) { }

        /// <summary>
        /// Constructor with a given exception message and a given inner exception.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        /// <param name="inner">Inner exception to be thrown.</param>
        public InvalidKeyException(string message, Exception inner) : base(message, inner) { }
    }
}
