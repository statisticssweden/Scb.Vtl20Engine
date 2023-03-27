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

namespace VTL.Vtl20Engine.Test.DataValidationOperatorTests
{
    [TestClass]
    public class CheckTests
    {
        [TestMethod]
        public void Check_Example()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(25)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(40)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(65)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- check (DS_1 >= DS_2 imbalance DS_1 - DS_2);", new Operand[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-25), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-8), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2012"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2012"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-15), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2015"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2015"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

            }
        }

        [TestMethod]
        public void Check_NonMatchingDatapointsThrowsException()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(25)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(40)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(65)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- check (DS_1 >= DS_2 imbalance DS_2);", new Operand[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var exception = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Bara matchande datapunkter för operand och imbalance får förekomma i check.", exception.Message);
        }

        [TestMethod]
        public void Check_ExampleWithErrorCodeWithoutImbalance()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(25)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(40)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(65)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- check (DS_1 >= DS_2 errorcode \"generalfel\" errorlevel \"error\");", new Operand[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new StringType("generalfel"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new StringType("error"), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new StringType("generalfel"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new StringType("error"), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2012"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2012"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new StringType("generalfel"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new StringType("error"), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new StringType("generalfel"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new StringType("error"), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2015"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2015"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

            }
        }

        [TestMethod]
        public void Check_opHasExactlyOneMeasure()
        {
            var ds_1 = new Operand
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
                        },
                        new MockComponent(typeof(IntegerType))
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1),
                                new IntegerType(1)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var exception = Assert.ThrowsException<VtlException>(() => sut.Execute
            ("DS_r <- check (DS_1 errorcode \"generalfel\" errorlevel \"error\");", new Operand[] { ds_1 }));
            Assert.AreEqual("Operanden måste ha exakt en measurekomponent.", exception.Message);
        }

        [TestMethod]
        public void Check_imbalanceHasExactlyOneMeasure()
        {
            var ds_1 = new Operand
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
                        },
                        new MockComponent(typeof(IntegerType))
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1),
                                new IntegerType(1)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1),
                                new IntegerType(1)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var exception = Assert.ThrowsException<VtlException>(() => sut.Execute
            ("DS_r <- check (DS_1 < DS_2 errorcode \"generalfel\" errorlevel \"error\" imbalance DS_1);", new Operand[] { ds_1, ds_2 }));
            Assert.AreEqual("Imbalance måste ha exakt en measurekomponent.", exception.Message);
        }

        [TestMethod]
        public void Check_opAndImbalanceHasSameIdentifiers()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                            Name = "Id_3",
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1),
                                new IntegerType(1)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var exception = Assert.ThrowsException<VtlException>(() => sut.Execute
            ("DS_r <- check (DS_1 errorcode \"generalfel\" errorlevel \"error\" imbalance DS_2);", new Operand[] { ds_1, ds_2 }));
            Assert.AreEqual("Operand och imbalance måste ha samma identifiers.", exception.Message);
        }

        [TestMethod]
        public void Check_Invalid()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(25)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(null)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(40)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(65)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- check (DS_1 >= DS_2 imbalance DS_1 - DS_2 invalid);", new Operand[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-25), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-8), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-15), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }

        [TestMethod]
        public void Check_ExampleIntegerErrorCodeAndErrorLevel()
        {
            var ds_1 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(4)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(25)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var ds_2 = new Operand
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
                                new StringType("2010"),
                                new StringType("I"),
                                new IntegerType(9)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("I"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("I"),
                                new IntegerType(10)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("I"),
                                new IntegerType(7)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("I"),
                                new IntegerType(5)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("I"),
                                new IntegerType(6)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2010"),
                                new StringType("D"),
                                new IntegerType(50)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2011"),
                                new StringType("D"),
                                new IntegerType(35)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("D"),
                                new IntegerType(40)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("D"),
                                new IntegerType(55)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("D"),
                                new IntegerType(65)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("D"),
                                new IntegerType(75)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- check (DS_1 >= DS_2 errorcode 404 errorlevel 5 invalid);", new Operand[] { ds_1, ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataSetComponents.Length);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[errorcode].DataType);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[errorlevel].DataType);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new IntegerType(404), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new IntegerType(404), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2013"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new IntegerType(404), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2014"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.AreEqual(new IntegerType(404), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[errorlevel]);

                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }
        }
    }
}
