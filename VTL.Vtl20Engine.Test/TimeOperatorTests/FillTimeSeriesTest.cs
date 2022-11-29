using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.TimeOperatorTests
{
    [TestClass]
    public class FillTimeSeriesTest
    {
        private Operand _ds_1, _ds_2, _ds_3, _ds_4;

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
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(TimeType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31))),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31))),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                                new StringType("hi!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31))),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };

            _ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(DateType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new DateType(new DateTime(2010, 12, 31)),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new DateType(new DateTime(2012, 12, 31)),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new DateType(new DateTime(2013, 12, 31)),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new DateType(new DateTime(2011, 12, 31)),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new DateType(new DateTime(2012, 12, 31)),
                                new StringType("hi!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new DateType(new DateTime(2014, 12, 31)),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };

            _ds_3 = new Operand
            {
                Alias = "DS_3",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(TimePeriodType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Annual, 1),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2012, Duration.Annual, 1),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2013, Duration.Annual, 1),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2011, Duration.Annual, 1),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2012, Duration.Annual, 1),
                                new StringType("hi!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2014, Duration.Annual, 1),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };

            _ds_4 = new Operand
            {
                Alias = "DS_4",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(TimePeriodType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Annual, 1),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2012, Duration.Annual, 1),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 1),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 2),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 4),
                                new StringType("hi!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2011, Duration.Quarter, 2),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };
        }

        [TestMethod]
        public void FillTimeSeries_Example1()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_1, single );", new Operand[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }


        [TestMethod]
        public void FillTimeSeries_Example2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_1, all );", new Operand[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2010, 1, 1), new DateTime(2010, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2012, 1, 1), new DateTime(2012, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2013, 1, 1), new DateTime(2013, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimeType(new Tuple<DateTime, DateTime>(new DateTime(2014, 1, 1), new DateTime(2014, 12, 31))),
                    dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example3()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_2, single );", new Operand[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2010, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2011, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2012, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2013, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2011, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2012, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2013, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2014, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example4()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_2, all );", new Operand[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2010, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2011, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2012, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2013, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2014, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2010, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2011, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2012, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2013, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new DateType(new DateTime(2014, 12, 31)), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example5()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_3, single );", new Operand[] { _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2014, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example6()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_3, all );", new Operand[] { _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2014, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2014, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example7()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_4, single );", new Operand[] { _ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 2), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 3), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 4), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 2), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FillTimeSeries_Example8()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- fill_time_series( DS_4, all );", new Operand[] { _ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            var id1Index = result.IndexOfComponent("Id_1");
            var id2Index = result.IndexOfComponent("Id_2");
            var me1Index = result.IndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 2), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 3), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 4), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hi!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 2), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 3), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Quarter, 4), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 1), dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[me1Index]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Quarter, 1), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Quarter, 2), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Quarter, 3), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id1Index]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Quarter, 4), dataPointEnumerator.Current[id2Index]);
                Assert.IsFalse(dataPointEnumerator.Current[me1Index].HasValue());

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

    }
}
