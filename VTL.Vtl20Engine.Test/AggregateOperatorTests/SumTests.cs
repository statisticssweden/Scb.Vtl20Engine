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

namespace VTL.Vtl20Engine.Test.AggregateOperatorTests
{
    [TestClass]
    public class SumTests
    {
        private Operand _ds_1;

        [TestInitialize]
        public void TestSetup()
        {
            _ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "RefDate",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "MeasName",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value2",
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
                                new IntegerType(1),
                                new IntegerType(200),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(1),
                                new IntegerType(800),
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(1),
                                new IntegerType(250),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1),
                                new IntegerType(1000),
                                new IntegerType(400)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(2),
                                new IntegerType(200),
                                new IntegerType(110)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(2),
                                new IntegerType(800),
                                new IntegerType(210)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(2),
                                new IntegerType(250),
                                new IntegerType(310)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(2),
                                new IntegerType(1000),
                                new IntegerType(null)
                            }
                        )
                    }))
            };
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupExcept()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group except RefDate);", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1800, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 600, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1800, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 210, dataPointEnumerator.Current[3]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupExceptTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group except RefDate, Id);", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3600, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 810, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 900, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 820, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupBy()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by MeasName);", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3600, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 810, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 900, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 820, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupByTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by MeasName, Id);", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1800, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 600, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1800, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 210, dataPointEnumerator.Current[3]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupByComponentCountTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by MeasName, Id);", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ids = DS_r.GetIdentifierNames();
            Assert.AreEqual(2, ids.Length);
            Assert.IsTrue(ids.Contains("MeasName"));
            Assert.IsTrue(ids.Contains("Id"));

            var meas = DS_r.GetMeasureNames();
            Assert.AreEqual(2, meas.Length);
            Assert.IsTrue(meas.Contains("Value1"));
            Assert.IsTrue(meas.Contains("Value2"));

            var comp = DS_r.GetComponentNames();
            Assert.AreEqual(4, comp.Length);
            Assert.IsTrue(comp.Contains("MeasName"));
            Assert.IsTrue(comp.Contains("Id"));
            Assert.IsTrue(comp.Contains("Value1"));
            Assert.IsTrue(comp.Contains("Value2"));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupExceptComponentCountTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group except MeasName, Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ids = DS_r.GetIdentifierNames();
            Assert.AreEqual(1, ids.Length);
            Assert.IsTrue(ids.Contains("RefDate"));

            var meas = DS_r.GetMeasureNames();
            Assert.AreEqual(2, meas.Length);
            Assert.IsTrue(meas.Contains("Value1"));
            Assert.IsTrue(meas.Contains("Value2"));

            var comp = DS_r.GetComponentNames();
            Assert.AreEqual(3, comp.Length);
            Assert.IsTrue(comp.Contains("RefDate"));
            Assert.IsTrue(comp.Contains("Value1"));
            Assert.IsTrue(comp.Contains("Value2"));
        }
        
        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_WithoutGrouping()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 4500, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1630, dataPointEnumerator.Current[1]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_HavingThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute(
                "DS_r <- sum(DS_1 group by MeasName having count(MeasName) > 0);", new[] {_ds_1}));
            Assert.AreEqual("Having är inte implementerad än.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_ResultIsIntegerType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by MeasName);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual("Value1", result.DataSetComponents[1].Name);
            Assert.IsTrue(result.DataSetComponents[1].DataType == typeof(IntegerType));
        }

    }
}