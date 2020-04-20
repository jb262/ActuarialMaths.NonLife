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
    }
}
