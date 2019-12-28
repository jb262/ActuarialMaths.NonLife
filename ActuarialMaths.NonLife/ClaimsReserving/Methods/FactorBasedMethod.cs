using System;
using System.Collections.Generic;
using System.Linq;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.Methods
{
    /// <summary>
    /// Abstract base class for factor based claims reserving methods.
    /// </summary>
    public abstract class FactorBasedMethod : IReservingMethod
    {
        /// <summary>
        /// Run-off triangle the method uses to project claims.
        /// </summary>
        public Triangle Triangle { get; }

        /// <summary>
        /// Factors the run-off triangle is to be developed with.
        /// </summary>
        protected decimal[] factors;

        /// <summary>
        /// The reserves per period accoring to the model.
        /// </summary>
        private decimal[] reserves;

        /// <summary>
        /// The expected cashflows for each period according to the model.
        /// </summary>
        private decimal[] cashflows;

        /// <summary>
        /// The "run-off square" containing the projected claims for each accident and settlement period.
        /// </summary>
        private Square projection;

        /// <summary>
        /// Constructor given a run-off triangle.
        /// </summary>
        /// <param name="triangle">Run-off triangle to be developed by this method.</param>
        protected FactorBasedMethod(Triangle triangle)
        {
            Triangle = triangle;
        }

        /// <summary>
        /// Provides the method's underlying development factors.
        /// </summary>
        /// <returns>Read-only list of the method's underlying factors.</returns>
        public virtual IReadOnlyList<decimal> Factors()
        {
            return Array.AsReadOnly(factors);
        }

        /// <summary>
        /// Provides the method's claims projection as a "run-off square".
        /// </summary>
        /// <returns>"Run-off square" containg the projected claims.</returns>
        public Square Projection()
        {
            if (projection == null)
            {
                projection = CalculateProjection();
            }

            return projection;
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with the method's underlying factors.
        /// </summary>
        /// <returns>The projected "run-off square" according to the chosen method.</returns>
        protected abstract Square CalculateProjection();

        /// <summary>
        /// Calculates the total reserve according to the chosen model.
        /// </summary>
        /// <returns>The total reserve according to the chosen model.</returns>
        public decimal TotalReserve()
        {
            return Reserves().Sum();
        }

        /// <summary>
        /// Provides the calculated reserves for each period according to the chosen model.
        /// </summary>
        /// <returns>The calculated reserves for each period according to the chosen model.</returns>
        public IEnumerable<decimal> Reserves()
        {
            if (reserves == null)
            {
                reserves = Projection().CalculateReserves().ToArray();
            }

            return reserves;
        }

        /// <summary>
        /// Provides the calculated reserves for a given period according to the chosen model.
        /// </summary>
        /// <param name="period">Period for which the reserve is to be returned.</param>
        /// <returns>The calculated reserves for the given period according to the chosen model.</returns>
        public decimal Reserve(int period)
        {
            if (reserves == null)
            {
                reserves = Projection().CalculateReserves().ToArray();
            }

            return reserves[period];
        }

        /// <summary>
        /// Provides the calculated cashflows for a given period according to the chosen model.
        /// </summary>
        /// <returns>The calculated cashflows for each period according to the chosen model.</returns>
        public IReadOnlyList<decimal> Cashflows()
        {
            if (cashflows == null)
            {
                cashflows = Projection().CalculateCashflows().ToArray();
            }

            return cashflows;
        }
    }
}