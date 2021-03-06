﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// The Bornhuetter-Ferguson method for claims reserving.
    /// </summary>
    public class BornhuetterFerguson : FactorBasedMethod
    {
        /// <summary>
        /// Expected final claim levels according to the Bornhuetter-Ferguson method.
        /// </summary>
        private readonly IEnumerable<decimal> _alpha;

        /// <summary>
        /// Constructor of a Bornhuetter-Ferguson model given a  run-off triangle.
        /// </summary>
        /// <param name="triangle">Triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <param name="alpha">Ex-ante expected final claim levels.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the count of factors does not match the number of periods observed.</exception>
        /// <exception cref="DimensionMismatchException">Thrown when the number of expected final claim levels does not match the number of periods observed.</exception>
        public BornhuetterFerguson(ITriangle triangle, IEnumerable<decimal> factors, IEnumerable<decimal> alpha) : base(TriangleConverter<CumulativeTriangle>.Convert(triangle))
        {
            int factorsCount = factors.Count();

            if (factorsCount != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, factorsCount);
            }

            int alphaCount = alpha.Count();

            if (alphaCount != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, alphaCount);
            }

            _factors = new Lazy<IReadOnlyList<decimal>>(() => factors.ToList().AsReadOnly());
            _alpha = alpha.ToArray();
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a run-off square according to the Bornhuetter-Ferguson method.
        /// </summary>
        /// <returns>The projected run-off square according to the Bornhuetter-Ferguson method.</returns>
        protected override IReadOnlySquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);

            calc.InitFromTriangle(Triangle);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> diagonal =
                    Factors
                    .Skip(i + 1)
                    .Zip(Factors.Take(Factors.Count - i - 1), (x, y) => x - y)
                    .Zip(_alpha.Skip(i + 1).Reverse(), (x, y) => x * y)
                    .Zip(Triangle.GetDiagonal().Take(calc.Periods - i - 1), (x, y) => x + y);

                calc.SetDiagonal(diagonal, calc.Periods + i);
            }

            return calc.AsReadOnly();
        }

        /// <summary>
        /// Creates a string representation of the current Bornhuetter-Ferguson model.
        /// </summary>
        /// <returns>String representation of the current model.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Claims reserving - Bornhuetter-Ferguson method");
            sb.Append("\n--------------------\n");
            sb.Append("Alpha:\t");
            int n = _alpha.Count();
            int ctr = 0;

            foreach (decimal val in _alpha)
            {
                sb.Append(val.ToString("0.00"));

                if (++ctr != n)
                {
                    sb.Append("\t");
                }
            }

            return sb.ToString() + base.ToString();
        }
    }
}
