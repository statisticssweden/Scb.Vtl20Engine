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

namespace VTL.Vtl20Engine.Test.ComparisonOperatorTests
{
    [TestClass]
    public class ExistsInTests
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
                        new MockComponent(typeof(IntegerType))
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
                                new StringType("2012"),
                                new StringType("B"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(11094850)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("G"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(55435435)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("S"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(345435)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("M"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(3453454)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("F"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(6786345)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("W"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(2342421)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("Z"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new IntegerType(2342421)
                            }
                        )
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_4",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new StringType("2012"),
                                new StringType("B"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new NumberType(0.023m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("G"),
                                new StringType("Total"),
                                new StringType("M"),
                                new NumberType(0.286m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("S"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new NumberType(0.064m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("M"),
                                new StringType("Total"),
                                new StringType("M"),
                                new NumberType(0.043m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("F"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new NullType()
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("W"),
                                new StringType("Total"),
                                new StringType("Total"),
                                new NumberType(0.08m)
                            }
                        )
                    }))
            };
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void ExistsIn_Example1()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- exists_in(DS_1, DS_2, all);", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(7, result.DataPoints.Length);
            var id2Index = result.IndexOfComponent("Id_2");
            var bool_varIndex = result.IndexOfComponent("bool_var");
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[bool_varIndex].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"B", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"F", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"G", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"M", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"S", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"W", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Z", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void ExistsIn_Example2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- exists_in(DS_1, DS_2, true);", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            var id2Index = result.IndexOfComponent("Id_2");
            var bool_varIndex = result.IndexOfComponent("bool_var");
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[bool_varIndex].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"B", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"F", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"S", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"W", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[bool_varIndex]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void ExistsIn_Example3()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- exists_in(DS_1, DS_2, false);", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataPoints.Length);
            var id2Index = result.IndexOfComponent("Id_2");
            var bool_varIndex = result.IndexOfComponent("bool_var");
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[bool_varIndex].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"G", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"M", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Z", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[bool_varIndex]);

            }
        }
    }
}