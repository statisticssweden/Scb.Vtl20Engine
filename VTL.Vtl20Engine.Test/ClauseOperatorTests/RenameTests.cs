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
    public class RenameTests
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
        public void Rename_renameComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_2 := DS_1 + 100; DS_r <- DS_2[rename Value1 to zApa, Value2 to zBulle];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("MeasName", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("RefDate", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("zApa", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("zBulle", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_ThrowsExceptionWhenNameNotFound()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[rename Value1 to Apa, Value222 to Bulle];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_ThrowsExceptionWhenNameAllreadyExcists()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[rename Value1 to Apa, Value2 to MeasName];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_MultilineExpression()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"temp := DS_1 + 100; DS_r <- temp[rename Value1 to bulle];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("bulle", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_ToExcistingNameThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"DS_r <- DS_1[rename Value1 to Value2];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_TooLongComponentNameThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"DS_r <- DS_1[rename Value1 to abcdefghijabcdefghijabcdefghijabcdefghijabcdefghijk];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_SeveralComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"DS_r <- DS_1[rename Value1 to bulle, Value2 to saft];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("bulle", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("saft", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Rename_ComponentCountTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[rename Value1 to bulle, Value2 to saft]", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ids = DS_r.GetIdentifierNames();
            Assert.AreEqual(2, ids.Length);
            Assert.IsTrue(ids.Contains("RefDate"));
            Assert.IsTrue(ids.Contains("MeasName"));

            var meas = DS_r.GetMeasureNames();
            Assert.AreEqual(2, meas.Length);
            Assert.IsTrue(meas.Contains("bulle"));
            Assert.IsTrue(meas.Contains("saft"));

            var comp = DS_r.GetComponentNames();
            Assert.AreEqual(4, comp.Length);
            Assert.IsTrue(comp.Contains("RefDate"));
            Assert.IsTrue(comp.Contains("MeasName"));
            Assert.IsTrue(comp.Contains("bulle"));
            Assert.IsTrue(comp.Contains("saft"));
        }

    }
}