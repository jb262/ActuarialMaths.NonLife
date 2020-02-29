using System.Linq;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Basic implementation of a run-off triangle that allows read operations only.
    /// </summary>
    public class ReadOnlyTriangle : TriangleBase
    {
        /// <summary>
        /// Creates an immutable run-off triangle from a given mutable one.
        /// </summary>
        /// <param name="triangle">Run-off triangle that holds the values of the read only triangle to be created.</param>
        public ReadOnlyTriangle(ITriangle triangle)
        {
            Periods = triangle.Periods;
            _claims = new decimal[Periods][];

            for (int i = 0; i < Periods; i++)
            {
                _claims[i] = triangle.GetRow(i).ToArray();
            }
        }
    }
}
