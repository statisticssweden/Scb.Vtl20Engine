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
    public class MultiplyDataSetsTests
    {
        [TestCategory("Unit")]
        [TestMethod]
        public void MultiplyDataSetsTests_multiplyDataSetsWithHomonimousIdentifiers()
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
                            new IntegerType(200)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(800)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(250)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1000)
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
                            new IntegerType(300)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(900)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(350)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1000)
                        }
                    )
                }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData * EuropeanUnionData;", new[] { D1, D2 })
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Ref. Date", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 720000, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1000000, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 60000, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 87500, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void MultiplyDataSetsTests_multiplyAndAddDataSetsWithHomonimousIdentifiers()
        {
            var D1 = new Operand
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
                            new IntegerType(300)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(900)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(350)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1000)
                        }
                    )
                }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- EuropeanUnionData+1*3;", new[] { D1 })
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Ref. Date", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 903, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1003, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 303, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 353, dataPointEnumerator.Current[2]);
            }
        }

    }
}