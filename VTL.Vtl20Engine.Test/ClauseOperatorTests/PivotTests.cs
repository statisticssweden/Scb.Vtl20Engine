using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;
using System.Linq;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.Test.ClauseOperatorTests
{
    [TestClass]
    public class PivotTests
    {
        private Operand _ds_1;
        private Operand _ds_2;
        private Operand _ds_3;
        private Operand _ds_4;
        private Operand _ds_5;
        private Operand _ds_6;
        private Operand _ds_7;

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
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("fin"),
                                new StringType("hund"),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("fin"),
                                new StringType("katt"),
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("ful"),
                                new StringType("hund"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("ful"),
                                new StringType("katt"),
                                new IntegerType(400)
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
                            Name = "stad",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                       new MockComponent(typeof(StringType))
                        {
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
        new SimpleDataPointContainer(new HashSet<DataPointType>()
        {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("fin"),
                                new StringType("hund"),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("fin"),
                                new StringType("katt"),
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("ful"),
                                new StringType("hund"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("ful"),
                                new StringType("katt"),
                                new IntegerType(400)
                            }
                        ),
                         new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("fin"),
                                new StringType("hund"),
                                new IntegerType(150)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("fin"),
                                new StringType("katt"),
                                new IntegerType(250)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("ful"),
                                new StringType("hund"),
                                new IntegerType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("ful"),
                                new StringType("katt"),
                                new IntegerType(450)
                            }
                        )
                   }))
            };

            _ds_3 = new Operand
            {
                Alias = "DS_3",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                       new MockComponent(typeof(StringType))
                        {
                            Name = "stad",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                       new MockComponent(typeof(StringType))
                        {
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
        new SimpleDataPointContainer(new HashSet<DataPointType>()
        {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("fin"),
                                new StringType("hund"),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("ful"),
                                new StringType("hund"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("ful"),
                                new StringType("katt"),
                                new IntegerType(400)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("fin"),
                                new StringType("katt"),
                                new IntegerType(250)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("ful"),
                                new StringType("hund"),
                                new IntegerType(350)
                            }
                        )
                   }))
            };

            _ds_4 = new Operand
            {
                Alias = "DS_4",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                       new MockComponent(typeof(StringType))
                        {
                            Name = "stad",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                       new MockComponent(typeof(StringType))
                        {
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Value3",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("fin"),
                                new IntegerType(33),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("ful"),
                                new IntegerType(34),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new StringType("mittemellan"),
                                new IntegerType(35),
                                new IntegerType(400)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("fin"),
                                new IntegerType(36),
                                new IntegerType(250)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("mittemellan"),
                                new IntegerType(40),
                                new IntegerType(450)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new StringType("ful"),
                                new IntegerType(37),
                                new IntegerType(350)
                            }
                        )
                     }))
            };

            _ds_5 = new Operand
            {
                Alias = "DS_5",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                       new MockComponent(typeof(StringType))
                        {
                            Name = "stad",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                       new MockComponent(typeof(IntegerType))
                        {
                            Name = "storlek",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Identifier
                        },

                        new MockComponent(typeof(StringType))
                        {
                            Name = "namn",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },

                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(1000),
                                new StringType("katt"),
                                new StringType("kajsa"),
                                new StringType("½#¤%&?}="),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(1000),
                                new StringType("katt"),
                                new StringType("lotta"),
                                new StringType("ful"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(1000),
                                new StringType("hund"),
                                new StringType("nisse"),
                                new StringType("ful"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(2000),
                                new StringType("hund"),
                                new StringType("fia"),
                                new StringType("½#¤%&?}="),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(2000),
                                new StringType("hund"),
                                new StringType("maja"),
                                new StringType("ful"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Stockholm"),
                                new IntegerType(2000),
                                new StringType("katt"),
                                new StringType("putte"),
                                new StringType("ful"),
                            }
                        ),
                       new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new IntegerType(1000),
                                new StringType("hund"),
                                new StringType("olle"),
                                new StringType("½#¤%&?}="),
                            }
                        ),
                         new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("Örebro"),
                                new IntegerType(2000),
                                new StringType("katt"),
                                new StringType("pelle"),
                                new StringType("ful"),
                            }
                        )
                     }))
            };

            _ds_6 = new Operand
            {
                Alias = "DS_6",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                       new MockComponent(typeof(StringType))
                        {
                            Name = "typ",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "strval",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
                   new SimpleDataPointContainer(new HashSet<DataPointType>()
                   {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("fin"),
                                new StringType("typ"),
                                new StringType("typ")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("fin"),
                                new StringType("katt"),
                                new StringType("katt")
                            }
                        )
                   }))
            };

            _ds_7 = new Operand
            {
                Alias = "DS_7",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "djur",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value1",
                            Role = ComponentType.ComponentRole.Measure
                        },

                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("hund"),
                                new IntegerType(100)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("katt"),
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("häst"),
                                new IntegerType(300)
                            }
                        )
                    }))
            };
        }

        [TestMethod]
        public void Pivot_NoAdditionalIdentifiers()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_7[pivot djur, Value1];", new[] { _ds_7 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);

            Assert.AreEqual("hund", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[0].Role);

            Assert.AreEqual("häst", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[1].Role);

            Assert.AreEqual("katt", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Pivot_OneAdditionalIdentifier()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[pivot djur, Value1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(3, result.DataSetComponents.Length);
            Assert.AreEqual("typ", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.AreEqual("hund", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[1].Role);

            Assert.AreEqual("katt", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"fin", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"ful", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)400, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Pivot_TwoAdditionalIdentifiers()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2[pivot djur, Value1];", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);
            Assert.AreEqual("stad", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.AreEqual("typ", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);

            Assert.AreEqual("hund", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.AreEqual("katt", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"fin", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"ful", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)400, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"fin", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)150, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"ful", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)350, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)450, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void Pivot_TwoAdditionalIdentifiersWithMissingRows()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_3[pivot djur, Value1];", new[] { _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataSetComponents.Length);
            Assert.AreEqual("stad", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.AreEqual("typ", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);

            Assert.AreEqual("hund", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.AreEqual("katt", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"fin", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"ful", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)400, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"fin", dataPointEnumerator.Current[1]);
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"ful", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)350, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
            }
        }
        [TestMethod]
        public void Pivot_OneAdditionalIdentifiersWithExtraValueComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_4[pivot typ, Value1];", new[] { _ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;


            Assert.AreEqual("stad", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.AreEqual("fin", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[1].Role);

            Assert.AreEqual("ful", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.AreEqual("mittemellan", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)400, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)350, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)450, dataPointEnumerator.Current[3]);
            }
        }

        [TestMethod]
        public void Pivot_ThreeAdditionalIdentifiers_StringMeassure_MissingValues()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_5[pivot typ, namn];", new[] { _ds_5 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(5, result.DataSetComponents.Length);
            Assert.AreEqual("djur", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.AreEqual("stad", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);

            Assert.AreEqual("storlek", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[2].Role);

            Assert.AreEqual("½#¤%&?}=", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[3].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            Assert.AreEqual("ful", result.DataSetComponents[4].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[4].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[4].Role);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hund", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)1000, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.AreEqual((StringType)"nisse", dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hund", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"fia", dataPointEnumerator.Current[3]);
                Assert.AreEqual((StringType)"maja", dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hund", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)1000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"olle", dataPointEnumerator.Current[3]);
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"katt", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)1000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"kajsa", dataPointEnumerator.Current[3]);
                Assert.AreEqual((StringType)"lotta", dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"katt", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Stockholm", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.AreEqual((StringType)"putte", dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"katt", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[2]);
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.AreEqual((StringType)"pelle", dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void Pivot_NonExistingMeasureThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_3[pivot djur, Value5];", new[] { _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));
            Assert.AreEqual("Value5 kunde inte hittas.", ex.Message);
        }

        [TestMethod]
        public void Pivot_NonExistingIdentifierThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_3[pivot FinnsInte, Value1];", new[] { _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r")));
            Assert.AreEqual("FinnsInte kunde inte hittas.", ex.Message);
        }

        [TestMethod]
        public void Pivot_MeasureIsIdentifierThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("DS_r <- DS_3[pivot djur, stad];", new[] { _ds_3 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Komponenten stad är en identifierare, funktionen förväntar sig en värdekomponent.", ex.Message);
        }


        [TestMethod]
        public void Pivot_PivotedThreeArgumentsThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_2[pivot stad, typ, Value1];",
            new[] { _ds_2 }).FirstOrDefault(r => r.Alias.Equals("DS_r")));
            Assert.AreEqual("Anropet har för många argument.", ex.Message);
        }

        [TestMethod]
        public void Pivot_PivotedOneArgumentThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_2[pivot stad];",
            new[] { _ds_2 }).FirstOrDefault(r => r.Alias.Equals("DS_r")));
            Assert.AreEqual("Pivot kräver två parametrar.", ex.Message);
        }
    }
}