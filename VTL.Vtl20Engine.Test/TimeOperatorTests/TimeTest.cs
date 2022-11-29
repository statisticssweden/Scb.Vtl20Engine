using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.Test.TimeOperatorTests
{
    [TestClass]
    public class TimeTest
    {
        [TestMethod]
        public void Time_FromAndToString()
        {
            var sut = new TimeType("2020-01-01/2020-06-20");
            Assert.AreEqual("2020-01-01/2020-06-20", sut.ToString());
        }


    }
}
