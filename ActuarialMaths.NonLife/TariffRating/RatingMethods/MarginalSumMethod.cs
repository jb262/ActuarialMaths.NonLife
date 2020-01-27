using System;
using System.Collections.Generic;
using System.Linq;
using ActuarialMaths.NonLife.TariffRating.Model;

namespace ActuarialMaths.NonLife.TariffRating.RatingMethods
{
    /// <summary>
    /// The marginal sum tariff rating method.
    /// </summary>
    public class MarginalSumMethod : MarginalFactorMethod
    {
        /// <summary>
        /// Arbitrarily chosen threshold to indicate whether the changes in the marginal factors is so neglectible that
        /// the iterative calculation of the marginal factors can be considered finished.
        /// </summary>
        private const decimal _deltaThreshold = 10e-9m;

        /// <summary>
        /// Standard constructor of the marginal sum method.
        /// </summary>
        /// <param name="tariffData">The method's underlying tariff data.</param>
        public MarginalSumMethod(ITariffData tariffData) : base(tariffData) { }

        /// <summary>
        /// Calculates the marginal factors according to the chosen model.
        /// </summary>
        /// <returns>The mapping of each of the model's tariff attribute values to their marginal factors.</returns>
        protected override IDictionary<TariffAttributeValue, decimal> CalculateFactors()
        {
            bool? thresholdReached;

            IDictionary<TariffAttributeValue, decimal> factors = new Dictionary<TariffAttributeValue, decimal>();
            IEnumerable<TariffAttributeValue> distinctAttributeValues = TariffData.TariffKeys
                .SelectMany(x => x)
                .Distinct()
                .ToList();

            foreach (TariffAttributeValue attributeValue in distinctAttributeValues)
            {
                factors.Add(attributeValue, 1m);
            }

            do
            {
                factors = CalculateFactorsIteratively(distinctAttributeValues, factors, out thresholdReached);
            }
            while (thresholdReached != true);

            return factors;
        }

        /// <summary>
        /// Iteratively calculates the marginal factors for each attribute value.
        /// </summary>
        /// <param name="attributeValues">Attribute values the marginal factors are to be calculated for.</param>
        /// <param name="factors">Starting values for the iterative calculation of the marginal factors.</param>
        /// <param name="thresholdReached">Indicator whether the threshold of significant change is reached or not.</param>
        /// <param name="deltaThreshold">The threshold to indicate if the change between the initial and the new factor is significant.</param>
        /// <returns>A dictionary with the newly calculated marginal factors.</returns>
        private IDictionary<TariffAttributeValue, decimal> CalculateFactorsIteratively
            (
                IEnumerable<TariffAttributeValue> attributeValues,
                IDictionary<TariffAttributeValue, decimal> factors,
                out bool? thresholdReached,
                decimal deltaThreshold = _deltaThreshold
            )
        {

            IDictionary<TariffAttributeValue, decimal> factorsAfterIteration = new Dictionary<TariffAttributeValue, decimal>();
            thresholdReached = null;

            foreach (TariffAttributeValue attributeValue in attributeValues)
            {
                IEnumerable<ITariffKey> keys = TariffData.TariffKeys.Where(x => x.Contains(attributeValue));
                decimal aggregate = 0m;

                foreach (ITariffKey key in keys)
                {
                    aggregate += key
                        .Where(x => !x.Equals(attributeValue))
                        .Select(x => factorsAfterIteration.ContainsKey(x) ? factorsAfterIteration[x] : factors[x])
                        .Aggregate(1m, (x, y) => x * y) * TariffData[key].PolicyCount;
                }

                decimal totalClaimsAmount = TariffData[attributeValue].Select(x => x.ClaimsAmount).Sum();
                decimal factorAfterIteration = totalClaimsAmount / (TariffData.ExpectedClaimsExpenditure() * aggregate);

                thresholdReached = (thresholdReached ?? true) && (Math.Abs(factorAfterIteration - factors[attributeValue]) < deltaThreshold);

                factorsAfterIteration.Add(attributeValue, factorAfterIteration);
            }

            return factorsAfterIteration;
        }
    }
}
