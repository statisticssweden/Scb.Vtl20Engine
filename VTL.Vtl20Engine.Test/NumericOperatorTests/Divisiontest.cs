using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Divisiontest
    {
        public Divisiontest()
        {
          sut = new VtlEngine(new DataContainerFactory()); ;
        }
 
        public IVtlEngine sut;

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_NormalDecimalDivision()
        {
            var a = new Operand() { Data = new NumberType(1.0m), Alias = "a" };
            var b = new Operand() { Data = new NumberType(2.0m), Alias = "b" };
            var x = new Operand[2] { a, b };

            var c = sut.Execute("c<-a/b;", x).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)0.5m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DatasetDividedWithConstant()
        {
            var a = new Operand() { Data = new NumberType(1.0m), Alias = "a" };
            var x = new [] { a };

            var c = sut.Execute("c<-a/10;", x).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)0.1m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_ConstantDividedWithDataset()
        {
            var a = new Operand() { Data = new NumberType(2.0m), Alias = "a" };
            var x = new [] { a };

            var c = sut.Execute("c<-10/a;", x).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)5m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_IntegerDivisionWithDecimalResult()
        {
            var a = new Operand() { Data = new NumberType(10), Alias = "a" };
            var b = new Operand() { Data = new NumberType(3), Alias = "b" };
            var x = new Operand[2] { a, b };

            var c = sut.Execute("c<-a/b;", x).FirstOrDefault(r => r.Alias.Equals("c"));
            var d = Decimal.Round((Decimal)(NumberType) c.GetValue(),10);
            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)3.3333333333m, d);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DivisionWithCertainAmountOfDecimalsResult()
        {
            var a = new Operand() { Data = new NumberType(1234), Alias = "a" };
            var b = new Operand() { Data = new NumberType(1000), Alias = "b" };
            var x = new Operand[2] { a, b };

            var c = sut.Execute("c<-a/b;", x).FirstOrDefault(r => r.Alias.Equals("c"));
 
            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)1.234m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod] 
        public void Division_DivisionWithZero()
        {
            var a = new Operand() { Data = new NumberType(1234), Alias = "a" };
            var b = new Operand() { Data = new NumberType(0), Alias = "b" };
            var x = new Operand[2] { a, b };
            var c = sut.Execute("c<-a/b;", x).FirstOrDefault(r => r.Alias.Equals("c"));
            try
            {
                c.GetValue();
            }            
            catch 
            {
                Assert.ThrowsException<VtlException>(()=>c.GetValue());
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DivideZeroWithNumberResultZero()
        {
            var a = new Operand() { Data = new NumberType(0.0000m), Alias = "a" };
            var b = new Operand() { Data = new NumberType(1000.2813221m), Alias = "b" };
            var x = new Operand[2] { a, b };

            var c = sut.Execute("c<-a/b;", x).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)0m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DivisionWithNull()
        {
            var a = new Operand() { Data = new NumberType(1234), Alias = "a" };
            var b = new Operand() { Data = null, Alias = "b" };
            var x = new [] { a, b };
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("c<-a/b;", x));
            Assert.AreEqual("b tilldelades aldrig något värde.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DivisionWithNullAsNumerator()
        {
            var a = new Operand() { Data = null, Alias = "a" };
            var b = new Operand() { Data = new NumberType(1234), Alias = "b" };
            var x = new [] { a, b };
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("c<-a/b;", x));
            Assert.AreEqual("a tilldelades aldrig något värde.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_OrderRulesWithDivision()
        {
            var a = new Operand() { Data = new NumberType(1234), Alias = "a" };
            var b = new Operand() { Data = new NumberType(1000), Alias = "b" };
            var x = new Operand[2] { a, b };
            var c = sut.Execute("c<-30+a/b+6;", x).FirstOrDefault(r => r.Alias.Equals("c"));
                c.GetValue();
            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)37.234m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_ManyDivisions()
        {
            var a = new Operand() { Data = new NumberType(61), Alias = "a" };
            var b = new Operand() { Data = new NumberType(13), Alias = "b" };
            var x = new Operand[2] { a, b };
            var c = sut.Execute("c<-11895/a/b/5;", x).FirstOrDefault(r => r.Alias.Equals("c"));
            c.GetValue();
            Assert.IsNotNull(c);
            Assert.AreEqual((NumberType)3m, c.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_IntegerToInteger()
        {
            var a = new Operand
            { Alias = "a", Data = new IntegerType(6) };
            var b = new Operand
            { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a / b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsFalse(c.GetValue() is IntegerType);
            Assert.IsTrue(c.GetValue() is NumberType);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Division_DecimalToDecimal()
        {
            var a = new Operand
            { Alias = "a", Data = new NumberType(new decimal(4.4)) };
            var b = new Operand
            { Alias = "b", Data = new NumberType(new decimal(2.2)) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a / b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsTrue(c.GetValue() is NumberType);
            Assert.IsFalse(c.GetValue() is IntegerType);
        }
    }
}
