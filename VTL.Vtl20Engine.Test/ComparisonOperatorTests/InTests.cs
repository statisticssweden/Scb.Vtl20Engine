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
    public class InTests
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
                                new StringType("BS"),
                                new IntegerType(0)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("GZ"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("SQ"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("MO"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("FJ"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("CQ"),
                                new IntegerType(2)
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
                                new StringType("BS"),
                                new NumberType((NumberType)0.05m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("GZ"),
                                new NumberType((NumberType)4.07m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("SQ"),
                                new NumberType((NumberType)9.00m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("MO"),
                                new NumberType(((NumberType)6.32m))
                            }
                        ),
                    }))
            };
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void In_DataSetDecimal()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2 in {0.05, 9.00, 1.20};", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[2].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void In_DataSetDecimal_nolldecimaler()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2 in {2.00, 9.00, 10.00};", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[2].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void In_DataSetScalarInteger()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 in {0, 6, 1, 5};", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[2].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void In_ComponentString()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_2 := Id_2 in {\"BS\", \"MO\", \"HH\", \"PP\"}];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataPoints.Length);
            Assert.AreEqual("Me_2", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[3].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[3]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void In_ComponentNumber()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1#Me_1 in {0, 6, 9, 7};", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[2].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }
    }
}