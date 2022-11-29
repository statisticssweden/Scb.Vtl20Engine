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

namespace VTL.Vtl20Engine.Test.AggregateOperatorTests
{
    [TestClass]
    public class GroupAllTimeAggTest
    {
        /// <summary>
        /// VTL Reference manual pg. 147
        /// </summary>
        [TestMethod]
        public void GroupAllTimeAggTest_Example1()
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

            var dsr = sut.Execute("DS_r <- sum(DS_1 group all time_agg(\"A\", _, Me_1));", new[] { ds_1 })
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
        public void GroupAllTimeAggTest_Avg()
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
                                new IntegerType(30)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 3),
                                new StringType("A"),
                                new IntegerType(40)
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
                                new IntegerType(70)
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
                                new IntegerType(30)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- avg(DS_1 group all time_agg(\"A\", _, Me_1));", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(30), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(60), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(20), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void GroupAllTimeAggTest_Min()
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
                                new IntegerType(30)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 3),
                                new StringType("A"),
                                new IntegerType(40)
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
                                new IntegerType(70)
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
                                new IntegerType(30)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- min(DS_1 group all time_agg(\"A\", _, Me_1));", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(20), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(50), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(10), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void GroupAllTimeAggTest_Max()
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
                                new IntegerType(30)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new TimePeriodType(2010, Duration.Quarter, 3),
                                new StringType("A"),
                                new IntegerType(40)
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
                                new IntegerType(70)
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
                                new IntegerType(30)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- max(DS_1 group all time_agg(\"A\", _, Me_1));", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(40), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(70), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new TimePeriodType(2010, Duration.Annual, 0), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new IntegerType(30), dataPointEnumerator.Current[2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

    }
}
