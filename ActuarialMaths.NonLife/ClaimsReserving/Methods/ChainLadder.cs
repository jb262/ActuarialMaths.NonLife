using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.Methods
{
    /// <summary>
    /// The widely used chain-ladder method for claims reserving.
    /// </summary>
    public class ChainLadder : FactorBasedMethod
    {
        /// <summary>
        /// Constructor of a chain-ladder model given an incremental run-off triangle.
        /// The given incremental triangle is converted into a cumulative triangle before it is assigned to the object's Triangle property.
        /// </summary>
        /// <param name="triangle">Incremental triangle to be developed.</param>
        public ChainLadder(IncrementalTriangle triangle) : base(triangle.ToCumulativeTriangle()) { }

        /// <summary>
        /// Constructor of a chain-ladder model given an already cumulated run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        public ChainLadder(CumulativeTriangle triangle) : base(triangle) { }

        /// <summary>
        /// Provides the calculated chain-ladder factors.
        /// </summary>
        /// <returns>Read-only list of the calculated chain-ladder factors.</returns>
        public override IReadOnlyList<decimal> Factors()
        {
            if (factors == null)
            {
                factors = CalculateFactors();
            }

            return Array.AsReadOnly(factors);
        }

        /// <summary>
        /// Calculates the model's chain-ladder factors.
        /// </summary>
        /// <returns>An array containing the chain-ladder factors.</returns>
        private decimal[] CalculateFactors()
        {
            decimal[] calc = new decimal[Triangle.Periods - 1];

            for (int i = 0; i < Triangle.Periods - 1; i++)
            {
                decimal denominator = Triangle.GetColumn(i).Take(Triangle.Periods - 1 - i).Sum();
                decimal numerator = Triangle.GetColumn(i + 1).Sum();

                calc[i] = numerator / denominator;
            }

            return calc;
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with the calculated chain-ladder factors.
        /// </summary>
        /// <returns>The projected "run-off square" according to the chain-ladder method.</returns>
        protected override Square CalculateProjection()
        {
            Square calc = new Square(Triangle.Periods);
            calc.SetColumn(Triangle.GetColumn(0), 0);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculatedValues = calc.GetColumn(i).Skip(calc.Periods - 1 - i).Select(x => x * Factors()[i]);
                calc.SetColumn(Triangle.GetColumn(i + 1).Union(calculatedValues), i + 1);
            }

            return calc;
        }

        /// <summary>
        /// Creates a string representation of the curent chain-ladder model.
        /// </summary>
        /// <returns>String representation of the current chain-ladder model.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Claims reserving - Chain-ladder method");
            sb.Append("\n--------------------\n");
            sb.Append(Projection().ToString());
            sb.Append("\n--------------------\n");
            sb.Append("Factors: ");

            for (int i = 0; i < Factors().Count; i++)
            {
                sb.Append(Factors()[i].ToString("0.00"));

                if (i < Factors().Count - 1)
                {
                    sb.Append("\t");
                }
            }

            sb.Append("\nTotal reserve: ");
            sb.Append(TotalReserve().ToString("0.00"));

            return sb.ToString();
        }
    }
}