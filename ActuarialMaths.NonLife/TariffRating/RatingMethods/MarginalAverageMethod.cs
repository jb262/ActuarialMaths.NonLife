using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ActuarialMaths.NonLife.TariffRating.Model;

namespace ActuarialMaths.NonLife.TariffRating.RatingMethods
{
    /// <summary>
    /// The marginal average tariff rating method.
    /// </summary>
    public class MarginalAverageMethod : MarginalFactorMethod
    {
        /// <summary>
        /// Constructor of a marginal average tariff rating method.
        /// </summary>
        /// <param name="tariffData">The method's underlying tariff data.</param>
        public MarginalAverageMethod(ITariffData tariffData) : base(tariffData) { }

        /// <summary>
        /// Calculates the marginal factors according to the marginal average tariff rating method.
        /// </summary>
        /// <returns>The mapping of each of the model's tariff attribute values to their marginal factors.</returns>
        protected override IReadOnlyDictionary<TariffAttributeValue, decimal> CalculateFactors()
        {
            IDictionary<TariffAttributeValue, decimal> factors = new Dictionary<TariffAttributeValue, decimal>();

            foreach (TariffAttributeValue tariffAttributeValue in TariffData.TariffKeys.SelectMany(x => x).Distinct())
            {
                decimal totalAmount = 0m;
                int totalCount = 0;

                foreach (TariffCell cell in TariffData[tariffAttributeValue])
                {
                    totalAmount += cell.ClaimsAmount;
                    totalCount += cell.PolicyCount;
                }
                if (totalCount > 0)
                {
                    factors[tariffAttributeValue] = (totalAmount / totalCount) / TariffData.ExpectedClaimsExpenditure();
                }
                else
                {
                    factors[tariffAttributeValue] = 0m;
                }
            }

            return new ReadOnlyDictionary<TariffAttributeValue, decimal>(factors);
        }
    }
}
