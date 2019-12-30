using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface for a "run-off square".
    /// </summary>
    public interface ISquare : ISliceable<decimal>
    {
        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        int Periods { get; }
    }
}