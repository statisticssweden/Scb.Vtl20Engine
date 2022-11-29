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

namespace VTL.Vtl20Engine.Test.JoinOperatorTests
{
    [TestClass]
    public class CrossJoinTests
    {
        [TestMethod]
        public void CrossJoin_TwoDatasetsWithoutHomonymousComponents()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    })
                )
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var identifiers = dsr.GetComponentNames().ToArray();
            Assert.AreEqual("Id_1", identifiers[0]);
            Assert.AreEqual("Me_1", identifiers[1]);
            Assert.AreEqual("Id_2", identifiers[2]);
            Assert.AreEqual("Me_2", identifiers[3]);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
            }
        }

        [TestMethod]
        public void CrossJoin_WithRename()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 rename Id_1 to I_d1)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("I_d1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
            }
        }

        [TestMethod]
        public void CrossJoin_SimilarComponentsThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Resultatet av join innehåller flera komponenter med namn Id_1, Me_1", e.Message);
        }

        [TestMethod]
        public void CrossJoin_AmbiguousRenameThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 rename Id_1 to Id_2, Me_1 to Me_2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Flera komponenter med namnet Id_1, Me_1 hittades i datasetet och det oklart vilken som avses.", e.Message);
        }

        [TestMethod]
        public void CrossJoin_Rename()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 rename ds2#Id_1 to Id_2, ds2#Me_1 to Me_2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_2 = result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Id_2"));
            Assert.IsNotNull(id_2);
        }

        [TestMethod]
        public void CrossJoin_WithCalc()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 calc Me_3 := Me_1 + 10)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_3 = result.OriginalIndexOfComponent("Me_3");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);
            }
        }

        [TestMethod]
        public void CrossJoin_WithCalcAndMembership()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1 as a1, ds2 as a2 calc Me_3 := a1#Me_1 + 10); ", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_3 = result.OriginalIndexOfComponent("Me_3");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);
            }
        }

        [TestMethod]
        public void CrossJoin_AsDoesntRename()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("b := cross_join(ds1 as a1, ds2 as a2 calc Me_3 := a1#Me_1 + 10); r <- ds1 + b[keep Me_1];", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;
        }

        [TestMethod]
        public void CrossJoin_WithCalcAndHash()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1 as a1, ds2 as a2 calc Me_3 := a1#Me_1 + a2#Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;
        }

        [TestMethod]
        public void CrossJoin_WithFilter()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 filter Id_1 = \"A\")", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
            }
        }


        [TestMethod]
        public void CrossJoin_WithFilter2()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 filter Id_1 = Id_2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
            }
        }

        [TestMethod]
        public void CrossJoin_WithKeep()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 keep Me_1)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;


            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1.51m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(-1m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[me_1]);
            }

            Assert.IsFalse(result.DataSetComponents.Any(c => c.Name.Equals("Me_2")));
        }

        [TestMethod]
        public void CrossJoin_WithDrop()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 drop ds1#Me_1)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;


            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            Assert.IsFalse(result.DataSetComponents.Any(c => c.Name.Equals("Me_1")));
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(4m), dataPointEnumerator.Current[me_2]);
            }
        }

        [TestMethod]
        public void CrossJoin_SameComponentNamesThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Resultatet av join innehåller flera komponenter med namn Id_1", e.Message);
        }

        [TestMethod]
        public void CrossJoin_SingleDatasetThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("cross_join måste utföras på minst två dataset.", e.Message);
        }

        [TestMethod]
        public void CrossJoin_NonDatasetArgumentThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, 10)", new[] { ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("cross_join kan endast hantera dataset.", e.Message);
        }

        [TestMethod]
        public void CrossJoin_renameWithMembershipAndAlias()
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("A"),
                                new StringType("B"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("C"),
                                new StringType("D"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("E"),
                                new StringType("F"),
                            }
                        ),
                    }
                ))
            };
            var ds_2 = new Operand
            {
                Alias = "DS_2",
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1A",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("B"),
                                new StringType("Q"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("S"),
                                new StringType("T"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new StringType("A"),
                                new StringType("Z"),
                                new StringType("M"),
                            }
                        ),
                    }
                ))

            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(DS_1 as d1, DS_2 as d2 rename d1#Id_1 to Id11, d1#Id_2 to Id12, d2#Id_1 to Id21, d2#Id_2 to Id22, d1#Me_2 to Me12)",
                    new[] { ds_1, ds_2 }).FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(8, result.DataSetComponents.Length);
            Assert.AreEqual(9, result.DataPointCount);
        }

        [TestMethod]
        public void CrossJoin_filterWithMembership()
        {
            var ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_1A",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2A",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1A",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_2A",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("A"),
                                new StringType("B"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("C"),
                                new StringType("D"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("E"),
                                new StringType("F"),
                            }
                        ),
                    }
                ))
            };
            var ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_1B",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2B",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_1B",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Me_2B",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("B"),
                                new StringType("Q"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("S"),
                                new StringType("T"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new StringType("A"),
                                new StringType("Z"),
                                new StringType("M"),
                            }
                        ),
                    }
                ))

            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(DS_1, DS_2 filter DS_1#Id_1A = DS_2#Id_1B);",
                    new[] { ds_1, ds_2 }).FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(8, result.DataSetComponents.Length);
            Assert.AreEqual(4, result.DataPointCount);
        }

        [TestMethod]
        public void CrossJoin_WithFilterCalcAndDrop()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 filter Id_2 = \"M\" calc Me_3 := Me_1 + 10 drop Me_1)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_3 = result.OriginalIndexOfComponent("Me_3");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);
            }
        }

        [TestMethod]
        public void CrossJoin_WithFilterCalcAndKeep()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2 filter Id_2 = \"M\" calc Me_3 := Me_1 + 10 keep Me_3)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            Assert.IsNull(result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Me_1")));
            Assert.IsNull(result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Me_2")));
            var me_3 = result.OriginalIndexOfComponent("Me_3");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(11.51m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(9m), dataPointEnumerator.Current[me_3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(10.9m), dataPointEnumerator.Current[me_3]);
            }
        }

        [TestMethod]
        public void CrossJoin_WrongAliasThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() =>
                sut.Execute("t := cross_join(ds1, ds2); r <- t [filter t1#Id_1 = t#Id_2];", new[] { ds_1, ds_2 }));
            Assert.AreEqual("t1#Id_1 kunde inte hittas.", e.Message);
        }

        [TestMethod]
        public void CrossJoin_MixedUpAliasThrowsException()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() =>
                sut.Execute("t := cross_join(ds1, ds2); r <- t [filter ds1#Id_1 = t#Id_2];", new[] { ds_1, ds_2 }));

            Assert.AreEqual("ds1#Id_1 kunde inte hittas.", e.Message);
        }


        [TestMethod]
        public void CrossJoin_ResultIsIntegerType()
        {

            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                                new IntegerType(9),
                            }
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new IntegerType(4)
                            }
                        ),
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1, ds2)", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual("Me_1", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Me_2", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
        }

        #region Applytests

        [TestMethod]
        public void CrossJoin_Apply()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("D"),
                                new NumberType(1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("E"),
                                new NumberType(null)
                            }
                        ),
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
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
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new NumberType(2m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new NumberType(3m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("O"),
                                new NumberType(4m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("P"),
                                new NumberType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Q"),
                                new NumberType(5m)
                            }
                        )
                    }
                ))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1 as a1, ds2 as a2 apply a1+a2); ", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var componentNames = dsr.GetComponentNames();
            Assert.AreEqual("Id_1", componentNames[0]);
            Assert.AreEqual("Id_2", componentNames[1]);
            Assert.AreEqual("Me_1", componentNames[2]);

            var measureNames = dsr.GetMeasureNames();
            Assert.AreEqual("Me_1", measureNames[0]);

            var identifierNames = dsr.GetIdentifierNames();
            Assert.AreEqual("Id_1", identifierNames[0]);
            Assert.AreEqual("Id_2", identifierNames[1]);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3.51m), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(4.51m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(5.51m), dataPointEnumerator.Current[me_1]);

                //NULL test
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("P"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(6.51m), dataPointEnumerator.Current[me_1]);
                //
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(1m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3m), dataPointEnumerator.Current[me_1]);

                //NULL test
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("P"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                //

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(2.9m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(3.9m), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(4.9m), dataPointEnumerator.Current[me_1]);

                //NULL test
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("P"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                //D
                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                dataPointEnumerator.MoveNext();
                //E
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("P"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
                //
            }
        }

        [TestMethod]
        public void CrossJoin_ApplyIntegerType()
        {
            var ds_1 = new Operand
            {
                Alias = "ds1",
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
                        },
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
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
                                new IntegerType(100)
                            }
                        )
                    }
                ))
            };

            var ds_2 = new Operand
            {
                Alias = "ds2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
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
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("N"),
                                new IntegerType(null)
                            }
                        )
                    }
                ))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- cross_join(ds1 as a1, ds2 as a2 apply a1+a2); ", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            var componentNames = dsr.GetComponentNames();
            Assert.AreEqual("Id_1", componentNames[0]);
            Assert.AreEqual("Id_2", componentNames[1]);
            Assert.AreEqual("Me_1", componentNames[2]);

            var measureNames = dsr.GetMeasureNames();
            Assert.AreEqual("Me_1", measureNames[0]);

            var identifierNames = dsr.GetIdentifierNames();
            Assert.AreEqual("Id_1", identifierNames[0]);
            Assert.AreEqual("Id_2", identifierNames[1]);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(200), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[me_1]);
            }
        }
        #endregion
    }
}
