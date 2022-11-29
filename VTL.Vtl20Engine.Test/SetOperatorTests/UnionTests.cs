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

namespace VTL.Vtl20Engine.Test.SetOperatorTests
{
    [TestClass]
    public class UnionTests
    {
        private Operand _ds_1;
        private Operand _ds_2;
        private Operand _ds_3;
        private Operand _ds_4;

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
                                new IntegerType(805)
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
                                new IntegerType(884)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(4948)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new StringType("Population"),
                                new IntegerType(412)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new StringType("Gross Prod."),
                                new IntegerType(754)
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
                            Name = "RefDate",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "MeasName",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
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
                                new StringType("Gross Prod."),
                                new NumberType(666.6m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("Population"),
                                new NumberType(777.7m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new StringType("Gross Prod."),
                                new NumberType(555.5m)
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
                            Name = "Autor",
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
                                new StringType("Gross Prod."),
                                new StringType("Karin"),
                                new IntegerType(4234)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2017"),
                                new StringType("Population"),
                                new StringType("Karin"),
                                new IntegerType(546)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2017"),
                                new StringType("Gross Prod."),
                                new StringType("Karin"),
                                new IntegerType(8877)
                            }
                        )
                    }))
            };
        }



        [TestCategory("Unit")]

        [TestMethod]
        public void Union_OneDataSets()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_1);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)805, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Union_TwoDataSets()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_1, DS_2);", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)805, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)754, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)412, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Union_ThreeDataSets()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_1, DS_2, DS_3);", new[] { _ds_1, _ds_2, _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)805, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)754, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)250, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType)777.7m, dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[1]);
                Assert.AreEqual((IntegerType)412, dataPointEnumerator.Current[2]);
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Union_UnlikeComponents3DataSets()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            try
            {
                var DS_r = sut.Execute("DS_r <- union(DS_1, DS_2, DS_4);", new[] { _ds_1, _ds_2, _ds_4 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Alla dataset som ingår i Union måste ha samma datsetkomponenter.", ex.Message);
            }

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Union_NonDataSetComponentsDataSets()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            try
            {
                var DS_r = sut.Execute("DS_r <- union(DS_1, 250, 30);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Alla argument till Union måste vara datset.", ex.Message);
            }
        }

        [TestMethod]
        public void Union_ResultIsIntegerType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_1, DS_2);", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(IntegerType));
        }

        [TestMethod]
        public void Union_WithIntegerAndNumber()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_3, DS_1);", new[] { _ds_1, _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(NumberType));
            Assert.AreEqual(5, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)666.6m, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)555.5m, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)200, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)250, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)777.7m, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));
            }
        }

        [TestMethod]
        public void Union_WithNumberAndInteger()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- union(DS_1, DS_3);", new[] { _ds_1, _ds_3 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(NumberType));
            Assert.AreEqual(5, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)805, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)555.5m, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)200, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)250, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType)777.7m, dataPointEnumerator.Current[2]);
                Assert.IsInstanceOfType(dataPointEnumerator.Current[2], typeof(NumberType));
            }
        }
    }
}
