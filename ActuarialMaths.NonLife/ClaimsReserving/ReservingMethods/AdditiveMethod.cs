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
        private readonly IEnumerable<decimal> _premiums;

        private readonly ITriangle _cumulativeTriangle;

        /// <summary>
        /// Constructor of an additive claims reserving model given a run-off triangle.
        /// </summary>
        /// <param name="triangle">Triangle to be developed.</param>
        /// <param name="premiums">Premiums earned ordered by accident year in ascending order.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the count of premia does not match the number of periods observed.</exception>
        public AdditiveMethod(ITriangle triangle, IEnumerable<decimal> premiums) : base(TriangleConverter<IncrementalTriangle>.Convert(triangle))
        {
            int n = premiums.Count();

            if (n != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, n);
            }

            _premiums = premiums;
            _factors = new Lazy<IReadOnlyList<decimal>>(CalculateFactors);
            _cumulativeTriangle = TriangleConverter<CumulativeTriangle>.Convert(triangle);

        }

        /// <summary>
        /// Calculates the model's development factors.
        /// </summary>
        /// <returns>A read only list containing the factors of the additive method.</returns>
        private IReadOnlyList<decimal> CalculateFactors()
        {
            decimal[] calc = new decimal[Triangle.Periods];

            for (int i = 0; i < Triangle.Periods; i++)
            {
                calc[i] = Triangle.GetColumn(i).Sum() / _premiums.Take(Triangle.Periods - i).Sum();
            }

            return Array.AsReadOnly(calc);
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a run-off square with the calculated factors of the additive method.
        /// </summary>
        /// <returns>The projected run-off square according to the additive method.</returns>
        protected override IReadOnlySquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);
            calc.SetColumn(Triangle.GetColumn(0), 0);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculatedValues = _premiums
                    .Skip(calc.Periods - i - 1)
                    .Select(x => Factors[i + 1] * x)
                    .Zip(calc.GetColumn(i).Skip(calc.Periods - i - 1), (x, y) => x + y);

                calc.SetColumn(_cumulativeTriangle.GetColumn(i + 1).Concat(calculatedValues), i + 1);
            }

            return calc.AsReadOnly();
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
            int n = _premiums.Count();
            int ctr = 0;

            foreach (decimal premium in _premiums)
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
