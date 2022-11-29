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
    public class CountTests
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
                new SimpleDataPointContainer(new HashSet<DataPointType>
                {
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Population"),
                            new IntegerType(1),
                            new IntegerType(200),
                            new IntegerType(null)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(1),
                            new IntegerType(805),
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
                            new IntegerType(700)
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
                            new IntegerType(0)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(2),
                            new IntegerType(null),
                            new IntegerType(217)
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
                            new IntegerType(410)
                        }
                    )
                }))
            };
        }


        [TestMethod]
        public void CountByOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- count(DS_1 group by RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void CountGroupByTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- count(DS_1 group by RefDate,Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void CountGroupExceptOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- count(DS_1 group except RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void CountGroupExceptTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- count(DS_1 group except RefDate,Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)4, dataPointEnumerator.Current[2]);
            }
        }



        [TestMethod]
        public void CountTimePeriod()

        {
            var ds = new Operand
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
                    new MockComponent(typeof(TimePeriodType))
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
                            new StringType("2013"),
                            new StringType("Population"),
                            new IntegerType(1),
                            new TimePeriodType(2000, Duration.Month, 3)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(1),
                            new TimePeriodType(2000, Duration.Month, 1)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(1),
                            new TimePeriodType(2001, Duration.Month, 1)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1),
                            new TimePeriodType(2000, Duration.Month, 3)
                        }
                    )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- count(DS_1 group by MeasName);", new[] { ds })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)2, dataPointEnumerator.Current[1]);
            }
        }
    }
}