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
    public class NotEqualToTests
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
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(800)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(250)
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

            _ds_2 = new Operand
            {
                Alias = "DS_2",
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
                                new IntegerType(200)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(600)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(300)
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

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void NotEqualTo_Constant()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 <> 200;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
                
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
                
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void NotEqualTo_DataSet()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 <> DS_2;", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
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
        public void EqualTo_ConstantString()
        {
            var ds_s = new Operand
            {
                Alias = "DS_S",
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Value1",
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
                                new StringType("häst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("test")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("bäst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("fest")
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_S <> ""test"";", new[] { ds_s })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void EqualTo_DataSetString()
        {
            var ds_s1 = new Operand
            {
                Alias = "DS_S1",
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Value1",
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
                                new StringType("häst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("test")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("bäst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("fest")
                            }
                        )
                    }))
            };

            var ds_s2 = new Operand
            {
                Alias = "DS_S2",
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
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Value1",
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
                                new StringType("häst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("test")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("bäst")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("pest")
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_S1 <> DS_S2;", new[] { ds_s1, ds_s2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
            }
        }
        [TestCategory("Unit")]
        [TestMethod]
        public void EqualTo_boolean()
        {
            var a = new Operand
            { Alias = "a", Data = new BooleanType(true) };
            var b = new Operand
            { Alias = "b", Data = new BooleanType(false) };
            var c = new Operand
            { Alias = "c", Data = new BooleanType(false) };
            var sut = new VtlEngine(new DataContainerFactory());

            var d = sut.Execute("d <- a <> b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("d"));
            var dresult = d.GetValue() as BooleanType;
            var e = sut.Execute("e <- c <> b;", new[] { c, b }).FirstOrDefault(r => r.Alias.Equals("e"));
            var eresult = e.GetValue() as BooleanType;
            Assert.IsNotNull(dresult);
            Assert.AreEqual((BooleanType)true, dresult);
            Assert.IsNotNull(eresult);
            Assert.AreEqual((BooleanType)false, eresult);
        }
    }
}
