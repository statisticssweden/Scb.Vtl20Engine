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

namespace VTL.Vtl20Engine.Test.JoinOperatorTests
{
    [TestClass]
    public class InnerJoinTests
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
        public void InnerJoin_Example1()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2 keep Me_1, d2#Me_2, Me_1A);", new[] { ds_1, ds_2 })
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

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Example5()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute(@"DS_r <- inner_join(DS_1 as d1, DS_2 as d2 filter Me_1 = ""A"" calc Me_4 := Me_1 || Me_1A drop d1#Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_1A = result.OriginalIndexOfComponent("Me_1A");
            var me_4 = result.OriginalIndexOfComponent("Me_4");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);
                Assert.AreEqual(new StringType("AB"), dataPointEnumerator.Current[me_4]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Example6()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 filter Id_2 = \"B\" calc Me_2 := Me_2 || \"_NEW\" keep Me_1, Me_2 );", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("D_NEW"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Example7()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_22 := DS_2[rename Me_1A to Me_1];DS_r <- inner_join ( DS_1 as d1, DS_22 as d2 apply d1 || d2 );", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("AB"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("BQ"), dataPointEnumerator.Current[me_2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("CS"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("DT"), dataPointEnumerator.Current[me_2]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_NonMatchingComponents()
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

            var dsr = sut.Execute("DS_r <- inner_join ( DS_1, DS_2 );", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(7, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var id_3 = result.OriginalIndexOfComponent("Id_3");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_1a = result.OriginalIndexOfComponent("Me_1A");
            var me_2a = result.OriginalIndexOfComponent("Me_2A");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("AA"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_3]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1a]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2a]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("BA"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_3]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1a]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2a]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }
    
        [TestMethod]
        public void InnerJoin_FilterThenKeep()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join ( DS_1, DS_2 filter Id_1 = 1 keep DS_2#Me_2);", new[] { ds_1, ds_2 })
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

        public void InnerJoin_FilterThenKeepWithAlias()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 keep d2#Me_2);", new[] { ds_1, ds_2 })
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
        public void InnerJoin_FilterThenDrop()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join ( DS_1, DS_2 filter Id_1 = 1 drop DS_1#Me_1, DS_1#Me_2, DS_2#Me_1A);", new[] { ds_1, ds_2 })
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

        public void InnerJoin_FilterThenDropWithAlias()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 drop d1#Me_1, d1#Me_2, d2#Me_1A);", new[] { ds_1, ds_2 })
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
        public void InnerJoin_ThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- inner_join ( DS_1 as d1, DS_2 as d2 filter Id_1 = 1 keep Me_2);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));
        }

        [TestMethod]
        public void InnerJoin_AfterCount1()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("A := count(DS_1 group by Id_1); DS_r <- inner_join ( A, DS_2 rename DS_2#Me_2 to Me_2A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_1A = result.OriginalIndexOfComponent("Me_1A");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_2A = result.OriginalIndexOfComponent("Me_2A");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1A]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2A]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_AfterCount2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("A := count(DS_1 group by Id_1); DS_r <- inner_join ( DS_2, A rename DS_2#Me_2 to Me_2A);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var me_1A = result.OriginalIndexOfComponent("Me_1A");
            var me_2 = result.OriginalIndexOfComponent("Me_2");
            var me_2A = result.OriginalIndexOfComponent("Me_2A");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1A]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2A]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_ThrowsExceptionWhenStructureNotCompatible()
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

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- inner_join ( DS_1, DS_2 );", new[] { ds_1, ds_2 }));

        }

        [TestMethod]
        public void InnerJoin_Using()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2 using Id_1 keep Me_1 rename d2#Id_2 to d2#Id_22);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(3, dsr.GetIdentifierNames().Length);
            Assert.AreEqual(1, dsr.GetMeasureNames().Length);
            Assert.AreEqual(4, dsr.GetComponentNames().Length);
            Assert.AreEqual(4, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var id_22 = result.OriginalIndexOfComponent("Id_22");
            var me_1 = result.OriginalIndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Using2()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2 using Id_1, Id_2 keep Me_1, d2#Me_2, Me_1A);", new[] { ds_1, ds_2 })
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
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[me_1A]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[me_2]);
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[me_1A]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Using3()
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

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2 using Id_1 keep Me_1 rename d2#Id_2 to d2#Id_22);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var id_22 = result.OriginalIndexOfComponent("Id_22");
            var me_1 = result.OriginalIndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Using4()
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

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2 using Id_1 keep Me_1 rename d2#Id_2 to d2#Id_22);", new[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var id_22 = result.OriginalIndexOfComponent("Id_22");
            var me_1 = result.OriginalIndexOfComponent("Me_1");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[me_1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_22]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[me_1]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void InnerJoin_Using5()
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
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
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

            var ds_3 = new Operand
            {
                Alias = "DS_3",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Id_1",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2C",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Me_1C",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Me_2C",
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

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2, DS_3 as d3 using Id_1);", new[] { ds_1, ds_2, ds_3 }));
        }

        [TestMethod]
        public void InnerJoin_Using6()
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
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
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

            var ds_3 = new Operand
            {
                Alias = "DS_3",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Id_1",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2B",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Me_1C",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Me_2C",
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

            var dsr = sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as d2, DS_3 as d3 using Id_1 rename d3#Id_2B to apa);", new[] { ds_1, ds_2, ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

        }

        [TestMethod]
        public void InnerJoin_using_occupied_alias()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- inner_join(DS_1 as d1, DS_2 as DS_r using Id_1, Id_2 keep Me_1, DS_r#Me_2, Me_1A);", new[] { ds_1, ds_2 }));
        }

    }
}
