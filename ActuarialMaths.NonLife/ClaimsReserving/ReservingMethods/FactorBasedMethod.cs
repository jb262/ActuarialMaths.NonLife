using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// Abstract base class for factor based claims reserving methods.
    /// </summary>
    public abstract class FactorBasedMethod : IReservingMethod
    {
        /// <summary>
        /// Run-off triangle the method uses to project claims.
        /// </summary>
        public IReadOnlyTriangle Triangle { get; }

        /// <summary>
        /// Factors the run-off triangle is to be developed with.
        /// </summary>
        protected Lazy<IReadOnlyList<decimal>> _factors;

        /// <summary>
        /// The reserves per period accoring to the model.
        /// </summary>
        private IReadOnlyList<decimal> _reserves;

        /// <summary>
        /// The expected cashflows for each period according to the model.
        /// </summary>
        private IReadOnlyList<decimal> _cashflows;

        /// <summary>
        /// The run-off square containing the projected claims for each accident and settlement period.
        /// </summary>
        private Lazy<IReadOnlySquare> _projection;

        /// <summary>
        /// Constructor given a run-off triangle.
        /// </summary>
        /// <param name="triangle">Run-off triangle to be developed by this method.</param>
        protected FactorBasedMethod(IReadOnlyTriangle triangle)
        {
            Triangle = triangle;
            _projection = new Lazy<IReadOnlySquare>(CalculateProjection);
        }

        /// <summary>
        /// Provides the method's underlying development factors.
        /// </summary>
        /// <returns>Read-only list of the method's underlying factors.</returns>
        public IReadOnlyList<decimal> Factors
        {
            get => _factors.Value;
        }

        /// <summary>
        /// Provides the method's claims projection as a run-off square.
        /// </summary>
        /// <returns>Run-off square containg the projected claims.</returns>
        public IReadOnlySquare Projection
        {
            get => _projection.Value;
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a run-off square with the method's underlying factors.
        /// </summary>
        /// <returns>The projected run-off square according to the chosen method.</returns>
        protected abstract IReadOnlySquare CalculateProjection();

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
        public IReadOnlyList<decimal> Reserves()
        {
            if (_reserves == null)
            {
                _reserves = Projection.CalculateReserves().ToList().AsReadOnly();
            }

            return _reserves;
        }

        /// <summary>
        /// Provides the calculated reserves for a given period according to the chosen model.
        /// </summary>
        /// <param name="period">Period for which the reserve is to be returned.</param>
        /// <returns>The calculated reserves for the given period according to the chosen model.</returns>
        public decimal Reserve(int period)
        {
            return Reserves()[period];
        }

        /// <summary>
        /// Provides the calculated cashflows for each period according to the chosen model.
        /// </summary>
        /// <returns>The calculated cashflows for each period according to the chosen model.</returns>
        public IReadOnlyList<decimal> Cashflows()
        {
            if (_cashflows == null)
            {
                _cashflows = Projection.CalculateCashflows().ToList().AsReadOnly();
            }

            return _cashflows;
        }

        /// <summary>
        /// Creates a string representation of the current claims reserving model.
        /// </summary>
        /// <returns>String representation of the current claims reserving model.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\n--------------------\n");
            sb.Append(Projection.ToString());
            sb.Append("\n--------------------\n");
            sb.Append("Factors:\t");

            for (int i = 0; i < Factors.Count; i++)
            {
                sb.Append(Factors[i].ToString("0.00"));

                if (i < Factors.Count - 1)
                {
                    sb.Append("\t");
                }
            }

            sb.Append("\nTotal reserve:\t");
            sb.Append(TotalReserve().ToString("0.00"));

            return sb.ToString();
        }
    }
}
