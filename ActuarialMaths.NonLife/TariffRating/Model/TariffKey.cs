using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// Standard implementation of a tariff key.
    /// </summary>
    public class TariffKey : ITariffKey
    {
        /// <summary>
        /// Distinct values of each of the model's attributes.
        /// </summary>
        public ISet<TariffAttributeValue> TariffAttributeValues { get; }

        /// <summary>
        /// Constructor of an empty tariff key.
        /// </summary>
        public TariffKey() : this(Enumerable.Empty<TariffAttributeValue>()) { }

        /// <summary>
        /// Constructor of a tariff key given some attribute values.
        /// </summary>
        /// <param name="attributeValues">Attribute values the key is to consist of.</param>
        /// <exception cref="InvalidKeyException">Thrown when multiple attribute values are given for the same attribute.</exception>
        public TariffKey(IEnumerable<TariffAttributeValue> attributeValues)
        {
            int distinctAttributeCount = new HashSet<ITariffAttribute>(attributeValues.Select(x => x.Attribute)).Count;
            int attributeValueCount = attributeValues.Count();

            if (distinctAttributeCount != attributeValueCount)
            {
                throw new InvalidKeyException("One or more attributes have more than one unique value.");
            }

            TariffAttributeValues = new HashSet<TariffAttributeValue>(attributeValues);
        }

        /// <summary>
        /// Adds an attribute value to the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be added to the key.</param>
        /// <exception cref="InvalidAttributeValueException">Thrown when the key already holds a valid value for the attribute the added value is associated to.</exception>
        public void Add(TariffAttributeValue tariffAttributeValue)
        {
            if (ContainsAttribute(tariffAttributeValue.Attribute))
            {
                throw new InvalidAttributeValueException("The key already holds a value for this value's associated attribute.");
            }

            TariffAttributeValues.Add(tariffAttributeValue);
        }

        /// <summary>
        /// Removes an attribute value from the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be removed.</param>
        /// <exception cref="InvalidAttributeValueException">Thrown when the key does not hold the value to be removed.</exception>
        public void Remove(TariffAttributeValue tariffAttributeValue)
        {
            if (!TariffAttributeValues.Contains(tariffAttributeValue))
            {
                throw new InvalidAttributeValueException("The key does not hold the value to be removed.");
            }
            TariffAttributeValues.Remove(tariffAttributeValue);
        }

        /// <summary>
        /// Checks whether the given attribute value is part of the key.
        /// </summary>
        /// <param name="tariffAttributeValue">Attribute value to be checked.</param>
        /// <returns>True, if the key contains the attribute value, false otherwise.</returns>
        public bool Contains(TariffAttributeValue tariffAttributeValue)
        {
            return TariffAttributeValues.Contains(tariffAttributeValue);
        }

        /// <summary>
        /// Checks whether the key contains a valid value of the given attribute.
        /// </summary>
        /// <param name="tariffAttribute">Attribute to be checked.</param>
        /// <returns>True, if the key contains a valid value of the given attribute, false otherwise.</returns>
        public bool ContainsAttribute(ITariffAttribute tariffAttribute)
        {
            return TariffAttributeValues.Select(x => x.Attribute).Contains(tariffAttribute);
        }

        /// <summary>
        /// Standard enumerator of the key's attribute values.
        /// </summary>
        /// <returns>Enumerator of the key's attribute values.</returns>
        public IEnumerator<TariffAttributeValue> GetEnumerator()
        {
            return TariffAttributeValues.GetEnumerator();
        }

        /// <summary>
        /// Standard enumerator of the key's attribute values for the IEnumerable interface.
        /// </summary>
        /// <returns>Enumerator of the key's attribute values.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a string representation of the tariff key.
        /// </summary>
        /// <returns>String representation of the tariff key.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;

            sb.Append("{ ");

            foreach (TariffAttributeValue val in TariffAttributeValues)
            {
                sb.Append(val.Value);

                if (++i < TariffAttributeValues.Count)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" }");

            return sb.ToString();
        }

        /// <summary>
        /// Creates an EqualityComparer for tariff keys.
        /// </summary>
        /// <returns>An EqualityComparer for tariff keys.</returns>
        public static IEqualityComparer<ITariffKey> CreateTariffKeyComparer()
        {
            return new TariffKeyComparer();
        }

        /// <summary>
        /// Creates a collection of all possible keys given some tariff attributes.
        /// </summary>
        /// <param name="attributes">The attributes from whose attribute values all possible keys are to be created.</param>
        /// <returns>All possible keys provided by the given tariff attributes.</returns>
        public static IEnumerable<ITariffKey> Combinations(IEnumerable<ITariffAttribute> attributes)
        {
            IEnumerable<ITariffKey> combinations = null;

            foreach (ITariffAttribute attribute in attributes)
            {
                if (combinations == null)
                {
                    combinations = attribute.Select(x => new TariffKey(new List<TariffAttributeValue>() { x }));
                }
                else
                {
                    IList<ITariffKey> tmpCombinations = new List<ITariffKey>();

                    foreach (ITariffKey key in combinations)
                    {
                        foreach (TariffAttributeValue attributeValue in attribute)
                        {
                            ITariffKey tmpKey = new TariffKey(key.TariffAttributeValues.Append(attributeValue));
                            tmpCombinations.Add(tmpKey);
                        }
                    }

                    combinations = tmpCombinations;
                }
            }

            return combinations;
        }
    }
}
