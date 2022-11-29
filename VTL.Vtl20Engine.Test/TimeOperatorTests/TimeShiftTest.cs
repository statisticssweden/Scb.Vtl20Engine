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
    public class TimeShiftTest
    {
        /// <summary>
        /// VTL Reference manual pg. 144
        /// </summary>
        [TestMethod]
        public void TimeShift_Example3()
        {
            var ds_3 = new Operand
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Annual, 0),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2011, Duration.Annual, 0),
                                new StringType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2012, Duration.Annual, 0),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2013, Duration.Annual, 0),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2010, Duration.Annual, 0),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2011, Duration.Annual, 0),
                                new StringType("hi")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2012, Duration.Annual, 0),
                                new StringType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new TimePeriodType(2013, Duration.Annual, 0),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- timeshift(DS_3, 1);", new[] { ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2014, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hi"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2013, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2014, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[2]);
            }
        }

        /// <summary>
        /// VTL Reference manual pg. 144
        /// </summary>
        [TestMethod]
        public void TimeShift_Example4()
        {
            var ds_4 = new Operand
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Annual, 0),
                                new StringType("hello world")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2011, Duration.Annual, 0),
                                new StringType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2012, Duration.Annual, 0),
                                new StringType("say hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2013, Duration.Annual, 0),
                                new StringType("he")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 1),
                                new StringType("hi, hello!")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 2),
                                new StringType("hi")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 3),
                                new StringType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new TimePeriodType(2010, Duration.Quarter, 4),
                                new StringType("hello!")
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- timeshift(DS_4, -1);", new[] { ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2009, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hello world"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2009, Duration.Quarter, 4), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hi, hello!"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 1), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hi"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 2), dataPointEnumerator.Current[1]);
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2010, Duration.Quarter, 3), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("hello!"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2011, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("say hello"), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new TimePeriodType(2012, Duration.Annual, 0), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("he"), dataPointEnumerator.Current[2]);
            }
        }

        /// <summary>
        /// Det var ingen bugg utan användarfel. Tyckte att det var synd att slänga testfallet som testar
        /// calc, cast, sum, group, rename, time_period, time_shift
        /// </summary>
        [TestMethod]
        public void TimeShift_Bugg128546()
        {
            var ds_4 = new Operand
            {
                Alias = "DS_4",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
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
                                new StringType("A"),
                                new StringType("1981-Q1"),
                                new IntegerType(0)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("1991-Q2"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("1999-Q3"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("2000-Q4"),
                                new IntegerType(3)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("2011-Q1"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("2021-Q2"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("2031-Q3"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("2041-Q4"),
                                new IntegerType(7)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var command = "B := DS_4 [calc identifier ref_cast := cast(Id_2 ,time_period, \"YYYY-\\QQ\")];";
            command = command + " C := timeshift (B , 1);";
            command = command + " D := C [calc identifier cast_tillbaka := cast (ref_cast, string, \"YYYY-\\QQ\")];";
            command = command + "laggad_data <- (sum (D group except ref_cast))  [rename cast_tillbaka to NR_Referensperiod2];";
            var dsr = sut.Execute(command, new[] { ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("laggad_data"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("1981-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("1981-Q2"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("1991-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("1991-Q3"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(1), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("1999-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("1999-Q4"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(2), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2000-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("2001-Q1"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2011-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("2011-Q2"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2021-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("2021-Q3"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2031-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("2031-Q4"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(6), dataPointEnumerator.Current[3]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2041-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("2042-Q1"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(7), dataPointEnumerator.Current[3]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }
    }
}
