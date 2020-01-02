using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// The additive method for claims reserving.
    /// </summary>
    public class AdditiveMethod : FactorBasedMethod
    {
        /// <summary>
        /// Premiums earned for each period ordered by accident year in ascending order.
        /// </summary>
        private readonly IEnumerable<decimal> premiums;

        /// <summary>
        /// Constructor of an additive claims reserving model given an incremental run-off triangle.
        /// </summary>
        /// <param name="triangle">Incremental triangle to be developed.</param>
        /// <param name="premiums">Premiums earned ordered by accident year in ascending order.</param>
        public AdditiveMethod(IncrementalTriangle triangle, IEnumerable<decimal> premiums) : base(triangle)
        {
            int n = premiums.Count();

            if (n != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, n);
            }
            this.premiums = premiums;
        }

        /// <summary>
        /// Constructor of an additive claims reserving model given a cumulative run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        /// <param name="premiums">Premiums earned ordered by accident year in ascending order.</param>
        public AdditiveMethod(CumulativeTriangle triangle, IEnumerable<decimal> premiums) : this(triangle.ToIncrementalTriangle(), premiums) { }

        /// <summary>
        /// Provides the method's underlying development factors.
        /// </summary>
        /// <returns>Read-only list of the method's underlying factors.</returns>
        public override IReadOnlyList<decimal> Factors()
        {
            if (factors == null)
            {
                factors = CalculateFactors();
            }

            return Array.AsReadOnly(factors);
        }

        /// <summary>
        /// Calculates the model's development factors.
        /// </summary>
        /// <returns>An array containing the factors of the additive method.</returns>
        private decimal[] CalculateFactors()
        {
            decimal[] calc = new decimal[Triangle.Periods];

            for (int i = 0; i < Triangle.Periods; i++)
            {
                calc[i] = Triangle.GetColumn(i).Sum() / premiums.Take(Triangle.Periods - i).Sum();
            }

            return calc;
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with the calculated factors of the additive method.
        /// </summary>
        /// <returns>The projected "run-off square" according to the additive method.</returns>
        protected override ISquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);
            ITriangle cumul = new CumulativeTriangle((IncrementalTriangle)Triangle);

            calc.SetColumn(Triangle.GetColumn(0), 0);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculatedValues = premiums
                    .Skip(calc.Periods - i - 1)
                    .Select(x => Factors()[i + 1] * x)
                    .Zip(calc.GetColumn(i).Skip(calc.Periods - i - 1), (x, y) => x + y);

                calc.SetColumn(cumul.GetColumn(i + 1).Union(calculatedValues), i + 1);
            }

            return calc;
        }

        /// <summary>
        /// Creates a string representation of the current additive model.
        /// </summary>
        /// <returns>String representation of the current additive model.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Claims reserving - Additive method");
            sb.Append("\n--------------------\n");
            sb.Append("Premiums:\t");
            int n = premiums.Count();
            int ctr = 0;

            foreach (decimal premium in premiums)
            {
                sb.Append(premium.ToString("0.00"));

                if (++ctr != n)
                {
                    sb.Append("\t");
                }
            }

            return sb.ToString() + base.ToString();
        }
    }
}