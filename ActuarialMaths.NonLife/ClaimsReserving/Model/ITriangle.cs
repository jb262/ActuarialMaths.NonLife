using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface for a run-off triangle.
    /// </summary>
    public interface ITriangle : ISliceable<decimal>
    {
        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        int Periods { get; }

        /// <summary>
        /// Adds a sequence of claims to the run-off triangle.
        /// </summary>
        /// <param name="values">Claims to be appended to the triangle.</param>
        void AddClaims(IEnumerable<decimal> values);
    }
}