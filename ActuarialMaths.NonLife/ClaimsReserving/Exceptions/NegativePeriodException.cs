﻿using System;

namespace ActuarialMaths.NonLife.ClaimsReserving.Exceptions
{
    /// <summary>
    /// Exception thrown when a negative observation period, which does not exist in this model, is tried to be accessed.
    /// </summary>
    public class NegativePeriodException : Exception
    {
        /// <summary>
        /// Default message shown when no other exception message is given.
        /// </summary>
        private const string defaultMessage = "The year tried to be accessed is smaller than zero.";

        /// <summary>
        /// Basic constructor with the default message.
        /// </summary>
        public NegativePeriodException() : base(defaultMessage) { }

        /// <summary>
        /// Constructor with a given exception message.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        public NegativePeriodException(string message) : base(message) { }

        /// <summary>
        /// Constructor with a given exception message and a given inner exception.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        /// <param name="inner">Inner exception to be thrown.</param>
        public NegativePeriodException(string message, Exception inner) : base(message, inner) { }
    }
}