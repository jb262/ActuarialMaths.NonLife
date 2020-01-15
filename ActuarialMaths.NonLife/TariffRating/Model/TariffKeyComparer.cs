using System.Collections.Generic;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// A standard comparer for tariff keys.
    /// </summary>
    public class TariffKeyComparer : IEqualityComparer<ITariffKey>
    {
        /// <summary>
        /// EqualityComparer for the key's underlying set
        /// </summary>
        private readonly IEqualityComparer<HashSet<TariffAttributeValue>> _comparer;

        /// <summary>
        /// Basic constrcutor of the equality comparer.
        /// </summary>
        public TariffKeyComparer()
        {
            _comparer = HashSet<TariffAttributeValue>.CreateSetComparer();
        }

        /// <summary>
        /// Checks two keys for equality by comparing their underlying sets.
        /// </summary>
        /// <param name="key">First key.</param>
        /// <param name="otherKey">Second key the first one is to be compared to.</param>
        /// <returns>True, if the keys are equal, false otherwise.</returns>
        public bool Equals(ITariffKey key, ITariffKey otherKey)
        {
            HashSet<TariffAttributeValue> keySet;
            HashSet<TariffAttributeValue> otherKeySet;

            if (key.TariffAttributeValues is HashSet<TariffAttributeValue>)
            {
                keySet = (HashSet<TariffAttributeValue>)key.TariffAttributeValues;
            }
            else
            {
                keySet = new HashSet<TariffAttributeValue>(key);
            }

            if (otherKey.TariffAttributeValues is HashSet<TariffAttributeValue>)
            {
                otherKeySet = (HashSet<TariffAttributeValue>)otherKey.TariffAttributeValues;
            }
            else
            {
                otherKeySet = new HashSet<TariffAttributeValue>(otherKey);
            }

            return _comparer.Equals(keySet, otherKeySet);
        }

        /// <summary>
        /// Retrieves the hash code of a tariff key.
        /// </summary>
        /// <param name="key">Key whose hash code is to be retrieved.</param>
        /// <returns>Hash code of the key, i.e. the hash code of its underlying set.</returns>
        public int GetHashCode(ITariffKey key)
        {
            HashSet<TariffAttributeValue> keySet;

            if (key.TariffAttributeValues is HashSet<TariffAttributeValue>)
            {
                keySet = (HashSet<TariffAttributeValue>)key.TariffAttributeValues;
            }
            else
            {
                keySet = new HashSet<TariffAttributeValue>(key);
            }

            return _comparer.GetHashCode(keySet);
        }
    }
}
