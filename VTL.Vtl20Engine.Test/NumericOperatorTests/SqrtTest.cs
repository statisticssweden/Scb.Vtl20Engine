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

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class SqrtTest
    {
        private Operand _ds;

        [TestInitialize]
        public void Init()
        {
            _ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
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
                        },
                        new MockComponent(typeof(NumberType))
                        {
                        Name = "Me_2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("10"),
                                new StringType("A"),
                                new IntegerType(16),
                                new NumberType(0.7545m),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("10"),
                                new StringType("B"),
                                new IntegerType(81),
                                new NumberType(13.45m),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("11"),
                                new StringType("A"),
                                new IntegerType(64),
                                new NumberType(1.87m),
                            }
                        )
                    }))
            };
        }

        [TestMethod]
        public void Sqrt_Example1()
        {

            var x = new Operand[1] { _ds };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- sqrt(ds)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[2]);
                Assert.AreEqual(0.86862m, Math.Round((decimal)(NumberType)dataPointEnumerator.Current[3], 5));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(9), dataPointEnumerator.Current[2]);
                Assert.AreEqual(3.667424m, Math.Round((decimal)(NumberType)dataPointEnumerator.Current[3], 6));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[2]);
                Assert.AreEqual(1.367479m, Math.Round((decimal)(NumberType)dataPointEnumerator.Current[3], 6));

            }
        }

        [TestMethod]
        public void Sqrt_Example2_on_components()
        {

            var x = new Operand[1] { _ds };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <-  ds [ calc Me_1 := sqrt ( Me_1 ) ]", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[2]);
                Assert.AreEqual(0.7545m, (decimal)(NumberType)dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(9), dataPointEnumerator.Current[2]);
                Assert.AreEqual(13.45m, (decimal)(NumberType)dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[2]);
                Assert.AreEqual(1.87m, (decimal)(NumberType)dataPointEnumerator.Current[3]);

            }
        }

        [TestMethod]
        public void Sqrt_Zero()
        {
            var ds  = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
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
                        },
                        new MockComponent(typeof(NumberType))
                        {
                        Name = "Me_2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("10"),
                                new StringType("A"),
                                new IntegerType(0),
                                new NumberType(0m),
                            }
                        )
                    }))
            };

            var x = new Operand[1] {ds};

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- sqrt(ds)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[3]);

            }
        }

        [TestMethod]
        public void Sqrt_null()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
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
                        },
                        new MockComponent(typeof(NumberType))
                        {
                        Name = "Me_2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("10"),
                                new StringType("A"),
                                new IntegerType(null),
                                new NumberType(null),
                            }
                        )
                    }))
            };

            var x = new Operand[1] { ds };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- sqrt(ds)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[3]);

            }
        }

        [TestMethod]
        public void Sqrt_NegativeNumber()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
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
                        },
                        new MockComponent(typeof(NumberType))
                        {
                        Name = "Me_2",
                        Role = ComponentType.ComponentRole.Measure
                    }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("10"),
                                new StringType("A"),
                                new IntegerType(-9),
                                new NumberType(-6.25m),
                            }
                        )
                    }))
            };

            var x = new Operand[1] { ds };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- sqrt(ds)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Man kan inte dra kvadratroten ur ett negativt tal", e.Message);

        }

    }
}