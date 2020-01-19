using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// Standard implementation of a tariff attribute.
    /// </summary>
    public class TariffAttribute : ITariffAttribute
    {
        /// <summary>
        /// Standard constructor of a TariffAttribute.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        public TariffAttribute(string label)
        {
            Label = label;
            Values = new HashSet<TariffAttributeValue>();
        }

        /// <summary>
        /// Constructor of a TariffAttribute providing a collection of valid values.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        /// <param name="attributeValues">Valid values for the attribute.</param>
        /// <exception cref="InvalidAttributeValueException">Thrown when the attribute of one attribute value differs from this attribute.</exception>
        public TariffAttribute(string label, IEnumerable<TariffAttributeValue> attributeValues) : this(label)
        {
            foreach (TariffAttributeValue attributeValue in attributeValues)
            {
                if (attributeValue.Attribute != this)
                {
                    throw new InvalidAttributeValueException("The value's attribute does not match the atribute it is to be added to.");
                }
                Values.Add(attributeValue);
            }
        }

        /// <summary>
        /// Constructor of a TariffAttribute given a collection of valid values.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        /// <param name="attributeValues">Valid values for the attribute as plain text.</param>
        public TariffAttribute(string label, IEnumerable<string> attributeValues) : this(label)
        {
            foreach (string attributeValue in attributeValues)
            {
                Values.Add(new TariffAttributeValue(this, attributeValue));
            }
        }

        /// <summary>
        /// Indexer to retrieve a valid value of the attribute.
        /// </summary>
        /// <param name="val">Description of the attribute value as plain text.</param>
        /// <returns>An instance of the TariffAttributeValue struct representing a valid value for this attribute.</returns>
        /// <exception cref="InvalidAttributeValueException">Thrown when a value that is not legal for this attribute is tried to be accessed.</exception>
        public TariffAttributeValue this[string val]
        {
            get
            {
                TariffAttributeValue attributeValue = new TariffAttributeValue(this, val);

                if (!Values.Contains(attributeValue))
                {
                    throw new InvalidAttributeValueException();
                }

                return attributeValue;
            }
        }

        /// <summary>
        /// Description of the attribute.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Possible values of the attribute.
        /// </summary>
        public ICollection<TariffAttributeValue> Values { get; }

        /// <summary>
        /// Adds a TariffAttributeValue to the attribute.
        /// </summary>
        /// <param name="attributeValue">TariffAttributeValue to be added.</param>
        /// /// <exception cref="InvalidAttributeValueException">Thrown when the attribute of the attribute value differs from this attribute.</exception>
        public void Add(TariffAttributeValue attributeValue)
        {
            if (attributeValue.Attribute != this)
            {
                throw new InvalidAttributeValueException("The value's attribute does not match the atribute it is to be added to.");
            }

            Values.Add(attributeValue);
        }

        /// <summary>
        /// Adds a TariffAttributeValue to the attribute.
        /// </summary>
        /// <param name="attributeValue">String representation of the TariffAttributeVale to be added.</param>
        public void Add(string attributeValue)
        {
            Add(new TariffAttributeValue(this, attributeValue));
        }

        /// <summary>
        /// Remoces a TaiffAttributeValue from the attribute.
        /// </summary>
        /// <param name="attributeValue">TariffAttributeValue to be removed.</param>
        /// <exception cref="InvalidAttributeValueException">Thrown when the attribute of the attribute value differs from this attribute.</exception>
        /// <exception cref="InvalidAttributeValueException">Thrown when the Values set does not contain the value to be removed.</exception>
        public void Remove(TariffAttributeValue attributeValue)
        {
            if (attributeValue.Attribute != this)
            {
                throw new InvalidAttributeValueException("The value's attribute does not match the atribute it is to be added to.");
            }

            if (!Values.Contains(attributeValue))
            {
                throw new InvalidAttributeValueException();
            }

            Values.Remove(attributeValue);
        }

        /// <summary>
        /// Removes a TariffAttributeValue from the attribute.
        /// </summary>
        /// <param name="attributeValue">String representation of the TariffAttributeValue to be removed.</param>
        public void Remove(string attributeValue)
        {
            Remove(new TariffAttributeValue(this, attributeValue));
        }

        /// <summary>
        /// Standard enumerator of the attribute values.
        /// </summary>
        /// <returns>Enumerator of the attribute values.</returns>
        public IEnumerator<TariffAttributeValue> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        /// <summary>
        /// Standard enumerator of the attribute values for the IEnumerable interface.
        /// </summary>
        /// <returns>Enumerator of the attribute values.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a string representation of the attribute.
        /// </summary>
        /// <returns>String representation of the attribute with its values.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Label + ": {");

            int i = 0;

            foreach (TariffAttributeValue val in Values)
            {
                sb.Append(val.Value);

                if (++i < Values.Count)
                {
                    sb.Append(", ");
                }
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
