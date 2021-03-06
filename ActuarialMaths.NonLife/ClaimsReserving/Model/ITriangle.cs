﻿using System.Collections.Generic;

namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface for an editable run-off triangle.
    /// </summary>
    public interface ITriangle : IWriteableSliceable<decimal>, IReadOnlyTriangle
    {
        /// <summary>
        /// Adds a sequence of claims to the run-off triangle.
        /// </summary>
        /// <param name="values">Claims to be appended to the triangle.</param>
        void AddClaims(IEnumerable<decimal> values);

        /// <summary>
        /// Shifts the claims partially to the future by a given factor.
        /// </summary>
        /// <param name="shiftFactor">Factor betwen 0 and 1 the claims are to be partially shifted by.</param>
        /// <returns>The triangle shifted by the given factor.</returns>
        ITriangle Shift(decimal shiftFactor);

        /// <summary>
        /// Creates a read only copy of the original run off triangle.
        /// </summary>
        /// <returns>A read only copy of the run off triangle.</returns>
        IReadOnlyTriangle AsReadOnly();

        /// <summary>
        /// Extrapolates the main diagonal by multiplaying its values with a given factor.
        /// </summary>
        /// <param name="factor">The factor the main diagonal is to be multiplied with.</param>
        void ExtrapolateDiagonal(decimal factor);
    }
}
