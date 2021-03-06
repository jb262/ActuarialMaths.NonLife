﻿using System;
using System.Collections.Generic;
using System.Linq;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// The widely used chain-ladder method for claims reserving.
    /// </summary>
    public class ChainLadder : FactorBasedMethod
    {
        /// <summary>
        /// Constructor of a chain-ladder model given a run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        public ChainLadder(ITriangle triangle)
            : base(TriangleConverter<CumulativeTriangle>.Convert(triangle))
        {
            _factors = new Lazy<IReadOnlyList<decimal>>(CalculateFactors);
        }

        /// <summary>
        /// Calculates the model's chain-ladder factors.
        /// </summary>
        /// <returns>A read only list containing the chain-ladder factors.</returns>
        private IReadOnlyList<decimal> CalculateFactors()
        {
            decimal[] calc = new decimal[Triangle.Periods - 1];

            for (int i = 0; i < Triangle.Periods - 1; i++)
            {
                decimal denominator = Triangle.GetColumn(i).Take(Triangle.Periods - 1 - i).Sum();
                decimal numerator = Triangle.GetColumn(i + 1).Sum();

                calc[i] = numerator / denominator;
            }

            return Array.AsReadOnly(calc);
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a run-off square with the calculated chain-ladder factors.
        /// </summary>
        /// <returns>The projected run-off square according to the chain-ladder method.</returns>
        protected override IReadOnlySquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);
            calc.SetColumn(Triangle.GetColumn(0), 0);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculatedValues = 
                    calc.GetColumn(i)
                    .Skip(calc.Periods - 1 - i)
                    .Select(x => x * Factors[i]);

                calc.SetColumn(Triangle.GetColumn(i + 1).Concat(calculatedValues), i + 1);
            }

            return calc.AsReadOnly();
        }

        /// <summary>
        /// Creates a string representation of the current chain-ladder model.
        /// </summary>
        /// <returns>String representation of the current chain-ladder model.</returns>
        public override string ToString()
        {
            return "Claims reserving - Chain-ladder method" + base.ToString();
        }
    }
}
