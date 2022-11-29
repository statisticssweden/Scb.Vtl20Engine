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

namespace VTL.Vtl20Engine.Test.GeneralPurposeOperatorTests
{
    [TestClass]
    public class ParenthesisTests
    {
        [TestCategory("Unit")]
        [TestMethod]
        public void Parenthesis_parenthesisWorks()
        {
            var a = new Operand
            { Alias = "a", Data = new IntegerType(1) };
            var b = new Operand
            { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- (a + b) * 3;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is IntegerType);
            Assert.AreEqual(9, (IntegerType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Parenthesis_executionOrder()
        {
            var a = new Operand
            { Alias = "a", Data = new NumberType(1.5m) };
            var b = new Operand
            { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- (a + b) / 2 - (5 + 2 * 10);", new[] { a, b })
                .FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(-23.25m, (NumberType)result);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Parenthesis_dataSets()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Ref. Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000),
                                new IntegerType(200)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Ref. Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(300),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(900),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(350),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000),
                                new IntegerType(300)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- (UnitedStatesData + EuropeanUnionData)/2;", new[] { D1, D2 })
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)850, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);
            }
        }
    }
}
