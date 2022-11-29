using System;
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
    public class AverageTest
    {
        private Operand _ds_1;
        private Operand _ds_2;

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
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType(null),
                            new StringType("Gross Prod."),
                            new IntegerType(2),
                            new IntegerType(10),
                            new IntegerType(10)
                        }
                    )
                }))
            };

            _ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "RefDate",
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
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                   {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new IntegerType(1),
                                new IntegerType(0),
                            }
                        ),

                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new IntegerType(1),
                                new IntegerType(0)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new IntegerType(1),
                                new IntegerType(0)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new IntegerType(1),
                                new IntegerType(0)
                            }
                        )
                    }))
            };
        }

        [TestMethod]
        public void Average_GroupByOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group by RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[2].DataType);

            Assert.AreEqual(3, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[0].HasValue());
                Assert.AreEqual((NumberType) 10m, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 10m, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                var d1 = Decimal.Round((Decimal) (NumberType) dataPointEnumerator.Current[1], 10);
                Assert.AreEqual((NumberType) 401.6666666667m, d1);
                var d2 = Decimal.Round((Decimal) (NumberType) dataPointEnumerator.Current[2], 10);
                Assert.AreEqual((NumberType) 175.6666666667m, d2);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)625m, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType)455m, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Average_GroupByTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group by RefDate, Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 502.5m, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 200, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 625, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 550, dataPointEnumerator.Current[3]);
                                
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 2, dataPointEnumerator.Current[0]);
                Assert.IsFalse(dataPointEnumerator.Current[1].HasValue());
                Assert.AreEqual((NumberType) 10, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 200, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 163.5m, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 625, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 360, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void Average_GroupExceptOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group except RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 902.5m, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 300m, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 225, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 700, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 505, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 637m / 3m, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 225, dataPointEnumerator.Current[2]);
                Assert.AreEqual((NumberType) 210, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void Average_GroupExceptTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group except RefDate, Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 703.75m, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 247.4m, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 225m, dataPointEnumerator.Current[1]);
                var d1 = Decimal.Round((Decimal) (NumberType) dataPointEnumerator.Current[2], 10);
                Assert.AreEqual((NumberType) 373.3333333333m, d1);
            }
        }

        [TestMethod]
        public void Average_OfZero()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_2 group by Id);", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 0m, dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Average_ResultIsNumberType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group except RefDate, Id);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual("Value1", result.DataSetComponents[1].Name);
            Assert.IsTrue(result.DataSetComponents[1].DataType == typeof(NumberType));
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.IsTrue(dataPointEnumerator.Current[1] is NumberType);
            }
        }
    }
}
