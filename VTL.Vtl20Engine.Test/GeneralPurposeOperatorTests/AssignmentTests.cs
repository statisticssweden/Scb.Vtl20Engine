using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.GeneralPurposeOperatorTests
{
    [TestClass]
    public class AssignmentTests
    {
        [TestMethod]
        public void VtlEngine_PassDataSetThrough()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var result = sut.Execute("b <- 1", new Operand[0]);

            Assert.AreEqual(new IntegerType(1), result[0].Data);
        }

        [TestMethod]
        public void VtlEngine_PersistantAssignment()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var result = sut.Execute("b := 1; c <- 2;", new Operand[0]);

            Assert.AreEqual(new IntegerType(2), result.FirstOrDefault(o => o.Alias.Equals("c")).Data);
            Assert.IsTrue(result.FirstOrDefault(o => o.Alias.Equals("c")).Persistant);
            Assert.IsFalse(result.FirstOrDefault(o => o.Alias.Equals("b")).Persistant);
        }

        [TestMethod]
        public void VtlEngine_PersistantAssignment2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var result = sut.Execute("c <- 2; b := 1;", new Operand[0]);

            Assert.AreEqual(new IntegerType(2), result.FirstOrDefault(o => o.Alias.Equals("c")).Data);
            Assert.IsTrue(result.FirstOrDefault(o => o.Alias.Equals("c")).Persistant);
            Assert.IsFalse(result.FirstOrDefault(o => o.Alias.Equals("b")).Persistant);
        }

        [TestMethod]
        public void VtlEngine_PermanetAssignmentCircleReference()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute("c <- c;", new Operand[0]));
            Assert.AreEqual("Koden innehåller cirkelreferenser gällande operand c, eller innehåller för många tilldelningar.", e.Message);
        }

        [TestMethod]
        public void VtlEngine_CircleReference()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute("c := r; r <- c;", new Operand[0]));
            Assert.AreEqual("Koden innehåller cirkelreferenser gällande operand c, eller innehåller för många tilldelningar.", e.Message);
        }
    }
}