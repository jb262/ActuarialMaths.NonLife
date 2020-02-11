using Microsoft.VisualStudio.TestTools.UnitTesting;
using ActuarialMaths.NonLife.ClaimsReserving.Model;
using ActuarialMaths.NonLife.ClaimsReserving.ReservingMethods;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Test.ClaimsReservingTests
{
    [TestClass]
    public class ReservingMethodTests
    {
        [TestMethod]
        public void AdditiveMethod_CalculatesReservesCorrectly()
        {
            IReservingMethod additiveMethod = TestObjectBuilder.CreateAdditiveMethod();

            IEnumerable<decimal> reserves = new List<decimal>() { 0m, 166.5m, 675.59m, 1615.69m, 2919.53m, 5291.09m };

            bool calculatedCorrectly = additiveMethod.Reserves()
                .Zip(reserves, (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                .Aggregate(true, (x, y) => x && y);

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void AdditiveMethod_CalculatesFactorsCorrectly()
        {
            FactorBasedMethod additiveMethod = TestObjectBuilder.CreateAdditiveMethod();

            IEnumerable<decimal> factors = new List<decimal>() { 0.243m, 0.222m, 0.154m, 0.142m, 0.09m, 0.037m };

            bool calculatedCorrectly = additiveMethod.Factors()
                .Zip(factors, (x, y) => Math.Abs(x - Math.Round(y, 3)) <= 0.001m)
                .Aggregate(true, (x, y) => x && y);

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void AdditiveMethod_CalculatesCashflowsCorrectly()
        {
            IReservingMethod additiveMethod = TestObjectBuilder.CreateAdditiveMethod();

            IEnumerable<decimal> cashflows = new List<decimal>() { 4379.85m, 2978.89m, 2009.11m, 997.15m, 303.4m };
            var cf = additiveMethod.Cashflows().ToList();

            bool calculatedCorrectly = additiveMethod.Cashflows()
                .Zip(cashflows, (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                .Aggregate(true, (x, y) => x && y);

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void AdditiveMethod_CalculatesProjectionCorrectly()
        {
            ISquare projection = new Square(6);
            projection.SetRow(new List<decimal>() { 1001m, 1855m, 2423m, 2988m, 3335m, 3483m }, 0);
            projection.SetRow(new List<decimal>() { 1113m, 2103m, 2774m, 3422m, 3844m, 4010.5m }, 1);
            projection.SetRow(new List<decimal>() { 1265m, 2433m, 3233m, 3977m, 4456.49m, 4652.59m }, 2);
            projection.SetRow(new List<decimal>() { 1490m, 2873m, 3880m, 4730.87m, 5273.69m, 5495.69m }, 3);
            projection.SetRow(new List<decimal>() { 1725m, 3261m, 4322.48m, 5300.98m, 5925.23m, 6180.53m }, 4);
            projection.SetRow(new List<decimal>() { 1889m, 3710.51m, 4971.98m, 6134.84m, 6876.69m, 7180.09m }, 5);

            bool calculatedCorrectly = true;

            IReservingMethod additiveMethod = TestObjectBuilder.CreateAdditiveMethod();

            for (int i = 0; i < additiveMethod.Projection().Periods; i++)
            {
                IEnumerable<decimal> row = additiveMethod.Projection().GetRow(i);
                calculatedCorrectly = calculatedCorrectly
                    && row.Zip(projection.GetRow(i), (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                    .Aggregate(true, (x, y) => x && y);
            }

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void ChainLadder_CalculatesProjectionCorrectly()
        {
            ISquare projection = new Square(6);
            projection.SetRow(new List<decimal>() { 1001m, 1855m, 2423m, 2988m, 3335m, 3483m }, 0);
            projection.SetRow(new List<decimal>() { 1113m, 2103m, 2774m, 3422m, 3844m, 4014.59m }, 1);
            projection.SetRow(new List<decimal>() { 1265m, 2433m, 3233m, 3977m, 4454.12m, 4651.78m }, 2);
            projection.SetRow(new List<decimal>() { 1490m, 2873m, 3880m, 4780.73m, 5354.27m, 5591.88m }, 3);
            projection.SetRow(new List<decimal>() { 1725m, 3261m, 4333.22m, 5339.16m, 5979.69m, 6245.06m }, 4);
            projection.SetRow(new List<decimal>() { 1889m, 3588.07m, 4767.82m, 5874.66m, 6579.44m, 6871.42m }, 5);

            bool calculatedCorrectly = true;

            IReservingMethod chainLadder = TestObjectBuilder.CreateChainLadderMethod();

            for (int i = 0; i < chainLadder.Projection().Periods; i++)
            {
                IEnumerable<decimal> row = chainLadder.Projection().GetRow(i);
                calculatedCorrectly = calculatedCorrectly
                    && row.Zip(projection.GetRow(i), (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                    .Aggregate(true, (x, y) => x && y);
            }

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void ChainLadder_CalculatesFactorsCorrectly()
        {
            IEnumerable<decimal> factors = new List<decimal>() { 1.899454m, 1.3288m, 1.232147m, 1.119969m, 1.044378m };

            FactorBasedMethod chainLadder = TestObjectBuilder.CreateChainLadderMethod();

            bool calculatedCorrectly = chainLadder.Factors()
                .Zip(factors, (x, y) => Math.Abs(x - Math.Round(y, 6)) <= 1e-6m)
                .Aggregate(true, (x, y) => x && y);

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void BornhuetterFerguson_CalculatesProjectionCorrectly()
        {
            ISquare projection = new Square(6);
            projection.SetRow(new List<decimal>() { 1001m, 1855m, 2423m, 2988m, 3335m, 3483m }, 0);
            projection.SetRow(new List<decimal>() { 1113m, 2103m, 2774m, 3422m, 3844m, 4043.05m }, 1);
            projection.SetRow(new List<decimal>() { 1265m, 2433m, 3233m, 3977m, 4390.82m, 4620.72m }, 2);
            projection.SetRow(new List<decimal>() { 1490m, 2873m, 3880m, 4785.28m, 5294.5m, 5577.4m }, 3);
            projection.SetRow(new List<decimal>() { 1725m, 3261m, 4441.66m, 5435.9m, 5995.16m, 6305.86m }, 4);
            projection.SetRow(new List<decimal>() { 1889m, 3343.75m, 4545.5m, 5557.5m, 6126.75m, 6443m }, 5);

            bool calculatedCorrectly = true;

            IReservingMethod bornhuetterFerguson = TestObjectBuilder.CreateBornhuetterFergusonMethod();

            for (int i = 0; i < bornhuetterFerguson.Projection().Periods; i++)
            {
                IEnumerable<decimal> row = bornhuetterFerguson.Projection().GetRow(i);
                calculatedCorrectly = calculatedCorrectly
                    && row.Zip(projection.GetRow(i), (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                    .Aggregate(true, (x, y) => x && y);
            }

            Assert.IsTrue(calculatedCorrectly);
        }

        [TestMethod]
        public void CapeCod_CalculatedProjectionCorrectly()
        {
            ISquare projection = new Square(6);
            projection.SetRow(new List<decimal>() { 1001m, 1855m, 2423m, 2988m, 3335m, 3483m }, 0);
            projection.SetRow(new List<decimal>() { 1113m, 2103m, 2774m, 3422m, 3844m, 4044.24m }, 1);
            projection.SetRow(new List<decimal>() { 1265m, 2433m, 3233m, 3977m, 4401.51m, 4637.36m }, 2);
            projection.SetRow(new List<decimal>() { 1490m, 2873m, 3880m, 4734.37m, 5214.95m, 5481.94m }, 3);
            projection.SetRow(new List<decimal>() { 1725m, 3261m, 4427.75m, 5410.27m, 5962.94m, 6269.98m }, 4);
            projection.SetRow(new List<decimal>() { 1889m, 3567.48m, 4954.05m, 6121.69m, 6778.49m, 7143.37m }, 5);

            bool calculatedCorrectly = true;

            IReservingMethod capeCod = TestObjectBuilder.CreateCapeCodMethod();

            for (int i = 0; i< capeCod.Projection().Periods; i++)
            {
                IEnumerable<decimal> row = capeCod.Projection().GetRow(i);
                calculatedCorrectly = calculatedCorrectly
                    && row.Zip(projection.GetRow(i), (x, y) => Math.Abs(x - Math.Round(y, 2)) <= 0.01m)
                    .Aggregate(true, (x, y) => x && y);
            }

            Assert.IsTrue(calculatedCorrectly);
        }
    }
}
