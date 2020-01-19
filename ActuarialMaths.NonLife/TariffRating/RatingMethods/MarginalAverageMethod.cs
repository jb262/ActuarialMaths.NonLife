using System.Collections.Generic;
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
        protected override IDictionary<TariffAttributeValue, decimal> CalculateFactors()
        {
            IDictionary<TariffAttributeValue, decimal> factors = new Dictionary<TariffAttributeValue, decimal>();

            foreach (ITariffAttribute attribute in TariffData.Attributes)
            {
                foreach (TariffAttributeValue tariffAttributeValue in attribute)
                {
                    TariffCell cell = TariffData[tariffAttributeValue].Aggregate(new TariffCell(0m, 0), (x, y) => x + y);
                    if (cell.PolicyCount > 0)
                    {
                        factors[tariffAttributeValue] = (cell.ClaimsAmount / cell.PolicyCount) / TariffData.ExpectedClaimsExpenditure();
                    }
                    else
                    {
                        factors[tariffAttributeValue] = 0m;
                    }
                }
            }

            return factors;
        }
    }
}
