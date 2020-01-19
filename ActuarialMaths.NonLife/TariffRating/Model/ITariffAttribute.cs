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
        ICollection<TariffAttributeValue> Values { get; }

        /// <summary>
        /// Indexer to retrieve a valid value of the attribute.
        /// </summary>
        /// <param name="val">Description of the attribute value as plain text.</param>
        /// <returns>An instance of the TariffAttributeValue struct representing a valid value for this attribute.</returns>
        TariffAttributeValue this[string val] { get; }

        /// <summary>
        /// Adds a TariffAttributeValue to the attribute.
        /// </summary>
        /// <param name="attributeValue">String representation of the TariffAttributeVale to be added.</param>
        void Add(string attributeValue);

        /// <summary>
        /// Adds a TariffAttributeValue to the attribute.
        /// </summary>
        /// <param name="attributeValue">TariffAttributeValue to be added.</param>
        void Add(TariffAttributeValue attributeValue);

        /// <summary>
        /// Removes a TariffAttributeValue from the attribute.
        /// </summary>
        /// <param name="attributeValue">String representation of the TariffAttributeValue to be removed.</param>
        void Remove(string attributeValue);

        /// <summary>
        /// Remoces a TaiffAttributeValue from the attribute.
        /// </summary>
        /// <param name="attributeValue">TariffAttributeValue to be removed.</param>
        void Remove(TariffAttributeValue attributeValue);
    }
}
