using System.Collections.Generic;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// Key to describe a tariff group.
    /// </summary>
    public interface ITariffKey : IEnumerable<TariffAttributeValue>
    {
        /// <summary>
        /// Distinct values of each of the model's attributes.
        /// </summary>
        ISet<TariffAttributeValue> TariffAttributeValues { get; }

        /// <summary>
        /// Adds an attribute value to the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be added to the key.</param>
        void Add(TariffAttributeValue tariffAttributeValue);

        /// <summary>
        /// Removes an attribute value from the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be removed.</param>
        void Remove(TariffAttributeValue tariffAttributeValue);

        /// <summary>
        /// Checks whether the given attribute value is part of the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be checked.</param>
        /// <returns>True, if the key contains the attribute value, false otherwise.</returns>
        bool Contains(TariffAttributeValue tariffAttributeValue);

        /// <summary>
        /// Checks whether the key contains a valid valid of the given attribute.
        /// </summary>
        /// <param name="tariffAttribute">Attribute to be checked.</param>
        /// <returns>True, if the key contains a valid value of the given attribute, false otherwise.</returns>
        bool ContainsAttribute(ITariffAttribute tariffAttribute);
    }
}
