using System.Collections.Generic;
using ActuarialMaths.NonLife.ClaimsReserving.Model;
using ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods;

namespace Test.ClaimsReservingTests
{
    public static class TestObjectBuilder
    {
        internal static ITriangle CreateIncrementalTriangle()
        {
            ITriangle triangle = new IncrementalTriangle();

            triangle.AddClaims(new List<decimal>() { 1001m });
            triangle.AddClaims(new List<decimal>() { 1113m, 854m });
            triangle.AddClaims(new List<decimal>() { 1265m, 990m, 568m });
            triangle.AddClaims(new List<decimal>() { 1490m, 1168m, 671m, 565m });
            triangle.AddClaims(new List<decimal>() { 1725m, 1383m, 800m, 648m, 347m });
            triangle.AddClaims(new List<decimal>() { 1889m, 1536m, 1007m, 744m, 422m, 148m });

            return triangle;
        }

        internal static ITriangle CreateCumulativeTriangle()
        {
            return TriangleBuilder<CumulativeTriangle>.CreateFrom(CreateIncrementalTriangle());
        }

        internal static IEnumerable<decimal> CreateFactors()
        {
            return new List<decimal>() { 0.28m, 0.51m, 0.7m, 0.86m, 0.95m, 1m };
        }

        internal static IEnumerable<decimal> CreateVolumeMeasures()
        {
            return new List<decimal>() { 4000m, 4500m, 5300m, 6000m, 6900m, 8200m };
        }

        internal static FactorBasedMethod CreateAdditiveMethod()
        {
            ITriangle triangle = CreateIncrementalTriangle();
            IEnumerable<decimal> premiums = CreateVolumeMeasures();
            return new AdditiveMethod(triangle, premiums);
        }

        internal static FactorBasedMethod CreateChainLadderMethod()
        {
            return new ChainLadder(CreateIncrementalTriangle());
        }

        internal static IReservingMethod CreateBornhuetterFergusonMethod()
        {
            ITriangle triangle = CreateIncrementalTriangle();
            IEnumerable<decimal> alpha = new List<decimal>() { 3517m, 3981m, 4598m, 5658m, 6214m, 6325m };
            IEnumerable<decimal> factors = CreateFactors();

            return new BornhuetterFerguson(triangle, factors, alpha);
        }

        internal static IReservingMethod CreateCapeCodMethod()
        {
            ITriangle triangle = CreateIncrementalTriangle();
            IEnumerable<decimal> factors = CreateFactors();
            IEnumerable<decimal> volumeMeasures = CreateVolumeMeasures();

            return new CapeCod(triangle, factors, volumeMeasures);
        }

        internal static IReservingMethod CreateLossDevelopmentMethod()
        {
            ITriangle triangle = CreateIncrementalTriangle();
            IEnumerable<decimal> factors = CreateFactors();
            return new LossDevelopment(triangle, factors);
        }
    }
}
