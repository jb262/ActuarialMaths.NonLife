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

        /// <summary>
        /// Overloaded operator for adding two tariff cells.
        /// </summary>
        /// <param name="cell">First summand.</param>
        /// <param name="otherCell">Second summand.</param>
        /// <returns>Sum of two tariff cells, i.e. the sums of the claims amounts and policy counts.</returns>
        public static TariffCell operator + (TariffCell cell, TariffCell otherCell)
        {
            decimal amount = cell.ClaimsAmount + otherCell.ClaimsAmount;
            int count = cell.PolicyCount + otherCell.PolicyCount;

            return new TariffCell(amount, count);
        }

        /// <summary>
        /// Overloaded operator for substracting two tariff cells.
        /// </summary>
        /// <param name="cell">Minuend.</param>
        /// <param name="otherCell">Subtrahend.</param>
        /// <returns>The difference between two tariff cells, i.e. the differences bewteen their claims amounts and policy counts.</returns>
        public static TariffCell operator - (TariffCell cell, TariffCell otherCell)
        {
            decimal amount = cell.ClaimsAmount - otherCell.ClaimsAmount;
            int count = cell.PolicyCount - otherCell.PolicyCount;

            return new TariffCell(amount, count);
        }
    }
}
