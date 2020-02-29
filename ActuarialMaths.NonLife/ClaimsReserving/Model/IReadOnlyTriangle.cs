namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// An immutable run-off triangle that allows read operations only.
    /// </summary>
    public interface IReadOnlyTriangle : ISliceable<decimal>
    {
        /// <summary>
        /// Number of periods under observation.
        /// </summary>
        int Periods { get; }
    }
}
