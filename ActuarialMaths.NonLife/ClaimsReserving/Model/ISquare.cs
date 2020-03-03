namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Interface for a run-off square.
    /// </summary>
    public interface ISquare : IWriteableSliceable<decimal>, IReadOnlySquare
    {
        /// <summary>
        /// Initializes the values of the square from a triangle.
        /// </summary>
        /// <param name="triangle">Triangle the square is to be initialized from.</param>
        void InitFromTriangle(IReadOnlyTriangle triangle);

        /// <summary>
        /// Creates a read only copy of the run off square.
        /// </summary>
        /// <returns>A read only copy of the original run off square.</returns>
        IReadOnlySquare AsReadOnly();
    }
}
