using System.Collections.Generic;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// An attribute that describes the tariff classes.
    /// </summary>
    public interface ITariffAttribute : IEnumerable<TariffAttributeValue>
    {
        /// <summary>
        /// Description of the attribute.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Possible values of the attribute.
        /// </summary>
        IReadOnlyCollection<TariffAttributeValue> Values { get; }

        /// <summary>
        /// Indexer to retrieve a valid value of the attribute.
        /// </summary>
        /// <param name="val">Description of the attribute value as plain text.</param>
        /// <returns>An instance of the TariffAttributeValue struct representing a valid value for this attribute.</returns>
        TariffAttributeValue this[string val] { get; }
    }
}
