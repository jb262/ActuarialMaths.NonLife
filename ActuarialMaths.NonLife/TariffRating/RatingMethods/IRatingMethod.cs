using System.Collections.Generic;
using ActuarialMaths.NonLife.TariffRating.Model;

namespace ActuarialMaths.NonLife.TariffRating.RatingMethods
{
    /// <summary>
    /// A method for rating non-life insurance tariffs.
    /// </summary>
    public interface IRatingMethod : IReadOnlyCollection<KeyValuePair<ITariffKey, decimal>>
    {
        /// <summary>
        /// The base tariff for a tariff group described by a combination of attribute values according to the chosen method.
        /// </summary>
        /// <param name="key">Key to identify the tariff group.</param>
        /// <returns>The specified group's base tariff.</returns>
        decimal this[IEnumerable<TariffAttributeValue> key] { get; }

        /// <summary>
        /// The method's underlying tariff data.
        /// </summary>
        ITariffData TariffData { get; }

        /// <summary>
        /// The mapping of each tariff group to their resepctive base tariff.
        /// </summary>
        /// <returns>A read only dictionary which maps tariff groups to their base tariff according to the chosen method.</returns>
        IReadOnlyDictionary<ITariffKey, decimal> Tariffs();
    }
}
