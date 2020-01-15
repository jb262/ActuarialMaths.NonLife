namespace ActuarialMaths.NonLife.TariffRating.Model
{
    /// <summary>
    /// A valid value for a given attribute.
    /// </summary>
    public struct TariffAttributeValue
    {
        /// <summary>
        /// Description of the attribute value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Attribute this value is valid for.
        /// </summary>
        public readonly ITariffAttribute Attribute;

        /// <summary>
        /// Constructor of a TarkiffAttributeValue.
        /// </summary>
        /// <param name="attribute">Attribute the value is to be valid for.</param>
        /// <param name="val">Description of the value.</param>
        public TariffAttributeValue(ITariffAttribute attribute, string val)
        {
            Value = val;
            Attribute = attribute;
        }
    }
}
