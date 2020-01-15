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
        /// Set of all valid attribute values.
        /// </summary>
        private ISet<TariffAttributeValue> _values;

        /// <summary>
        /// Read only collection of all valid attribute values, will be exposed to the public when assigned.
        /// </summary>
        private IReadOnlyCollection<TariffAttributeValue> _valuesList;

        /// <summary>
        /// Standard constructor of a TariffAttribute.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        public TariffAttribute(string label)
        {
            Label = label;
        }

        /// <summary>
        /// Constructor of a TariffAttribute providing a collection of valid values.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        /// <param name="attributeValues">Valid values for the attribute.</param>
        public TariffAttribute(string label, IEnumerable<TariffAttributeValue> attributeValues) : this(label)
        {
            _values = new HashSet<TariffAttributeValue>(attributeValues);
        }

        /// <summary>
        /// Constructor of a TariffAttribute providing a collection of valid values.
        /// </summary>
        /// <param name="label">Description of the attribute.</param>
        /// <param name="attributeValues">Valid values for the attribute as plain text.</param>
        public TariffAttribute(string label, IEnumerable<string> attributeValues) : this(label)
        {
            _values = new HashSet<TariffAttributeValue>(attributeValues.Select(x => new TariffAttributeValue(this, x)));
        }

        /// <summary>
        /// Indexer to retrieve a valid value of the attribute.
        /// </summary>
        /// <param name="val">Description of the attribute value as plain text.</param>
        /// <returns>An instance of the TariffAttributeValue struct representing a valid value for this attribute.</returns>
        /// <exception cref="InvalidAttributeValueException">Thrown when a values that is not legal for this attribute is tried to be accessed.</exception>
        public TariffAttributeValue this[string val]
        {
            get
            {
                TariffAttributeValue attributeValue = new TariffAttributeValue(this, val);

                if (!_values.Contains(attributeValue))
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
        public IReadOnlyCollection<TariffAttributeValue> Values
        {
            get
            {
                if (_valuesList == null)
                {
                    _valuesList = _values.ToList().AsReadOnly();
                }

                return _valuesList;
            }
        }

        /// <summary>
        /// Standard enumerator of the attribute values.
        /// </summary>
        /// <returns>Enumerator of the attribute values.</returns>
        public IEnumerator<TariffAttributeValue> GetEnumerator()
        {
            return _values.GetEnumerator();
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

            foreach (TariffAttributeValue val in _values)
            {
                sb.Append(val.Value);

                if (++i < _values.Count)
                {
                    sb.Append(", ");
                }
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
