using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.TimeOperatorTests
{
    [TestClass]
    public class TimeAggTest
    {
        /// <summary>
        /// VTL Reference manual pg. 147
        /// </summary>
        [TestMethod]
        public void TimeAggTest_Example2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- time_agg(\"Q\", cast(\"2012M01\", time_period, \"YYYY\\MMM\" ));", new Operand[0])
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as TimePeriodType;
            
            Assert.AreEqual(new TimePeriodType(2012, Duration.Quarter, 1), result);

        }

        /// <summary>
        /// VTL Reference manual pg. 147
        /// </summary>
        [TestMethod]
        public void TimeAggTest_Example3()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- time_agg(\"Q\", _, cast(\"20120213\", date, \"YYYYMMDD\"), last);", new Operand[0])
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DateType;

            Assert.AreEqual(new DateType(new DateTime(2012, 3, 31)), result);
        }

        /// <summary>
        /// VTL Reference manual pg. 147
        /// </summary>
        [TestMethod]
        public void TimeAggTest_Example4()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- time_agg(\"A\", _, cast(\"2012M1\", date, \"YYYY\\MM\"), first);", new Operand[0])
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DateType;

            Assert.AreEqual(new DateType(new DateTime(2012, 1, 1)), result);
        }

        [TestMethod]
        public void TimeAggTest_DataSet_TimePeriod()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(TimePeriodType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 1),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 2),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 3),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 1),
                                new StringType("B"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 2),
                                new StringType("B"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 1),
                                new StringType("C"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 2),
                                new StringType("C"),
                                new IntegerType(10)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- time_agg(\"A\", DS_1);", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(60), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(100), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(20), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void TimeAggTest_DataSet_Date()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(DateType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 1, 1)),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 2, 5)),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 1, 20)),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 2, 1)),
                                new StringType("A"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 4, 1)),
                                new StringType("A"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 2, 24)),
                                new StringType("A"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new DateType(new DateTime(2010, 4, 10)),
                                new StringType("A"),
                                new IntegerType(10)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- time_agg(\"M\", DS_1);", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new DateType(new DateTime(2010, 1, 1)), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(40), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new DateType(new DateTime(2010, 2, 1)), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(80), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new DateType(new DateTime(2010, 4, 1)), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(60), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void TimeAggTest_DataSet_Time()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(TimeType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 7, 1),
                                        new DateTime(2010, 7, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("B"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("B"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("C"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("C"),
                                new IntegerType(10)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- time_agg(\"A\", DS_1);", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(60), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(100), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(20), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void TimeAggTest_DataSet_TimeQ()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(TimeType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 7, 1),
                                        new DateTime(2010, 7, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(20)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 1, 1),
                                        new DateTime(2010, 1, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimeType(new Tuple<DateTime, DateTime>(
                                        new DateTime(2010, 4, 1),
                                        new DateTime(2010, 4, 7)
                                    )),
                                new StringType("A"),
                                new IntegerType(10)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- time_agg(\"Q\", DS_1);", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 1, 1), new DateTime(2010, 3, 31))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(80), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 4, 1), new DateTime(2010, 6, 30))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(80), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>
                    (new DateTime(2010, 7, 1), new DateTime(2010, 9, 30))), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(20), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

    }
}
