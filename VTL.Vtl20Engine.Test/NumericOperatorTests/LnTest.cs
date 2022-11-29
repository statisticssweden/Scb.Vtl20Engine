using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class LnTest
    {
        [TestMethod]
        public void Ln_Example1()
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
                                new NumberType((decimal)148.413),
                                new NumberType((decimal)0.7545)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType((decimal)2980.95),
                                new NumberType((decimal)13.45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType((decimal)7.38905),
                                new NumberType((decimal)1.87)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- ln(DS_1)", new Operand[] { ds_1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(5, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(-0.2817, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(8, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(2.598979, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(2, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(0.625938, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);
            }
        }

        [TestMethod]
        public void Ln_Example2()
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
                                new NumberType((decimal)148.413),
                                new NumberType((decimal)0.7545)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new NumberType((decimal)2980.95),
                                new NumberType((decimal)13.45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType((decimal)7.38905),
                                new NumberType((decimal)1.87)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- DS_1[calc Me_2 := ln(DS_1#Me_1)]", new Operand[] { ds_1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(148.413, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(5.0, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(2980.95, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(8.0, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(7.38905, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(2.0, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);
            }
        }

        [TestMethod]
        public void Ln_Null()
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
                                new NumberType((decimal)148.413),
                                new NumberType((decimal)0.7545)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(null),
                                new StringType("B"),
                                new NumberType((decimal)2980.95),
                                new NumberType((decimal)13.45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new NumberType(null),
                                new NumberType((decimal)1.87)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- ln(DS_1)", new Operand[] { ds_1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(8, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(2.598979, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(5, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(-0.2817, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());
                Assert.AreEqual(0.625938, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.00001);
            }
        }


        [TestMethod]
        public void Ln_Integer()
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
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new IntegerType(3)
                            }
                        ),
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- ln(DS_1)", new Operand[] { ds_1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(0, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(0.69314718056, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(1.09861228867, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
            }
        }

    }
}