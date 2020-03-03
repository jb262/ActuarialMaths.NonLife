using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const decimal _deltaThreshold = 1e-9m;

        /// <summary>
        /// Standard constructor of the marginal sum method.
        /// </summary>
        /// <param name="tariffData">The method's underlying tariff data.</param>
        public MarginalSumMethod(ITariffData tariffData) : base(tariffData) { }

        /// <summary>
        /// Calculates the marginal factors according to the chosen model.
        /// </summary>
        /// <returns>The mapping of each of the model's tariff attribute values to their marginal factors.</returns>
        protected override IReadOnlyDictionary<TariffAttributeValue, decimal> CalculateFactors()
        {
            bool thresholdReached = false;

            IDictionary<TariffAttributeValue, decimal> factors = new Dictionary<TariffAttributeValue, decimal>();
            IEnumerable<TariffAttributeValue> distinctAttributeValues = TariffData.TariffKeys
                .SelectMany(x => x)
                .Distinct()
                .ToList();

            foreach (TariffAttributeValue attributeValue in distinctAttributeValues)
            {
                factors.Add(attributeValue, 1m);
            }

            while (!thresholdReached)
            {
                thresholdReached = CalculateFactorsIteratively(distinctAttributeValues, ref factors);
            }

            return new ReadOnlyDictionary<TariffAttributeValue, decimal>(factors);
        }

        /// <summary>
        /// Iteratively calculates the marginal factors for each attribute value.
        /// </summary>
        /// <param name="attributeValues">Attribute values the marginal factors are to be calculated for.</param>
        /// <param name="factors">Starting values for the iterative calculation of the marginal factors.</param>
        /// <returns>True if the threshold indicating a significant change in factors was undercut, false otherwise.</returns>
        private bool CalculateFactorsIteratively(IEnumerable<TariffAttributeValue> attributeValues, ref IDictionary<TariffAttributeValue, decimal> factors)
        {
            bool thresholdReached = true;

            foreach (TariffAttributeValue attributeValue in attributeValues)
            {
                IEnumerable<ITariffKey> keys = TariffData.TariffKeys.Where(x => x.Contains(attributeValue));
                decimal aggregate = 0m;

                foreach (ITariffKey key in keys)
                {
                    decimal partialFactor = 1m;
                    foreach (TariffAttributeValue differentAttributeValue in key.Where(x => !x.Equals(attributeValue)))
                    {
                        partialFactor *= factors[differentAttributeValue];
                    }

                    aggregate += partialFactor * TariffData[key].PolicyCount;
                }

                decimal totalClaimsAmount = TariffData[attributeValue].Select(x => x.ClaimsAmount).Sum();
                decimal factorAfterIteration = totalClaimsAmount / (TariffData.ExpectedClaimsExpenditure() * aggregate);

                thresholdReached = thresholdReached && (Math.Abs(factorAfterIteration - factors[attributeValue]) < _deltaThreshold);

                factors[attributeValue] = factorAfterIteration;
            }

            return thresholdReached;
        }
    }
}
