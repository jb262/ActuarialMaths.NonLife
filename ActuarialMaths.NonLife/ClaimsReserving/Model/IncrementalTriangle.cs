using System.Collections.Generic;
using System.Linq;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// An incremental run-off triangle based on an abstract base run-off triangle.
    /// </summary>
    public class IncrementalTriangle : Triangle
    {
        /// <summary>
        /// Constructor for an empty run-off triangle.
        /// </summary>
        public IncrementalTriangle() : base() { }

        /// <summary>
        /// Constructor for a run-off triangle containing only zeroes for a given number of periods.
        /// </summary>
        public IncrementalTriangle(int periods) : base(periods) { }

        /// <summary>
        /// Constructor for an icremental run-off triangle from a given cumulative triangle.
        /// </summary>
        /// <param name="cumul">Cumulative triangle from which the incremental triangle is to be created from.</param>
        public IncrementalTriangle(CumulativeTriangle cumul) : base(cumul.Periods)
        {
            SetColumn(cumul.GetColumn(0), 0);
            for (int i = 0; i < Periods - 1; i++)
            {
                SetColumn(cumul.GetColumn(i + 1).Zip(cumul.GetColumn(i), (x, y) => x - y), i + 1);
            }
        }

        /// <summary>
        /// Converts the incremental triangle into a cumulative triangle.
        /// </summary>
        /// <returns>Cumulative run-off triangle based on this incremental triangle.</returns>
        public CumulativeTriangle ToCumulativeTriangle()
        {
            return new CumulativeTriangle(this);
        }

        /// <summary>
        /// Adds a sequence of claims to the run-off triangle.
        /// </summary>
        /// <param name="values">Claims to be appended to the triangle.</param>
        /// <remarks>
        /// It is assumed that the claims to be added are amounts paid in the corresponding period,
        /// i.e. the claims are appended to the triangle as they are passed to the method.
        /// It is assumed that claims to be added are all claims paid in a fixed year. They are ordered
        /// by the relative settlement period in ascending order, e.g.: There are three claims to be added for the year 2019. They are ordered as follows:
        /// 1. Claim occured in 2019 and was paid in 2019 (relative settlement year = 0)
        /// 2. Claim occured in 2018 and was paid in 2019 (relative settlement year = 1)
        /// 3. Claim occured in 2017 and was paid in 2019 (relative settlement year = 2)
        /// </remarks>
        public override void AddClaims(IEnumerable<decimal> values)
        {
            base.AddClaims(values);

            int row = Periods - 1;
            int column = 0;

            foreach (decimal value in values)
            {
                _claims[row][column] = value;
                row--;
                column++;
            }
        }
    }
}
