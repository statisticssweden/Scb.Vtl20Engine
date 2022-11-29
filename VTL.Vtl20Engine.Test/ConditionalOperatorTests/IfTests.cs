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

namespace VTL.Vtl20Engine.Test.ConditionalOperatorTests
{
    [TestClass]
    public class IfTests
    {
        [TestCategory("Unit")]
        [TestMethod]
        public void If_ScalarIfTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("r <- if x > 0 then 2 else 5;", new Operand[] { new Operand { Alias = "x", Data = new IntegerType(3) } })
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = DS_r.GetValue() as IntegerType;
            Assert.AreEqual(2, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_ScalarElseTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("r <- if x > 0 then 2 else 5;", new Operand[] { new Operand { Alias = "x", Data = new IntegerType(-2) } })
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = DS_r.GetValue() as IntegerType;
            Assert.AreEqual(5, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_DataSetTest()
        {
            var _ds_cond = new Operand
            {
                Alias = "DS_cond",
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("B"), new StringType("Total"), new StringType("M"),
                            new IntegerType(5451780)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("B"), new StringType("Total"), new StringType("F"),
                            new IntegerType(5643070)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("G"), new StringType("Total"), new StringType("M"),
                            new IntegerType(5449803)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("G"), new StringType("Total"), new StringType("F"),
                            new IntegerType(5673231)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("M"),
                            new IntegerType(23099012)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new IntegerType(23719207)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("M"),
                            new IntegerType(31616281)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new IntegerType(33671580)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("M"),
                            new IntegerType(28726599)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new IntegerType(30667608)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new IntegerType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("F"),
                            new IntegerType(null)
                        })
                    }))
            };

            var _ds_1 = new Operand
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
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new NumberType(25.8m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new NumberType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new NumberType(20.9m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new NumberType(6.3m)
                        })
                    }))
            };

            var _ds_2 = new Operand
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("B"), new StringType("Total"), new StringType("M"),
                            new NumberType(0.12m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("G"), new StringType("Total"), new StringType("M"),
                            new NumberType(22.5m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("M"),
                            new NumberType(23.7m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("F"),
                            new NumberType(null)
                        })
                    }))
            };


            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- if(DS_cond#Id_4 = \"F\") then DS_1 else DS_2;", new[] { _ds_cond, _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(6, result.DataPoints.Length);
            Assert.IsTrue(result.DataSetComponents.Any(c => c.Name.Equals("Me_1")));
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)0.12m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)22.5m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)20.9m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)25.8m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)23.7m, dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_DataSetScalarTest()
        {
            var _ds_cond = new Operand
            {
                Alias = "DS_cond",
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("B"), new StringType("Total"), new StringType("M"),
                            new IntegerType(5451780)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("B"), new StringType("Total"), new StringType("F"),
                            new IntegerType(5643070)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("G"), new StringType("Total"), new StringType("M"),
                            new IntegerType(5449803)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("G"), new StringType("Total"), new StringType("F"),
                            new IntegerType(5673231)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("M"),
                            new IntegerType(23099012)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new IntegerType(23719207)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("M"),
                            new IntegerType(31616281)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new IntegerType(33671580)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("M"),
                            new IntegerType(28726599)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new IntegerType(30667608)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new IntegerType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("F"),
                            new IntegerType(null)
                        })
                    }))
            };

            var _ds_1 = new Operand
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
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new NumberType(25.8m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new NumberType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new NumberType(20.9m)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new NumberType(6.3m)
                        })
                    }))
            };


            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- if(DS_cond#Id_4 = \"F\") then DS_1 else 2;", new[] { _ds_cond, _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(9, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)20.9m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)25.8m, dataPointEnumerator.Current[4]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)2m, dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_ThenDataSetElseScalarTest()
        {
            var _ds_1 = new Operand
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new IntegerType(25),
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new IntegerType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new IntegerType(20)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new IntegerType(6)
                        })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- if(DS_1#Id_4 = \"F\") then DS_1 else 1;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.IsTrue(result.DataSetComponents.Any(c => c.Name.Equals("Me_1")));
            //Assert.IsTrue(result.DataPoints.Where(dp => dp[3].Equals((StringType)"M")).All(dp => dp[4].Equals((IntegerType)1)));
            //Assert.IsTrue(result.DataPoints.Where(dp => dp[3].Equals((StringType)"F")).All(dp => !dp[4].Equals((IntegerType)1)));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_ThenScalarElseDataSetTest()
        {
            var _ds_1 = new Operand
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new IntegerType(25),
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new IntegerType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new IntegerType(20)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new IntegerType(6)
                        })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- if(DS_1#Id_4 = \"M\") then 1 else DS_1;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.IsTrue(result.DataSetComponents.Any(c => c.Name.Equals("Me_1")));
            //Assert.IsTrue(result.DataPoints.Where(dp => dp[3].Equals((StringType)"M")).All(dp => dp[4].Equals((IntegerType)1)));
            //Assert.IsTrue(result.DataPoints.Where(dp => dp[3].Equals((StringType)"F")).All(dp => !dp[4].Equals((IntegerType)1)));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void If_Then_Bugg129709()
        {
            var _ds_1 = new Operand
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
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("S"), new StringType("Total"), new StringType("F"),
                            new IntegerType(25),
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("F"), new StringType("Total"), new StringType("F"),
                            new IntegerType(null)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("I"), new StringType("Total"), new StringType("F"),
                            new IntegerType(20)
                        }),
                        new DataPointType(new ScalarType[]
                        {
                            new StringType("2012"), new StringType("A"), new StringType("Total"), new StringType("M"),
                            new IntegerType(6)
                        })
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- if(DS_1#Id_4 = \"M\") then (sum(DS_1 group except Id_1))[calc identifier Id_1:=\"1900\"] else DS_1;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;


            //Kan inte matcha på raderna i "then" satsen efter att index komponenterna har räknats om
            Assert.AreNotEqual(4, result.DataPoints.Length);

            //Korrekt sätt att skriva om index komponenterna ska räknas om
            //Operationen måste ske i flera steg.
            var vtl = "C := DS_1[filter Id_4 <> \"M\"];";
            vtl += " D := DS_1[filter Id_4 = \"M\"];";
            vtl += "E := (sum(D group except Id_1))[calc identifier Id_1:= \"1900\"];";
            vtl += "DS_r <- union(C, E);";
            DS_r = sut.Execute(vtl, new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual(4, result.DataPoints.Length);

        }

    }
}