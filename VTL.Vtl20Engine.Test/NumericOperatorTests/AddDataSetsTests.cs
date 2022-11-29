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

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class AddDataSetsTests
    {
        /// <summary>
        ///     VTL 2.0 user manual pg. 81
        /// </summary>
        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addDataSetsWithHomonimousIdentifiers()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Date",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Meas. Name",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Meas. Value",
                        Role = ComponentType.ComponentRole.Measure
                    }
                }, new SimpleDataPointContainer(new HashSet<DataPointType>
                {
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Population"),
                            new IntegerType(200)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(800)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(250)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1000)
                        }
                    )
                }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(900)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData + EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Date", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1700, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 600, dataPointEnumerator.Current[2]);
            }
        }

        /// <summary>
        ///     VTL 2.0 user manual pg. 81
        /// </summary>
        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addDataSetsWithNonHomonimousIdentifiers()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new IntegerType(800)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new IntegerType(250)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new IntegerType(1000)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(900)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData - EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Date", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) (-700), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) (-100), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) (-200), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 450, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addDataSetsWithNonHomonimousIdentifiers2()
        {
            var D1 = new Operand
            {
                Alias = "d1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "A",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "C",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "M",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("A"),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("B"),
                                new IntegerType(800)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("A"),
                                new IntegerType(250)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("B"),
                                new IntegerType(1000)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "d2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "A",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "B",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "C",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "M",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("ELO"),
                                new StringType("A"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("PELO"),
                                new StringType("A"),
                                new IntegerType(900)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("BELO"),
                                new StringType("A"),
                                new IntegerType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("CELO"),
                                new StringType("A"),
                                new IntegerType(1000)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- d1 - d2;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("A", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("B", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("C", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("M", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "ELO", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType) "A", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) (-100), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "PELO", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType) "A", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) (-700), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "BELO", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType) "A", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) (-100), dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "CELO", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType) "A", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) (-750), dataPointEnumerator.Current[3]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addTwoDataSetsWithNotMatchingDataPoints()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2012"),
                                new StringType("Population"),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(900)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Urban"),
                                new IntegerType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData + EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1700, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addDataSetsWithMultipleMeasures()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000),
                                new IntegerType(200)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(300),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(900),
                                new IntegerType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(350),
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
                                new IntegerType(300)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData + EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Date", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 1700, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 2000, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType) 600, dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType) 500, dataPointEnumerator.Current[3]);
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_useSameVariableServalTimes()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Ref. Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000),
                                new IntegerType(200)
                            })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("a := UnitedStatesData;" +
                                 "Dr <- UnitedStatesData + 2;", new[] {D1})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_throwsErrorWhenStringMeasures()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new StringType("200")
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("400")
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("230")
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("1000")
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new StringType("300")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("300")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("300")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("300")
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData + EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var ex = Assert.ThrowsException<VtlException>(() => Dr.GetValue());
            Assert.AreEqual(
                "Värdekomponenten Meas. Value har datatypen StringType vilken inte är tillåten för operatorn Addition.",
                ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_addDataSetsWithMixedNumericDataTypes()
        {
            var D1 = new Operand
            {
                Alias = "UnitedStatesData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            })
                    }))
            };

            var D2 = new Operand
            {
                Alias = "EuropeanUnionData",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Meas. Name",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new NumberType(300)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new NumberType(900)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new NumberType(350)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new NumberType(1000)
                            }
                        )
                    }))
            };
            var sut = new VtlEngine(new DataContainerFactory());

            var Dr = sut.Execute("Dr <- UnitedStatesData + EuropeanUnionData;", new[] {D1, D2})
                .FirstOrDefault(r => r.Alias.Equals("Dr"));

            var result = Dr.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Date", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Meas. Value", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 1700, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 500, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 2000, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType) "Population", dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 600, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void AddDataSets_withEmptyDataset()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var _ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Date",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Meas. Value",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Population"),
                                new IntegerType(200)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250)
                            }),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(1000)
                            })
                    }))
            };

            var DS_r = sut.Execute("DS_2 := DS_1[filter Id = 0];DS_r <- DS_2 + DS_1;", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.DataPoints.Length);
        }
    }
}