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
    public class CalcTests
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
                    }
                ))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_addTwoComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc zCOMP_r := Value1 + Value2];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("MeasName", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("RefDate", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("Value2", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[4].DataType);
            Assert.AreEqual("zCOMP_r", result.DataSetComponents[4].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[4].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1000, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1400, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)550, dataPointEnumerator.Current[4]);
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_addComponentAndScalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc COMP_r := Value1 + 100];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(4, result.DataPointCount);

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("MeasName", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("RefDate", result.DataSetComponents[1].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("COMP_r", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[3].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)900, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1100, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)350, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_addAttribute()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc attribute COMP_r := 100];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[4].DataType);
            Assert.AreEqual("COMP_r", result.DataSetComponents[4].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Attribure, result.DataSetComponents[4].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_addIdentifier()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_1[calc identifier COMP_r := ""apa""];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsFalse(DS_r.GetMeasureNames().Contains("COMP_r"));
            Assert.IsTrue(DS_r.GetIdentifierNames().Contains("COMP_r"));
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("COMP_r", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[0]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[0]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[0]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[0]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_addUnspecifiedMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_1[calc zCOMP_r := ""apa""];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[4].DataType);
            Assert.AreEqual("zCOMP_r", result.DataSetComponents[4].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[4].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_updateMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_1[calc Value1 := 100];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_updateMeasureWithNewType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_1[calc Value1 := ""apa""];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_updateIdentifierThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute(@"DS_r <- DS_1[calc MeasName := 100];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            }
            catch (Exception e)
            {
                Assert.AreEqual("Identifier MeasName får ej uppdateras.", e.Message);
            }

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_severalComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r <- DS_1[calc Value1 := ""apa"", Value3 := Value2];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Value1", result.DataSetComponents[2].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)200, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)400, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)100, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"Population", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"apa", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)300, dataPointEnumerator.Current[4]);
            }

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[4].DataType);
            Assert.AreEqual("Value3", result.DataSetComponents[4].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[4].Role);

            Assert.IsNotNull(result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_multilineExpression()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(@"temp := DS_1 + 100; DS_r <- temp[calc zbulle := Value1 + 10];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[4].DataType);
            Assert.AreEqual("zbulle", result.DataSetComponents[4].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[4].Role);

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)910, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1110, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)310, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)360, dataPointEnumerator.Current[4]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_ComponentCountTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 + 100; DS_r <- temp[calc bulle := Value1 + 10];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ids = DS_r.GetIdentifierNames();
            Assert.AreEqual(2, ids.Length);
            Assert.IsTrue(ids.Contains("RefDate"));
            Assert.IsTrue(ids.Contains("MeasName"));

            var meas = DS_r.GetMeasureNames();
            Assert.AreEqual(3, meas.Length);
            Assert.IsTrue(meas.Contains("Value1"));
            Assert.IsTrue(meas.Contains("Value2"));
            Assert.IsTrue(meas.Contains("bulle"));

            var comp = DS_r.GetComponentNames();
            Assert.AreEqual(5, comp.Length);
            Assert.IsTrue(comp.Contains("RefDate"));
            Assert.IsTrue(comp.Contains("MeasName"));
            Assert.IsTrue(comp.Contains("Value1"));
            Assert.IsTrue(comp.Contains("Value2"));
            Assert.IsTrue(comp.Contains("bulle"));
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_ResultIsIntegerType()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 + 100; DS_r <- temp[calc bulle := Value1 + 10];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual("bulle", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(IntegerType));

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Calc_ThrowsExceptionWhenNoMeasureIsSpecified()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 + 100; DS_r <- temp[calc bulle := temp + 10];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Operationen kan inte utföras då temp innehåller flera värdekomponenter. Specificera vilken komponent som avses genom temp#komponent.", e.Message);
        }
    }
}