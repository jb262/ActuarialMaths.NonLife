using System;

namespace ActuarialMaths.NonLife.ClaimsReserving.Exceptions
{
    /// <summary>
    /// Exception thrown when a period that exceeds the observation period is tried to be accessed.
    /// </summary>
    public class ObservationPeriodExceedenceException : Exception
    {
        /// <summary>
        /// Default message shown when no other exception message is given.
        /// </summary>
        private const string defaultMessage = "The period tried to be accessed exceeds the observation period.";

        /// <summary>
        /// Basic constructor with the default message.
        /// </summary>
        public ObservationPeriodExceedenceException() : base(defaultMessage) { }

        /// <summary>
        /// Constructor with a given exception message.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        public ObservationPeriodExceedenceException(string message) : base(message) { }

        /// <summary>
        /// Constructor with a given exception message and a given inner exception.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        /// <param name="inner">Inner exception to be thrown.</param>
        public ObservationPeriodExceedenceException(string message, Exception inner) : base(message, inner) { }
    }
}