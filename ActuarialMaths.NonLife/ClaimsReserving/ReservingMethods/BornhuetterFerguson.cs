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
        private readonly IEnumerable<decimal> alpha;

        /// <summary>
        /// Constructor of a Bornhuetter-Ferguson model given an incremental run-off triangle.
        /// </summary>
        /// <param name="triangle">Incremental triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <param name="alpha">Expected final claim levels according to the model.</param>
        public BornhuetterFerguson(IncrementalTriangle triangle, IEnumerable<decimal> factors, IEnumerable<decimal> alpha) : this(triangle.ToCumulativeTriangle(), factors, alpha) { }

        /// <summary>
        /// Constructor of a Bornhuetter-Ferguson model given a cumulative run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <param name="alpha">Expected final claim levels according to the model.</param>
        public BornhuetterFerguson(CumulativeTriangle triangle, IEnumerable<decimal> factors, IEnumerable<decimal> alpha) : base(triangle)
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

            this.factors = factors.ToArray();
            this.alpha = alpha.ToArray();
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with according to the Bornhuetter-Ferguson method.
        /// </summary>
        /// <returns>The projected "run-off square" according to the Bornhuetter-Ferguson method.</returns>
        protected override ISquare CalculateProjection()
        {
            ISquare calc = new Square(Triangle.Periods);

            calc.InitFromTriangle(Triangle);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> diagonal =
                    Factors()
                    .Skip(i + 1)
                    .Zip(Factors().Take(Factors().Count - i - 1), (x, y) => x - y)
                    .Zip(alpha.Skip(i + 1).Reverse(), (x, y) => x * y)
                    .Zip(Triangle.GetDiagonal().Take(calc.Periods - i - 1), (x, y) => x + y);

                calc.SetDiagonal(diagonal, calc.Periods + i + 1);
            }

            return calc;
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
            int n = alpha.Count();
            int ctr = 0;

            foreach (decimal val in alpha)
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