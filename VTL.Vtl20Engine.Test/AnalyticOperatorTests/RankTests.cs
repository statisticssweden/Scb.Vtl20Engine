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

namespace VTL.Vtl20Engine.Test.AnalyticOperatorTests
{
    [TestClass]
    public class RankTests
    {

        private Operand _ds_1;
        private Operand _ds_2;

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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_3",
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2000),
                                new NumberType(3),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2001),
                                new NumberType(4),
                                new NumberType(9),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2002),
                                new NumberType(7),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2003),
                                new NumberType(6),
                                new NumberType(8),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2000),
                                new NumberType(9),
                                new NumberType(3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2001),
                                new NumberType(5),
                                new NumberType(4),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2002),
                                new NumberType(10),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2003),
                                new NumberType(5),
                                new NumberType(7),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2003),
                                new NumberType(null),
                                new NumberType(7),
                            }
                        )
                    }
                ))
            };

            _ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "bokstav",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "ayear",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "tvalue",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "c_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "c_2",
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
                                new IntegerType(2000),
                                new StringType("XX"),
                                new NumberType(3),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2001),
                                new StringType("XX"),
                                new NumberType(4),
                                new NumberType(9),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2002),
                                new StringType("XX"),
                                new NumberType(7),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2003),
                                new StringType("XX"),
                                new NumberType(6),
                                new NumberType(8),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2000),
                                new StringType("YY"),
                                new NumberType(9),
                                new NumberType(3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2001),
                                new StringType("YY"),
                                new NumberType(5),
                                new NumberType(4),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2002),
                                new StringType("YY"),
                                new NumberType(10),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new IntegerType(2003),
                                new StringType("YY"),
                                new NumberType(5),
                                new NumberType(7),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new IntegerType(2003),
                                new StringType("YY"),
                                new NumberType(null),
                                new NumberType(7),
                            }
                        )
                    }
                ))
            };
        }

        [TestMethod]
        public void Rank_Execute()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_2 := rank ( over ( partition by Id_1, Id_2 order by Me_1 ) ) ];",
                    new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
            }
        }


        [TestMethod]
        public void Rank_Execute_asc()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_2 := rank ( over ( partition by Id_1, Id_2 order by Me_1 asc ) ) ];",
                    new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Rank_Execute_desc()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_2 := rank ( over ( partition by Id_1, Id_2 order by Me_1 desc ) ) ];",
                    new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);

            }
        }

        [TestMethod]
        public void Rank_OrderByTwo()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_2 := rank ( over ( partition by Id_1 order by Id_2, Me_1 ) ) ];",
                    new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 8, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Rank_ManyPartitions()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_2 := rank ( over ( partition by Id_3 order by Me_1 ) ) ];",
                    new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Rank_PartitionIndexNotNull()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2 [calc Me_3 := rank ( over ( partition by tvalue order by  ayear) ) ];",
                    new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
            }
        }

        [TestMethod]
        public void Rank_PartitionIndexNotNullTwoOrderByTwo()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_2 [calc Me_3 := rank ( over ( partition by tvalue order by  ayear, bokstav) ) ];",
                    new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[5]);
            }

        }

        [TestMethod]
        public void Rank_PartitionIndexNotNullTwoOrderByTwoDesc()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_2 [calc Me_3 := rank ( over ( partition by tvalue order by  ayear desc, bokstav desc) ) ];",
                    new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 5, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2001, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2002, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 3, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 2, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 2003, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[5]);
            }
        }


        [TestMethod]
        public void Rank_ResultIsIntegerType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS := DS_2[calc bulle := 10]; DS_r <- DS [calc Me_3 := rank ( over ( partition by bulle order by  ayear, bokstav) ) ];",
                    new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.DataSetComponents.First(c => c.Name == "bulle").DataType == typeof(IntegerType));
            Assert.IsTrue(result.DataSetComponents.First(c => c.Name == "Me_3").DataType == typeof(IntegerType));
        }

        [TestMethod]
        public void Rank_PartitionByTimePeriod()
        {
            var ds_t = new Operand
            {
                Alias = "DS_1",
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
                        new MockComponent(typeof(TimePeriodType))
                        {
                            Name = "Id_3",
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2000, Duration.Quarter, 1),
                                new NumberType(3),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2001, Duration.Quarter, 1),
                                new NumberType(4),
                                new NumberType(9),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2002, Duration.Quarter, 1),
                                new NumberType(7),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2003, Duration.Quarter, 1),
                                new NumberType(6),
                                new NumberType(8),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2000, Duration.Quarter, 1),
                                new NumberType(9),
                                new NumberType(3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2001, Duration.Quarter, 1),
                                new NumberType(5),
                                new NumberType(4),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2002, Duration.Quarter, 1),
                                new NumberType(10),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2003, Duration.Quarter, 1),
                                new NumberType(5),
                                new NumberType(7),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2003, Duration.Quarter, 1),
                                new NumberType(null),
                                new NumberType(7),
                            }
                        )
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_3 := rank ( over ( partition by Id_3 order by  Id_1, Id_2) ) ];",
                    new[] { ds_t })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2001, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)9, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2002, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)8, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();

                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2001, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2002, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);
            }
        }

        [TestMethod]
        public void Rank_OrderByTimePeriod()
        {
            var ds_t = new Operand
            {
                Alias = "DS_1",
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
                        new MockComponent(typeof(TimePeriodType))
                        {
                            Name = "Id_3",
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2000, Duration.Quarter, 1),
                                new NumberType(3),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2001, Duration.Quarter, 1),
                                new NumberType(4),
                                new NumberType(9),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2002, Duration.Quarter, 1),
                                new NumberType(7),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new TimePeriodType(2003, Duration.Quarter, 1),
                                new NumberType(6),
                                new NumberType(8),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2000, Duration.Quarter, 1),
                                new NumberType(9),
                                new NumberType(3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2001, Duration.Quarter, 1),
                                new NumberType(5),
                                new NumberType(4),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2002, Duration.Quarter, 1),
                                new NumberType(10),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2003, Duration.Quarter, 2),
                                new NumberType(5),
                                new NumberType(7),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new TimePeriodType(2003, Duration.Quarter, 1),
                                new NumberType(null),
                                new NumberType(7),
                            }
                        )
                    }
                ))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut
                .Execute("DS_r <- DS_1 [calc Me_3 := rank ( over ( partition by Id_2 order by  Id_3) ) ];",
                    new[] { ds_t })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2001, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)9, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2002, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)6, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)8, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[5]);
                dataPointEnumerator.MoveNext();

                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)9, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2001, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2002, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)10, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)3, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 1), dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)4, dataPointEnumerator.Current[5]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[1]);
                Assert.AreEqual(new TimePeriodType(2003, Duration.Quarter, 2), dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)7, dataPointEnumerator.Current[4]);
                Assert.AreEqual((IntegerType)5, dataPointEnumerator.Current[5]);
            }
        }
    }
}
