using System;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Exception thrown when the element count of a source IEnumerable does not match the element count
    /// of a target IEnumerable the source is to be applied to.
    /// </summary>
    public class DimensionMismatchException : Exception
    {
        /// <summary>
        /// Default message shown when no other exception message is given.
        /// </summary>
        private const string defaultMessage = "The number of elements does not match the target's dimension.";
        /// <summary>
        /// Message part shown when the expected element count is to be shown alongside the given element count.
        /// </summary>
        private const string defaultExpectedDim = "Expected: {0}, given: {1}.";

        /// <summary>
        /// Basic constructor with the default message.
        /// </summary>
        public DimensionMismatchException() : base(defaultMessage) { }

        /// <summary>
        /// Constructor with a given exception message.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        public DimensionMismatchException(string message) : base(message) { }

        /// <summary>
        /// Constructor with a given exception message and a given inner exception.
        /// </summary>
        /// <param name="message">Exception message to be shown.</param>
        /// <param name="inner">Inner exception to be thrown.</param>
        public DimensionMismatchException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Constructor to create an exception displaying the excpected and given element count alongside the default message.
        /// </summary>
        /// <param name="expected">Excpected element count.</param>
        /// <param name="given">Given element count.</param>
        public DimensionMismatchException(int expected, int given) : base(string.Format(string.Concat(defaultMessage, " ", defaultExpectedDim), expected, given)) { }
    }
}