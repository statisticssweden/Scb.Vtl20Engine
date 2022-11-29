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
    public class GreaterThenTests
    {
        private Operand _ds_1, _ds_2, _ds_string1, _ds_string2;

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

            _ds_string1 = new Operand
            {
                Alias = "DS_STRING1",
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
                                new StringType("ABB"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("ABC"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("ACB"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("ACC"),
                            }
                        )
                    }))
            };

            _ds_string2 = new Operand
            {
                Alias = "DS_STRING2",
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
                                new StringType("ACC"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new StringType("ACB"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new StringType("ABC"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new StringType("ABB"),
                            }
                        )
                    }))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThen_Constant()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 > 200;", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThan_DataSet()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 > DS_2;", new[] { _ds_1, _ds_2 })
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
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThen_ConstantString()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_STRING1 > \"ABC\";", new[] { _ds_string1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);//ABC>ABC

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);//ACC>ABC

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);//ABB>ABC

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);//ACB>ABC
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThan_DataSetStringMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_STRING1 > DS_STRING2;", new[] { _ds_string1, _ds_string2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);//ABC>ACB

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);//ACC>ABB

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);//ABB>ACC

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);//ACB>ABC
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThan_timePeriod()
        {
            var ds_t = new Operand
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
                        new MockComponent(typeof(TimePeriodType))
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
                                new TimePeriodType(2001, Duration.Quarter, 1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new TimePeriodType(2000, Duration.Quarter, 1)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new TimePeriodType(2000, Duration.Quarter, 3)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new TimePeriodType(2000, Duration.Quarter, 1)
                            }
                        )
                    }))
            };


            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("DS_r <- DS_1 > cast(\"2000Q01\", time_period, \"YYYY\\QQQ\");", new[] { ds_t })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(BooleanType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("bool_var", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)false, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((BooleanType)true, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void GreaterThan_boolean()
        {
            var a = new Operand
            { Alias = "a", Data = new BooleanType(true) };
            var b = new Operand
            { Alias = "b", Data = new BooleanType(false) };
            var c = new Operand
            { Alias = "c", Data = new BooleanType(false) };
            var sut = new VtlEngine(new DataContainerFactory());

            var d = sut.Execute("d <- a > b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("d"));
            var dresult = d.GetValue() as BooleanType;
            var e = sut.Execute("e <- c > a;", new[] { c, a }).FirstOrDefault(r => r.Alias.Equals("e"));
            var eresult = e.GetValue() as BooleanType;
            var f = sut.Execute("f <- c > b;", new[] { c, b }).FirstOrDefault(r => r.Alias.Equals("f"));
            var fresult = f.GetValue() as BooleanType;
            Assert.IsNotNull(dresult);
            Assert.AreEqual((BooleanType)true, dresult);
            Assert.IsNotNull(eresult);
            Assert.AreEqual((BooleanType)false, eresult);
            Assert.IsNotNull(fresult);
            Assert.AreEqual((BooleanType)false, fresult);
        }

    }
}
