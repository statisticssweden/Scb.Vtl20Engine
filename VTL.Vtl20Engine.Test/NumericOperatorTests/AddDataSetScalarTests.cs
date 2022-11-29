using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class AddDataSetScalarTests
    {
        private DataSetType _dataSet1;

        [TestInitialize]
        public void TestSetup()
        {
            _dataSet1 = MockComponent.MakeDataSet(new List<MockComponent>
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "Name",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "Value",
                    Role = ComponentType.ComponentRole.Measure
                }
            },
            new SimpleDataPointContainer(new HashSet<DataPointType>
            {
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("a"),
                        new IntegerType(2)
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("b"),
                        new IntegerType(3)
                    }
                )
            }));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Addition_addDataSetAndInteger()
        {
            var a = new Operand
            {
                Alias = "a",
                Data = _dataSet1
            };
            var b = new Operand
                {Alias = "b", Data = new IntegerType(2)};
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a + b;", new[] {a, b}).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "a", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "b", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[1]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Addition_addNumberAndDataSet()
        {
            var b = new Operand
            {
                Alias = "b",
                Data = _dataSet1
            };
            var a = new Operand
                {Alias = "a", Data = new NumberType(2.5m)};
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a + b;", new[] {a, b}).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "a", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 4.5m, dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "b", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 5.5m, dataPointEnumerator.Current[1]);
            }
        }
    }
}