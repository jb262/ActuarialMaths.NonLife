using System.Collections.Generic;
using System.Linq;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// Extension methods to perform calculations on the modelled run-off objects.
    /// The methods were moved from the objects themselves as they only make sense in connection with an already applied reserving method.
    /// </summary>
    public static class Calculations
    {
        // <summary>
        /// Calculates the cashflows for each period according to the chosen model.
        /// </summary>
        /// <param name="square">Square whose cashflows are to be calculated.
        /// <returns>The calculated cashflows for each period according to the chosen model.</returns>
        public static IEnumerable<decimal> CalculateCashflows(this ISquare square)
        {
            for (int i = 0; i < square.Periods - 1; i++)
            {
                int diagonal = square.Periods + i;

                yield return square.GetDiagonal(diagonal)
                    .Take(square.Periods - 1 - i)
                    .Zip(square.GetDiagonal(diagonal - 1), (x, y) => x - y)
                    .Sum();
            }
        }

        /// <summary>
        /// Calculates the reserves for each period according to the chosen model.
        /// </summary>
        /// <returns>The calculated reserves for each period according to the chosen model.</returns>
        public static IEnumerable<decimal> CalculateReserves(this ISquare square)
        {
            return square.GetColumn(square.Periods - 1)
                .Zip(square.GetDiagonal().Reverse(), (x, y) => x - y);
        }
    }
}
