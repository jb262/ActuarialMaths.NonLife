using System.Collections.Generic;
using System.Linq;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// A cumulative run-off triangle based on an abstract base run-off triangle.
    /// </summary>
    public class CumulativeTriangle : Triangle
    {
        /// <summary>
        /// Constructor for an empty run-off triangle.
        /// </summary>
        public CumulativeTriangle() : base() { }

        /// <summary>
        /// Constructor for a run-off triangle containing only zeroes for a given number of periods.
        /// </summary>
        public CumulativeTriangle(int periods) : base(periods) { }

        /// <summary>
        /// Adds a sequence of claims to the run-off triangle.
        /// </summary>
        /// <param name="values">Claims to be appended to the triangle.</param>
        /// <remarks>
        /// It is assumed that the claims to be added are amounts paid in the corresponding period,
        /// i.e. for the cumulative triangle the claims are added to the claims of the previous period before being appended to the triangle.
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

            foreach (decimal val in values)
            {
                if (column == 0)
                {
                    _claims[row][column] = val;
                }
                else
                {
                    _claims[row][column] = _claims[row][column - 1] + val;
                }

                row--;
                column++;
            }
        }
    }
}
