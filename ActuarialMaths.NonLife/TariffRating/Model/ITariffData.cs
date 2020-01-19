using System.Collections.Generic;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// The data that forms the base for the tariff rating..
    /// </summary>
    public interface ITariffData : IEnumerable<KeyValuePair<ITariffKey, TariffCell>>
    {
        /// <summary>
        /// Attributes that describe the tariff groups.
        /// </summary>
        IReadOnlyCollection<ITariffAttribute> Attributes { get; }

        /// <summary>
        /// Keys describing a tariff group by a set of valid values of each attribute of the tariff.
        /// </summary>
        IEnumerable<ITariffKey> TariffKeys { get; }

        /// <summary>
        /// Cells holding the claims amount and policy count for each tariff group.
        /// </summary>
        IEnumerable<TariffCell> TariffCells { get; }

        /// <summary>
        /// Indexer for the tariff cell containing the information for the tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Collection of attribute values to describe the tariff group.</param>
        /// <returns>TariffCell object that contains the claims amount and the policy count of the tariff group specified by the given key.</returns>
        TariffCell this[IEnumerable<TariffAttributeValue> attributeValues] { get; set; }

        /// <summary>
        /// Indexer for all tariff cells linked to the fixed value of one attribute.
        /// </summary>
        /// <param name="attributeValue">Valid value for one attribute describing the model.</param>
        /// <returns>All tariff cells whose keys contain the given attribute value.</returns>
        IEnumerable<TariffCell> this[TariffAttributeValue attributeValue] { get; }

        /// <summary>
        /// Adds a tariff cell containing the claims amount and the policy count for a tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Key to describe the tariff group.</param>
        /// <param name="amount">Claims amount of the tariff group.</param>
        /// <param name="count">Policy count of the tariff group.</param>
        void Add(IEnumerable<TariffAttributeValue> attributeValues, decimal amount, int count);

        /// <summary>
        /// Removes a tariff cell containing the claims amount and the policy count for a tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Key to describe the tariff group.</param>
        void Remove(IEnumerable<TariffAttributeValue> attributeValues);

        /// <summary>
        /// Retrieves the average expected claims expenditure over all tariff groups in the model. 
        /// </summary>
        /// <returns>Average expected claims expenditure over all tariff groups in the model</returns>
        decimal ExpectedClaimsExpenditure();
    }
}
