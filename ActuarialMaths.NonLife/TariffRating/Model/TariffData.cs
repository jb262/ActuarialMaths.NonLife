using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    public class TariffData : ITariffData
    {
        private IDictionary<ITariffKey, TariffCell> _cells;
        private ISet<ITariffAttribute> _attributes;
        private IReadOnlyCollection<ITariffAttribute> _attributesCollection;
        private decimal? _expectedClaimsExpenditure;

        public TariffData(IEnumerable<ITariffAttribute> attributes)
        {
            _attributes = new HashSet<ITariffAttribute>(attributes);
            _cells = new Dictionary<ITariffKey, TariffCell>(TariffKey.CreateTariffKeyComparer());

            foreach (ITariffKey key in TariffKey.Combinations(_attributes))
            {
                _cells.Add(key, new TariffCell(0m, 0));
            }
        }

        /// <summary>
        /// Indexer for the tariff cell containing the information for the tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Collection of attribute values to describe the tariff group.</param>
        /// <returns>TariffCell object that contains the claims amount and the policy count of the tariff group specified by the given key.</returns>
        /// <exception cref="InvalidKeyException">Thrown when the given combination of attribute values is not a valid combination for the model's attributes.</exception>
        public TariffCell this[IEnumerable<TariffAttributeValue> attributeValues]
        {
            get
            {
                ITariffKey key = attributeValues is ITariffKey ? (ITariffKey)attributeValues : new TariffKey(attributeValues);

                if (!_cells.ContainsKey(key))
                {
                    throw new InvalidKeyException();
                }

                return _cells[key];
            }

            set
            {
                ITariffKey key = attributeValues is ITariffKey ? (ITariffKey)attributeValues : new TariffKey(attributeValues);

                if (!_cells.ContainsKey(key))
                {
                    throw new InvalidKeyException();
                }

                _cells[key] = value;
            }
        }

        /// <summary>
        /// Indexer for all tariff cells linked to the fixed value of one attribute.
        /// </summary>
        /// <param name="attributeValue">Valid value for one attribute describing the model.</param>
        /// <returns>All tariff cells whose keys contain the given attribute value.</returns>
        /// <exception cref="InvalidAttributeValueException">Thrown when the given attribute value is not valid for any attribute describing the model.</exception>
        public IEnumerable<TariffCell> this[TariffAttributeValue attributeValue]
        {
            get
            {
                if (!_attributes.Contains(attributeValue.Attribute))
                {
                    throw new InvalidAttributeValueException();
                }

                foreach (KeyValuePair<ITariffKey, TariffCell> keyValuePair in _cells)
                {
                    if (keyValuePair.Key.Contains(attributeValue))
                    {
                        yield return _cells[keyValuePair.Key];
                    }
                }
            }
        }

        /// <summary>
        /// Attributes that describe the tariff groups.
        /// </summary>
        public IReadOnlyCollection<ITariffAttribute> Attributes
        {
            get
            {
                if (_attributesCollection == null)
                {
                    _attributesCollection = _attributes.ToList().AsReadOnly();
                }

                return _attributesCollection;
            }
        }

        /// <summary>
        /// Keys describing a tariff group by a set of valid values of each attribute of the tariff.
        /// </summary>
        public IEnumerable<ITariffKey> TariffKeys { get => _cells.Keys; }

        /// <summary>
        /// Cells holding the claims amount and policy count for each tariff group.
        /// </summary>
        public IEnumerable<TariffCell> TariffCells { get => _cells.Values; }

        /// <summary>
        /// Adds a tariff cell containing the claims amount and the policy count for a tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Key to describe the tariff group.</param>
        /// <param name="amount">Claims amount of the tariff group.</param>
        /// <param name="count">Policy count of the tariff group.</param>
        /// <exception cref="InvalidKeyException">Thrown when the given combination of attribute values is not a valid combination for the model's attributes.</exception>
        /// <exception cref="ArgumentException">Thrown when the expected/occurred claims amount given is smaller than zero.</exception>
        /// <exception cref="ArgumentException">Thrown when the expected/sold policies count given is less or equal zero.</exception>
        public void Add(IEnumerable<TariffAttributeValue> attributeValues, decimal amount, int count)
        {
            ITariffKey key = attributeValues is ITariffKey ? (ITariffKey)attributeValues : new TariffKey(attributeValues);

            if (!_cells.ContainsKey(key))
            {
                throw new InvalidKeyException();
            }

            if (amount < 0)
            {
                throw new ArgumentException("The claims amount is smaller than zero.", nameof(amount));
            }

            if (count <= 0)
            {
                throw new ArgumentException("The count of policies expected/sold is zero or less.", nameof(count));
            }

            _cells[key] = new TariffCell(amount, count);
        }

        /// <summary>
        /// Removes a tariff cell containing the claims amount and the policy count for a tariff group described by the given key.
        /// </summary>
        /// <param name="attributeValues">Key to describe the tariff group.</param>
        /// <exception cref="InvalidKeyException">Thrown when the given combination of attribute values is not a valid combination for the model's attributes.</exception>
        public void Remove(IEnumerable<TariffAttributeValue> attributeValues)
        {
            ITariffKey key = attributeValues is ITariffKey ? (ITariffKey)attributeValues : new TariffKey(attributeValues);

            if (!_cells.ContainsKey(key))
            {
                throw new InvalidKeyException();
            }

            _cells[key] = new TariffCell(0m, 0);
        }

        /// <summary>
        /// Retrieves the average expected claims expenditure over all tariff groups in the model. 
        /// </summary>
        /// <returns>Average expected claims expenditure over all tariff groups in the model</returns>
        public decimal ExpectedClaimsExpenditure()
        {
            if (_expectedClaimsExpenditure == null)
            {
                decimal totalClaimsAmount = 0m;
                decimal totalPolicyCount = 0m;
                
                foreach (TariffCell cell in TariffCells)
                {
                    totalClaimsAmount += cell.ClaimsAmount;
                    totalPolicyCount += cell.PolicyCount;
                }

                _expectedClaimsExpenditure = totalClaimsAmount / totalPolicyCount;
            }

            return (decimal)_expectedClaimsExpenditure;
        }

        /// <summary>
        /// Standard enumerator of the tariff-group/tariff-cell key value pairs.
        /// </summary>
        /// <returns>Enumerator of the tariff-group/tariff-cell key value pairs.</returns>
        public IEnumerator<KeyValuePair<ITariffKey, TariffCell>> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        /// <summary>
        /// Standard enumerator of the tariff-group/tariff-cell key value pairs for the IEnumerable interface.
        /// </summary>
        /// <returns>Enumerator of the tariff-group/tariff-cell key value pairs.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a string representation of the tariff data.
        /// </summary>
        /// <returns>String representation of the tariff data with its tariff groups and their claims amount and policy count.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;

            foreach (KeyValuePair<ITariffKey, TariffCell> cell in _cells)
            {
                string claimsAmount = string.Format("Claims amount: {0}", cell.Value.ClaimsAmount.ToString("0.00"));
                sb.Append(cell.Key.ToString() + $": {claimsAmount}, Policy count: {cell.Value.PolicyCount}");

                if (++i < _cells.Count)
                {
                    sb.Append("\n");
                }
            }
                return sb.ToString();
        }
    }
}
