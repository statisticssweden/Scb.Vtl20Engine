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

namespace VTL.Vtl20Engine.Test.BooleanOperatorTests
{
    [TestClass]
    public class XorTests
    {
        private Operand _ds_1, _ds_2;

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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_3",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_4",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(BooleanType))
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
                                new StringType("M"),
                                new IntegerType(15),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(true),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new IntegerType(64),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new IntegerType(65),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(true),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(15),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(64),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(65),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(true),
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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_3",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_4",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(BooleanType))
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
                                new StringType("M"),
                                new IntegerType(15),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new IntegerType(64),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(true),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("M"),
                                new IntegerType(65),
                                new StringType("B"),
                                new StringType("2013"),
                                new BooleanType(true),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(15),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(true),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(64),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("F"),
                                new IntegerType(65),
                                new StringType("U"),
                                new StringType("2013"),
                                new BooleanType(false),
                            }
                        ),
                    }))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Xor_datasets()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 xor DS_2;", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Xor_datasetScalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 xor true;", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Xor_datasetsWithConditions()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1#Id_1 = \"M\" xor DS_1#Id_2 > 30;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(true, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(false, dataPointEnumerator.Current[4] as BooleanType);
            }
        }

    }
}
