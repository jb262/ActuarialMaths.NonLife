using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActuarialMaths.NonLife.ClaimsReserving.Model;

namespace ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods
{
    /// <summary>
    /// The Cape Cod method for claims reserving.
    /// </summary>
    public class CapeCod : FactorBasedMethod
    {
        /// <summary>
        /// Volume measures needed for the claims development according to the Cape Cod method.
        /// </summary>
        private readonly IEnumerable<decimal> volumeMeasures;

        /// <summary>
        /// Constructor of a Cape Cod model given an incremental run-off triangle.
        /// </summary>
        /// <param name="triangle">Incremental triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <param name="volumeMeasures">Volume measures needed for the claims development according to the model.</param>
        public CapeCod(IncrementalTriangle triangle, IEnumerable<decimal> factors, IEnumerable<decimal> volumeMeasures) : this(triangle.ToCumulativeTriangle(), factors, volumeMeasures) { }

        /// <summary>
        /// Constructor of a Cape Cod model given a cumulative run-off triangle.
        /// </summary>
        /// <param name="triangle">Cumulative triangle to be developed.</param>
        /// <param name="factors">Factors the triangle is to be developed with.</param>
        /// <param name="volumeMeasures">Volume measures needed for the claims development according to the model.</param>
        public CapeCod(CumulativeTriangle triangle, IEnumerable<decimal> factors, IEnumerable<decimal> volumeMeasures) : base(triangle)
        {
            int factorsCount = factors.Count();

            if (factorsCount != triangle.Periods)
            {
                throw new DimensionMismatchException(triangle.Periods, factorsCount);
            }

            int vmCount = volumeMeasures.Count();

            if (vmCount != triangle.Periods)
            {
                throw new DimensionMismatchException(triangle.Periods, vmCount);
            }

            this.factors = factors.ToArray();
            this.volumeMeasures = volumeMeasures;
        }

        /// <summary>
        /// Develops the model's cumulative triangle into a "run-off square" with according to the Cape Cod method.
        /// </summary>
        /// <returns>The projected "run-off square" according to the Cape Cod method.</returns>
        protected override ISquare CalculateProjection()
        {
            decimal kappa = Triangle.GetDiagonal().Sum() / volumeMeasures.Reverse().Zip(factors, (x, y) => x * y).Sum();
            IEnumerable<decimal> newVolumeMeasures = volumeMeasures.Select(x => x * kappa).Reverse();
            IEnumerable<decimal> deltaFactors =
                Factors()
                .Skip(1)
                .Zip(Factors().Take(Factors().Count - 1), (x, y) => x - y);

            ISquare calc = new Square(Triangle.Periods);
            calc.InitFromTriangle(Triangle);

            for (int i = 0; i < calc.Periods - 1; i++)
            {
                IEnumerable<decimal> diagonal =
                    deltaFactors
                    .Skip(i)
                    .Zip(newVolumeMeasures, (x, y) => x * y)
                    .Zip(calc.GetDiagonal(calc.Periods + i - 1).Take(calc.Periods - 1 - i), (x, y) => x + y);

                calc.SetDiagonal(diagonal, calc.Periods + i + 1);
            }

            return calc;
        }

        /// <summary>
        /// Creates a string representation of the current Cape Cod model.
        /// </summary>
        /// <returns>String representation of the current model.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Claims reserving - Cape Cod method");
            sb.Append("\n--------------------\n");
            sb.Append("Volume measures:\t");
            int n = volumeMeasures.Count();
            int ctr = 0;

            foreach (decimal val in volumeMeasures)
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