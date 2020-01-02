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
        /// Constructor of a loss development claims reserving model given an incremental run-off triangle.
        /// </summary>
        /// <param name="triangle">Incremental triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        public LossDevelopment(IncrementalTriangle triangle, IEnumerable<decimal> factors) : this(triangle.ToCumulativeTriangle(), factors) { }

        /// <summary>
        /// Constructor of a loss development claims reserving model given an cumulative run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        public LossDevelopment(CumulativeTriangle triangle, IEnumerable<decimal> factors) : base(triangle)
        {
            int n = factors.Count();

            if (n != Triangle.Periods)
            {
                throw new DimensionMismatchException(Triangle.Periods, n);
            }

            this.factors = factors.ToArray();
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with the calculated factors of the loss development method.
        /// </summary>
        /// <returns>The projected "run-off square" according to the loss development method.</returns>
        protected override ISquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);
            IEnumerable<decimal> regressingLevels = Triangle.GetDiagonal().Zip(Factors(), (x, y) => x / y).Reverse();
            
            calc.SetColumn(Triangle.GetColumn(0), 0);
            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> calculated = regressingLevels
                    .Skip(i + 1)
                    .Select(x => x * Factors()[Factors().Count - i - 1]);

                calc.SetColumn(Triangle.GetColumn(calc.Periods - i - 1).Union(calculated), calc.Periods - i - 1);
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