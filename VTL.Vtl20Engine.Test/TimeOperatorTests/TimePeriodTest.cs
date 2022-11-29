using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.Test.TimeOperatorTests
{
    [TestClass]
    public class TimePeriodTest
    {
        [TestMethod]
        public void TimePeriod_QToString()
        {
            var sut = new TimePeriodType("2020-Q4");
            Assert.AreEqual("2020-Q4", sut.ToString());

            sut = new TimePeriodType("2020-Q12");
            Assert.AreEqual("2022-Q4", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_SToString()
        {
            var sut = new TimePeriodType("2020-S2");
            Assert.AreEqual("2020-S2", sut.ToString());

            sut = new TimePeriodType("2020-S12");
            Assert.AreEqual("2025-S2", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_YToString()
        {
            var sut = new TimePeriodType("2020-A1");
            Assert.AreEqual("2020-A1", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_MToString()
        {
            var sut = new TimePeriodType("2020-M04");
            Assert.AreEqual("2020-M04", sut.ToString());

            sut = new TimePeriodType("2020-M13");
            Assert.AreEqual("2021-M01", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_WToString()
        {
            var sut = new TimePeriodType("2020-W44");
            Assert.AreEqual("2020-W44", sut.ToString());

            sut = new TimePeriodType("2020-W144");
            Assert.AreEqual("2022-W40", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_DToString()
        {
            var sut = new TimePeriodType("2020D004");
            Assert.AreEqual("2020-D004", sut.ToString());

            sut = new TimePeriodType("2020D366");
            Assert.AreEqual("2020-D366", sut.ToString());

            sut = new TimePeriodType("2021D366");
            Assert.AreEqual("2022-D001", sut.ToString());

            sut = new TimePeriodType("2020D1113");
            Assert.AreEqual("2023-D017", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorAddToYear()
        {
            var sut = new TimePeriodType("2020-A1");

            sut = sut + 1;

            Assert.AreEqual("2021-A1", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorSubtractFromYear()
        {
            var sut = new TimePeriodType("2020-A1");

            sut = sut + (-1);

            Assert.AreEqual("2019-A1", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorAddToQuarter()
        {
            var sut = new TimePeriodType("2020-Q1");

            sut = sut + 1;

            Assert.AreEqual("2020-Q2", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorSubtractFromQuarter()
        {
            var sut = new TimePeriodType("2020-Q1");

            sut = sut + (-1);

            Assert.AreEqual("2019-Q4", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorAddToMonth()
        {
            var sut = new TimePeriodType("2020-M01");

            sut = sut + 1;

            Assert.AreEqual("2020-M02", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorSubtractFromMonth()
        {
            var sut = new TimePeriodType("2020-M01");

            sut = sut + (-1);

            Assert.AreEqual("2019-M12", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorAddToDay()
        {
            var sut = new TimePeriodType("2020-D001");

            sut = sut + 1000;

            Assert.AreEqual("2022-D270", sut.ToString());
        }

        [TestMethod]
        public void TimePeriod_PlusOperatorSubtractFromDay()
        {
            var sut = new TimePeriodType("2020-D001");

            sut = sut + (-500);

            Assert.AreEqual("2018-D231", sut.ToString());
        }
    }
}
