namespace ActuarialMaths.NonLife.ClaimsReserving.Model
{
    /// <summary>
    /// Basic implementation of a run-off square that allows read-only operations only.
    /// </summary>
    public class ReadOnlySquare : SquareBase, IReadOnlySquare
    {
        /// <summary>
        /// Creates an immutable run-off square from a mutable one.
        /// </summary>
        /// <param name="square">Mutable run-off square that provides the values for the read only square.</param>
        public ReadOnlySquare(ISquare square)
        {
            Periods = square.Periods;

            _claims = new decimal[Periods, Periods];

            for (int i = 0; i < Periods; i++)
            {
                for (int j = 0; j < Periods; j++)
                {
                    _claims[i, j] = square[i, j];
                }
            }
        }
    }
}
