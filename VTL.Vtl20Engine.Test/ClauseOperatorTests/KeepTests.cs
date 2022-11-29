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

namespace VTL.Vtl20Engine.Test.ClauseOperatorTests
{
    [TestClass]
    public class KeepTests
    {
        private Operand _ds_1;

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
                            Name = "RefDate",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "MeasName",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800),
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000),
                                new IntegerType(400)
                            }
                        )
                    }))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_KeepOneComponent()
        {

            var DS_1 = new Operand
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
                            Name = "Id_3",
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
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "At_1",
                            Role = ComponentType.ComponentRole.Attribure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(20),
                                new IntegerType(36),
                                new StringType("E"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(4),
                                new IntegerType(9),
                                new StringType("F"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("B"),
                                new StringType("XX"),
                                new IntegerType(9),
                                new IntegerType(10),
                                new StringType("F"),
                            })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[keep Me_1];", new[] { DS_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataSetComponents.Length);
            Assert.AreEqual("Id_1", result.DataSetComponents[0].Name);
            Assert.AreEqual("Id_2", result.DataSetComponents[1].Name);
            Assert.AreEqual("Id_3", result.DataSetComponents[2].Name);
            Assert.AreEqual("Me_1", result.DataSetComponents[3].Name);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_KeepTwoComponents()
        {

            var DS_1 = new Operand
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
                            Name = "Id_3",
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
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "At_1",
                            Role = ComponentType.ComponentRole.Attribure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(20),
                                new IntegerType(36),
                                new StringType("E"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(4),
                                new IntegerType(9),
                                new StringType("F"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("B"),
                                new StringType("XX"),
                                new IntegerType(9),
                                new IntegerType(10),
                                new StringType("F"),
                            })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[keep Me_1, At_1];", new[] { DS_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DataSetComponents.Length);
            Assert.AreEqual("Id_1", result.DataSetComponents[0].Name);
            Assert.AreEqual("Id_2", result.DataSetComponents[1].Name);
            Assert.AreEqual("Id_3", result.DataSetComponents[2].Name);
            Assert.AreEqual("Me_1", result.DataSetComponents[3].Name);
            Assert.AreEqual("At_1", result.DataSetComponents[4].Name);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_KeepUnknownComponent()
        {

            var DS_1 = new Operand
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
                            Name = "Id_3",
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
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "At_1",
                            Role = ComponentType.ComponentRole.Attribure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(20),
                                new IntegerType(36),
                                new StringType("E"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(4),
                                new IntegerType(9),
                                new StringType("F"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("B"),
                                new StringType("XX"),
                                new IntegerType(9),
                                new IntegerType(10),
                                new StringType("F"),
                            })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_1[keep Toffla];", new[] { DS_1 }));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_PreservesOriginalDataSet()
        {

            var DS_1 = new Operand
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
                            Name = "Id_3",
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
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "At_1",
                            Role = ComponentType.ComponentRole.Attribure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(20),
                                new IntegerType(36),
                                new StringType("E"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(4),
                                new IntegerType(9),
                                new StringType("F"),
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2010),
                                new StringType("B"),
                                new StringType("XX"),
                                new IntegerType(9),
                                new IntegerType(10),
                                new StringType("F"),
                            })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var calculation = sut.Execute("DS_r <- DS_1[keep Me_1]; DS_1r <- DS_1;", new[] { DS_1 });
            var DS_r = calculation.FirstOrDefault(r => r.Alias.Equals("DS_r")).GetValue() as DataSetType;
            var DS_1r = calculation.FirstOrDefault(r => r.Alias.Equals("DS_1r")).GetValue() as DataSetType;

            Assert.IsNotNull(DS_r);
            Assert.AreEqual(4, DS_r.DataSetComponents.Length);
            Assert.AreEqual("Me_1", DS_r.DataSetComponents[3].Name);

            Assert.IsNotNull(DS_1r);
            Assert.AreEqual(6, DS_1r.DataSetComponents.Length);
            Assert.AreEqual("At_1", DS_1r.DataSetComponents[5].Name);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_MultilineExpression()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"temp := DS_1 + 100; DS_r <- temp[keep Value1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_MultilinesExpression()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var vtlCode = @"temp := DS_1 + 100;";
            vtlCode += " DS_2 <- temp[keep Value1];";
            vtlCode += " DS_3 <- DS_2[rename Value1 to apa];";
            vtlCode += " DS_r <- DS_3[calc Value2 := apa + 200];";
            var DS_r = sut.Execute(vtlCode, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("apa", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("Value2", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_ComponentCountTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 + 100; DS_r <- temp[keep Value1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ids = DS_r.GetIdentifierNames();
            Assert.AreEqual(2, ids.Length);
            Assert.IsTrue(ids.Contains("RefDate"));
            Assert.IsTrue(ids.Contains("MeasName"));

            var meas = DS_r.GetMeasureNames();
            Assert.AreEqual(1, meas.Length);
            Assert.IsTrue(meas.Contains("Value1"));

            var comp = DS_r.GetComponentNames();
            Assert.AreEqual(3, comp.Length);
            Assert.IsTrue(comp.Contains("RefDate"));
            Assert.IsTrue(comp.Contains("MeasName"));
            Assert.IsTrue(comp.Contains("Value1"));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Keep_AndApply_ExpectVtlException()
        {
            var DS1 = new Operand
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
                                new StringType("A"),
                                new IntegerType(2),
                                new IntegerType(3),
                            })
                    }))
            };
            var DS2 = new Operand
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
                                            new StringType("A"),
                                            new IntegerType(2),
                                            new IntegerType(3),
                                        })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var commando = "DS_r <- left_join(DS_1, DS_2 ";
            commando = commando + " apply DS_1 * DS_2 ";
            commando = commando + " keep DS_2#Me_1";
            commando = commando + " );";

            Assert.ThrowsException<VtlException>(() => sut.Execute(commando, new[] { DS1, DS2 }));
        }
    }
}
