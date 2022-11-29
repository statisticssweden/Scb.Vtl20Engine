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
    public class TruncTests
    {
        [TestMethod]
        public void Trunc_Example1()
        {
            var ds_1 = new Operand
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

            var ds_r = sut.Execute("DS_r <- trunc(DS_1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(7), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(7), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(36), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new IntegerType(17), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(44), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new IntegerType(24), dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void Trunc_Example2()
        {
            var ds_1 = new Operand
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

            var ds_r = sut.Execute("DS_r <- DS_1[calc Me_10 := trunc(Me_1)]", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i = result.IndexOfComponent("Me_10");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(7), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(7), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(36), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(44), dataPointEnumerator.Current[i]);
            }
        }

        [TestMethod]
        public void Trunc_Example3()
        {
            var ds_1 = new Operand
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

            var ds_r = sut.Execute("DS_r <- DS_1[calc Me_20 := trunc(Me_1, -1)]", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i = result.IndexOfComponent("Me_20");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(30), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(40), dataPointEnumerator.Current[i]);
            }
        }

        [TestMethod]
        public void Trunc_One_decimal()
        {
            var ds_1 = new Operand
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
                                new NumberType(7.55m),
                                new NumberType(5.99m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType(7.11m),
                                new NumberType(5.55m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(36.22m),
                                new NumberType(17.77m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("B"),
                                new NumberType(44.55m),
                                new NumberType(24.33m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var ds_r = sut.Execute("DS_r <- trunc(DS_1, 1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i1 = result.IndexOfComponent("Me_1");
            var i2 = result.IndexOfComponent("Me_2");

            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[i1].DataType);
            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[i2].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(7.5m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(5.9m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(7.1m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(5.5m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(36.2m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(17.7m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(44.5m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(24.3m), dataPointEnumerator.Current[i2]);
            }
        }

        [TestMethod]
        public void Trunc_Integer()
        {
            var ds_1 = new Operand
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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
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
                                new NumberType(75m),
                                new NumberType(59m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType(71m),
                                new NumberType(55m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(362m),
                                new NumberType(177m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("B"),
                                new NumberType(445m),
                                new NumberType(243m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var ds_r = sut.Execute("DS_r <- trunc(DS_1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i1 = result.IndexOfComponent("Me_1");
            var i2 = result.IndexOfComponent("Me_2");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(75m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(59m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(71m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(55m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(362m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(177m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(445m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(243m), dataPointEnumerator.Current[i2]);
            }
        }

        [TestMethod]
        public void Trunc_Integer_ten()
        {
            var ds_1 = new Operand
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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
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
                                new NumberType(755m),
                                new NumberType(599m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType(711m),
                                new NumberType(555m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(3622m),
                                new NumberType(1777m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("B"),
                                new NumberType(4455m),
                                new NumberType(2433m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var ds_r = sut.Execute("DS_r <- trunc(DS_1, -1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i1 = result.IndexOfComponent("Me_1");
            var i2 = result.IndexOfComponent("Me_2");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(750m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(590m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(710m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(550m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(3620m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(1770m), dataPointEnumerator.Current[i2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4450m), dataPointEnumerator.Current[i1]);
                Assert.AreEqual(new NumberType(2430m), dataPointEnumerator.Current[i2]);
            }
        }

        [TestMethod]
        public void Trunc_Null()
        {
            var ds_1 = new Operand
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
                                new IntegerType(null),
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
                                new NumberType(null),
                                new NumberType(null)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var ds_r = sut.Execute("DS_r <- trunc(DS_1, -1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i = result.IndexOfComponent("Me_1");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(30), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[i].HasValue());
            }
        }

        [TestMethod]
        public void Trunc_numDigit_variable()
        {
            var ds_1 = new Operand
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

            var ds_r = sut.Execute("numDigit := -1; DS_r <- DS_1[calc Me_20 := trunc(Me_1, numDigit)];", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = ds_r.GetValue() as DataSetType;

            var i = result.IndexOfComponent("Me_20");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(30), dataPointEnumerator.Current[i]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(40), dataPointEnumerator.Current[i]);
            }
        }

    }
}