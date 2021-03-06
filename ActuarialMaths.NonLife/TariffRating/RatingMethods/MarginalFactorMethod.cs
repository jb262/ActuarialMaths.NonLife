﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ActuarialMaths.NonLife.TariffRating.Model;

namespace ActuarialMaths.NonLife.TariffRating.RatingMethods
{
    /// <summary>
    /// The family of marginal factor methods for non-life tariff rating.
    /// </summary>
    public abstract class MarginalFactorMethod : IRatingMethod
    {
        /// <summary>
        /// The mapping of tariff groups to their base tariff according to the chosen method.
        /// </summary>
        private Lazy<IReadOnlyDictionary<ITariffKey, decimal>> _tariffs;

        /// <summary>
        /// The marginal factors of each tariff group according to the chosen method.
        /// </summary>
        private Lazy<IReadOnlyDictionary<TariffAttributeValue, decimal>> _factors;

        /// <summary>
        /// The method's underlying tariff data.
        /// </summary>
        public ITariffData TariffData { get; }

        /// <summary>
        /// The number of tariffs, respectively tariff groups.
        /// </summary>
        public int Count { get => Tariffs.Count; }

        /// <summary>
        /// The mapping of each tariff group to their resepctive base tariff.
        /// </summary>
        public IReadOnlyDictionary<ITariffKey, decimal> Tariffs { get => _tariffs.Value; }

        /// <summary>
        /// Retrieves the method's marginal factors for each valid tariff attribute value.
        /// </summary>
        /// <returns>The method's marginal factors the tariffs are calculated with.</returns>
        public IReadOnlyDictionary<TariffAttributeValue, decimal> Factors { get => _factors.Value; }

        /// <summary>
        /// The base tariff for a tariff group described by a combination of attribute values according to the chosen method.
        /// </summary>
        /// <param name="key">Key to identify the tariff group.</param>
        /// <returns>The specified group's base tariff.</returns>
        /// <exception cref="InvalidKeyException">Thrown when the underlying tariff data does not contain the specified key.</exception>
        public decimal this[IEnumerable<TariffAttributeValue> key]
        {
            get
            {
                ITariffKey tariffKey = key is ITariffKey ? (ITariffKey)key : new TariffKey(key);

                if (!Tariffs.ContainsKey(tariffKey))
                {
                    throw new InvalidKeyException();
                }

                return Tariffs[tariffKey];
            }
        }

        /// <summary>
        /// Standard constructor for a marginal factor method.
        /// </summary>
        /// <param name="tariffData">The method's underlying tariff data.</param>
        protected MarginalFactorMethod(ITariffData tariffData)
        {
            TariffData = tariffData;

            _tariffs = new Lazy<IReadOnlyDictionary<ITariffKey, decimal>>(CalculateTariffs);
            _factors = new Lazy<IReadOnlyDictionary<TariffAttributeValue, decimal>>(CalculateFactors);
        }

        /// <summary>
        /// Calculates the tariffs for each tariff key given their factors.
        /// </summary>
        /// <returns>A read only dictionary containing the tariffs for each tariff key.</returns>
        private IReadOnlyDictionary<ITariffKey, decimal> CalculateTariffs()
        {
            IDictionary<ITariffKey, decimal> tariffs = new Dictionary<ITariffKey, decimal>();

            foreach (ITariffKey key in TariffData.TariffKeys)
            {
                tariffs[key] =
                    GetFactors(key)
                    .Aggregate(TariffData.ExpectedClaimsExpenditure(), (x, y) => x * y);
            }

            return new ReadOnlyDictionary<ITariffKey, decimal>(tariffs);
        }

        /// <summary>
        /// Retrieves the method's marginal factors for a given valid tariff attribute value.
        /// </summary>
        /// <param name="tariffAttributeValue">The attribute value the marginal factor is to be retrieved.</param>
        /// <returns>The method's marginal factor for a given attribute value the tariffs are calculated with.</returns>
        /// <exception cref="InvalidAttributeValueException">Thrown when there is no factor for the given attribute value.</exception>
        public decimal GetFactor(TariffAttributeValue tariffAttributeValue)
        {
            if (!Factors.ContainsKey(tariffAttributeValue))
            {
                throw new InvalidAttributeValueException("There exists no marginal factor for the given attribute value.");
            }
            return Factors[tariffAttributeValue];
        }

        /// <summary>
        /// Retrieves the method's marginal factors for each attribute value in a given tariff key.
        /// </summary>
        /// <param name="key">Tariff key for whose attribute values the marginal factors are to be retrieved.</param>
        /// <returns>The marginal factors for each attribute value in a given tariff key.</returns>
        public IEnumerable<decimal> GetFactors(ITariffKey key)
        {
            foreach (TariffAttributeValue tariffAttributeValue in key)
            {
                yield return GetFactor(tariffAttributeValue);
            }
        }

        /// <summary>
        /// Calculates the marginal factors according to the chosen model.
        /// </summary>
        /// <returns>The mapping of each of the model's tariff attribute values to their marginal factors.</returns>
        protected abstract IReadOnlyDictionary<TariffAttributeValue, decimal> CalculateFactors();

        /// <summary>
        /// Standard enumerator of the tariff rating method.
        /// </summary>
        /// <returns>Enumerator of the tariff rating method.</returns>
        public IEnumerator<KeyValuePair<ITariffKey, decimal>> GetEnumerator()
        {
            return Tariffs.GetEnumerator();
        }

        /// <summary>
        /// Standard enumerator of the tariff rating method for the IEnumerable interface.
        /// </summary>
        /// <returns>Enumerator of the tariff rating method.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a string representation of the tariff rating method.
        /// </summary>
        /// <returns>String representation of the tariff rating method.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (KeyValuePair<ITariffKey, decimal> tariff in this)
            {                
                sb.Append(tariff.Key.ToString() + ": " + tariff.Value.ToString("0.00"));

                if (++i < Count)
                {
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }
}
