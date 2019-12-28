using System.Collections.Generic;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.Methods
{
    /// <summary>
    /// Interface to model a method for actuarial claims reserving.
    /// </summary>
    public interface IReservingMethod
    {
        /// <summary>
        /// Run-off triangle the method uses to project claims.
        /// </summary>
        Triangle Triangle { get; }

        /// <summary>
        /// Provides the method's claims projection as a "run-off square".
        /// </summary>
        /// <returns>"Run-off square" containg the projected claims.</returns>
        Square Projection();

        /// <summary>
        /// Calculates the total reserve according to the chosen model.
        /// </summary>
        /// <returns>The total reserve according to the chosen model.</returns>
        decimal TotalReserve();

        /// <summary>
        /// Provides the calculated reserves for a given period according to the chosen model.
        /// </summary>
        /// <param name="period">Period for which the reserve is to be returned.</param>
        /// <returns>The calculated reserves for the given period according to the chosen model.</returns>
        decimal Reserve(int period);

        /// <summary>
        /// Provides the calculated reserves for each period according to the chosen model.
        /// </summary>
        /// <returns>The calculated reserves for each period according to the chosen model.</returns>
        IEnumerable<decimal> Reserves();

        /// <summary>
        /// Provides the calculated cashflows for a given period according to the chosen model.
        /// </summary>
        /// <returns>The calculated cashflows for each period according to the chosen model.</returns>
        IReadOnlyList<decimal> Cashflows();
    }
}