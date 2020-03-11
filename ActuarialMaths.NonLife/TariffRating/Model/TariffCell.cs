namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// A tariff cell, i.e. the expected/occurred claims amount and expected/sold policy count for a tariff group.
    /// </summary>
    public struct TariffCell
    {
        /// <summary>
        /// Monetary value of the expected/occurred claims.
        /// </summary>
        public readonly decimal ClaimsAmount;

        /// <summary>
        /// Number of policies expected/sold.
        /// </summary>
        public readonly int PolicyCount;

        /// <summary>
        /// Constructor of a tariff cell.
        /// </summary>
        /// <param name="amount">Expected/occurred claims amount.</param>
        /// <param name="count">Expected/occurred policy count.</param>
        public TariffCell(decimal amount, int count)
        {
            ClaimsAmount = amount;
            PolicyCount = count;
        }
    }
}
