namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// An immutable run-off square that allows read operations only.
    /// </summary>
    public interface IReadOnlySquare : ISliceable<decimal>
    {
        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        int Periods { get; }
    }
}
