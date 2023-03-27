using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.TimeOperatorTests
{
    [TestClass]
    public class CurrentDateTest
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

        [TestMethod]
        public void CurrentDate_BasicTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- current_date", new Operand[0])
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = dsr.GetValue() as DateType;

            Assert.IsTrue(result.HasValue());
        }

        [TestMethod]
        public void CurrentDate_CalcTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc identifier dateComponent := current_date];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            var resultComponent = result.GetDataSetComponent("dateComponent");

            Assert.AreEqual("dateComponent", resultComponent.Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, resultComponent.Role);
            Assert.AreEqual(typeof(DateType), resultComponent.DataType);

        }


        [TestMethod]
        public void CurrentDate_CastTest()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc identifier stringComponent := cast(current_date, string, \"YYYYMMDD\"];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            var resultComponent = result.GetDataSetComponent("stringComponent");
            var index = result.IndexOfComponent(resultComponent);
            Assert.AreEqual("stringComponent", resultComponent.Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, resultComponent.Role);
            Assert.AreEqual(typeof(StringType), resultComponent.DataType);


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType(DateTime.Now.ToString("yyyyMMdd")), dataPointEnumerator.Current[index]);
            }
        }
    }
}
