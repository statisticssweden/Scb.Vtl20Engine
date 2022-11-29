using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Exceptions;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.JoinOperatorTests
{
    [TestClass]
    public class FullJoinTests
    {
        private Operand ds_1, ds_2;

        [TestInitialize]
        public void Setup()
        {
            ds_1 = new Operand
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
                    }
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
                            new StringType("B")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(1),
                            new StringType("B"),
                            new StringType("C"),
                            new StringType("D")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(2),
                            new StringType("A"),
                            new StringType("E"),
                            new StringType("F")
                        }
                    )
                })
                )
            };

            ds_2 = new Operand
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
                    }
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
                            new StringType("Q")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(1),
                            new StringType("B"),
                            new StringType("S"),
                            new StringType("T")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(3),
                            new StringType("A"),
                            new StringType("Z"),
                            new StringType("M")
                        }
                    )
                })
                )
            };

        }


        [TestMethod]
        public void FullJoin_Example3()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join(DS_1 as d1, DS_2 as d2 keep Me_1, d2#Me_2, Me_1A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(2, dsr.GetIdentifierNames().Length);
            Assert.AreEqual(3, dsr.GetMeasureNames().Length);
            Assert.AreEqual(5, dsr.GetComponentNames().Length);
            Assert.AreEqual(5, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_1A = result.OriginalIndexOfComponent("Me_1A");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[me_1]);
                Assert.IsFalse(dataPointEnumerator.Current[me_2].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[me_1A].HasValue());

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.AreEqual(new IntegerType(3), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("Z"), dataPointEnumerator.Current[me_1A]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }


        [TestMethod]
        public void FullJoin_NotMatchingComponents()
        {
            ds_1 = new Operand
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
                        Name = "Id_3",
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
                    }
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
                            new StringType("B")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(1),
                            new StringType("B"),
                            new StringType("C"),
                            new StringType("D")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new IntegerType(2),
                            new StringType("A"),
                            new StringType("E"),
                            new StringType("F")
                        }
                    )
                })
                )
            };

            ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_3",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Id_1",
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
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("AA"),
                            new StringType("A"),
                            new IntegerType(1),
                            new StringType("B"),
                            new StringType("Q")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("BA"),
                            new StringType("B"),
                            new IntegerType(1),
                            new StringType("S"),
                            new StringType("T")
                        }
                        ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("AA"),
                            new StringType("A"),
                            new IntegerType(3),
                            new StringType("Z"),
                            new StringType("M")
                        }
                    )
                })
                )
            };
            var sut = new VtlEngine(new DataContainerFactory());


            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- full_join ( DS_1, DS_2 );", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));

        }


        [TestMethod]
        public void FullJoin_FilterThenKeep()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join ( DS_1, DS_2 filter Id_1 = 1 keep DS_2#Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FullJoin_FilterThenKeepWithAlias()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 keep d2#Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FullJoin_FilterThenDrop()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join ( DS_1, DS_2 filter Id_1 = 1 drop DS_1#Me_1, DS_1#Me_2, DS_2#Me_1A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FullJoin_FilterThenDropWithAlias()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 drop d1#Me_1, d1#Me_2, d2#Me_1A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void FullJoin_KeepComponentNotFound_ThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- full_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 keep Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));
        }

        [TestMethod]
        public void fullJoin_AfterCount1()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("A := count(DS_1 group by Id_1); DS_r <- full_join ( A, DS_2 rename DS_2#Me_2 to Me_2A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));

        }

        [TestMethod]
        public void FullJoin_ThrowsExceptionWhenStructureNotCompatible()
        {
            ds_1 = new Operand
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
                        }
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
                                new StringType("B")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("C"),
                                new StringType("D")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("E"),
                                new StringType("F")
                            }
                        )
                })
                )
            };

            ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1A",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
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
                            Name = "Me_2A",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("AA"),
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("Q")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("BA"),
                                new IntegerType(1),
                                new StringType("S"),
                                new StringType("T")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("AA"),
                                new IntegerType(3),
                                new StringType("Z"),
                                new StringType("M")
                            }
                        )
                })
                )
            };
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- full_join ( DS_1, DS_2 );", new[] { ds_1, ds_2 }));

        }

        [TestMethod]
        public void fullJoin_UsingNotAllowed()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VTLParserException>(() => sut.Execute("DS_r <- full_join(DS_1 as d1, DS_2 as d2 using Id_1 keep Me_1 rename d2#Id_2 to d2#Id_22);", new[] { ds_1, ds_2 }));

        }





        [TestMethod]
        public void FullJoin_NonMatchingComponents2()
        {

            ds_1 = new Operand
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
                        }
        },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(4),
                                new StringType("A"),
                                new StringType("A"),
                                new StringType("B")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(4),
                                new StringType("B"),
                                new StringType("C"),
                                new StringType("D")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("E"),
                                new StringType("F")
                            }
                        )
                })
                )
            };

            ds_2 = new Operand
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
                        }
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
                                new StringType("Q")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("S"),
                                new StringType("T")
                            }
                            ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new StringType("A"),
                                new StringType("Z"),
                                new StringType("M")
                            }
                        )
                })
                )
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- full_join(DS_1 as d1, DS_2 as d2 keep Me_1, d2#Me_2, Me_1A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(5, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_1A = result.OriginalIndexOfComponent("Me_1A");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[me_1]);
                Assert.IsFalse(dataPointEnumerator.Current[me_2].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[me_1A].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(3), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("Z"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(4), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);
                Assert.IsFalse(dataPointEnumerator.Current[me_2].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[me_1A].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(4), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);
                Assert.IsFalse(dataPointEnumerator.Current[me_2].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[me_1A].HasValue());

                Assert.IsFalse(dataPointEnumerator.MoveNext());

            }
        }

    }
}
