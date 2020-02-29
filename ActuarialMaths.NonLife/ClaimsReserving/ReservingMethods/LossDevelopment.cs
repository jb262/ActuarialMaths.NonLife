using System;
using System.Collections.Generic;
using System.Linq;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// The loss development method for claims reserving.
    /// </summary>
    public class LossDevelopment : FactorBasedMethod
    {
        /// <summary>
        /// Constructor of a loss development claims reserving model given a run-off triangle.
        /// </summary>
        /// <param name="triangle">Triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <exception cref="DimensionMismatchException">Thrown when the number of factors does not match the number of periods observed.</exception>
        public LossDevelopment(ITriangle triangle, IEnumerable<decimal> factors) : base(TriangleConverter<CumulativeTriangle>.Convert(triangle))
        {
            int n = factors.Count();

            if (n != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, n);
            }

            _factors = new Lazy<IReadOnlyList<decimal>>(factors.ToList().AsReadOnly);
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with the calculated factors of the loss development method.
        /// </summary>
        /// <returns>The projected "run-off square" according to the loss development method.</returns>
        protected override ISquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);
            IEnumerable<decimal> regressingLevels = Triangle.GetDiagonal()
                .Zip(Factors, (x, y) => x / y)
                .Reverse()
                .ToList(); ;
            
            calc.SetColumn(Triangle.GetColumn(0), 0);
            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculated = regressingLevels
                    .Skip(i + 1)
                    .Select(x => x * Factors[Factors.Count - i - 1]);

                calc.SetColumn(Triangle.GetColumn(calc.Periods - i - 1).Concat(calculated), calc.Periods - i - 1);
            }

            return calc;
        }

        /// <summary>
        /// Creates a string representation of the current loss development model.
        /// </summary>
        /// <returns>String representation of the current loss development model.</returns>
        public override string ToString()
        {
            return "Claims reserving - Loss development method" + base.ToString();
        }
    }
}
