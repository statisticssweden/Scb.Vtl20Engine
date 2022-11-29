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
    public class RoundTests
    {
        [TestMethod]
        public void Round_NoDecimalRoundDataset()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new NumberType(1.5m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-1.4m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(0.9m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- round(ds)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[1].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(-1), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Round_decimalRoundDataset()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new NumberType(1.51m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(0.9m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- round(ds, 1)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[1].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(1.5m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(-1), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Round_decimalRoundCalc()
        {
            var ds = new Operand
            {
                Alias = "DS_1",
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
                        new MockComponent(typeof(NumberType))
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
                                new IntegerType(10),
                                new StringType("A"),
                                new NumberType(7.5m),
                                new NumberType(5.9m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType(7.1m),
                                new NumberType(5.5m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(36.2m),
                                new NumberType(17.7m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("B"),
                                new NumberType(44.5m),
                                new NumberType(24.3m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- DS_1 [ calc Me_3:=round(Me_1) ];", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(7), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(36), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(45), dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Round_decimalRoundCalcNegativeDecimals()
        {
            var ds = new Operand
            {
                Alias = "DS_1",
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
                        new MockComponent(typeof(NumberType))
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
                                new IntegerType(10),
                                new StringType("A"),
                                new NumberType(7.5m),
                                new NumberType(5.9m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType(7.1m),
                                new NumberType(5.5m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(36.2m),
                                new NumberType(17.7m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("B"),
                                new NumberType(44.5m),
                                new NumberType(24.3m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- DS_1 [ calc Me_3:=round(Me_1, -1) ];", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(10), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(10), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(40), dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(40), dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Round_decimalWithNull()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new NumberType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(0.9m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- round(ds, 1)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(-1.0m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Round_IntegersToDecimal()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
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
                                new IntegerType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new IntegerType(-1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new IntegerType(9)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- round(ds, 1)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[1].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(-1.0m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(9.0m), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Round_Integers()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
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
                                new IntegerType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new IntegerType(-1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new IntegerType(9)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- round(ds)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(-1), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(9), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Round_numDigit_varable()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new NumberType(1.51m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(0.9m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("numdigit := 1; r <- round(ds, numdigit);", new[] { ds })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(1.5m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(-1), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[1]);
            }
        }
    }
}