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
    public class FilterTests
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
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2),
                                new StringType("E"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2),
                                new StringType("F"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("XX"),
                                new IntegerType(20),
                                new StringType("F"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(1),
                                new StringType("B"),
                                new StringType("YY"),
                                new IntegerType(1),
                                new StringType("F"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(4),
                                new StringType("E"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(9),
                                new StringType("F"),
                            }
                        ),
                    }))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_IdEquals()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Id_1 = 1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPointCount);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                while (dataPointEnumerator.MoveNext())
                {
                    Assert.AreEqual(new IntegerType(1), dataPointEnumerator.Current[0]);
                }
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_Divide()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_2 := DS_1[filter Id_1 = 1];DS_r <- 10/DS_2", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new StringType("XX"), dataPointEnumerator.Current[2]);
                Assert.AreEqual(new NumberType(5m), dataPointEnumerator.Current[3]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_StringIdEquals()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Id_3 = \"XX\"];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                while (dataPointEnumerator.MoveNext())
                {
                    Assert.AreEqual(new StringType("XX"), dataPointEnumerator.Current[2]);
                }
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_MeGraterThan()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Me_1 > 1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                while (dataPointEnumerator.MoveNext())
                {
                    Assert.IsTrue((IntegerType)dataPointEnumerator.Current[3] > new IntegerType(1));
                }
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_MeLessThanOrEqual()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Me_1 <= 2];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                while (dataPointEnumerator.MoveNext())
                {
                    Assert.IsTrue((IntegerType)dataPointEnumerator.Current[3] <= 2);
                }
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_MeGraterThanId()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Me_1 > Id_1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DataPointCount);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_MeGraterThanIdPlus1()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Me_1 > Id_1 + 1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPointCount);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_MeIn()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Me_1 in {1, 4, 9}];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPointCount);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_ErrorWhenDatasetDoesntExcist()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- a[filter Me_1 = 1];", new[] { _ds_1 }));
            Assert.AreEqual("Beräkningen kan inte exekveras eftersom dataset eller komponent med namn a inte kan hittas.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_ErrorWhenComponentDoesntExcist()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- DS_1[filter Bullbacka = 1];", new[] { _ds_1 }));
            Assert.AreEqual("Bullbacka kunde inte hittas.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_NoErrorWhenDatasetDoesExcist()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- a[filter Id_1 = 1]; a := DS_1", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPointCount);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Filter_ResultIsIntegerType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[filter Id_1 = 1];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPointCount);
            Assert.IsTrue(result.DataSetComponents[3].Name.Equals("Me_1"));
            Assert.IsTrue(result.DataSetComponents[3].DataType == typeof(IntegerType));
        }
    }
}
