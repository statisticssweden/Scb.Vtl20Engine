using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.AggregateOperatorTests
{
    [TestClass]
    public class DataTypeErrorsTest
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
                        Name = "Id",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Value1",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
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
                            new IntegerType(1),
                            new IntegerType(200),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(1),
                            new IntegerType(805),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(1),
                            new IntegerType(250),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1),
                            new IntegerType(1000),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Population"),
                            new IntegerType(2),
                            new IntegerType(200),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(2),
                            new IntegerType(null),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(2),
                            new IntegerType(313),
                            new StringType("20190819")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(2),
                            new IntegerType(414),
                            new StringType("20190819")
                        }
                    )

                }))
            };
        }

        [TestMethod]
        public void AverageGroupByOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- avg(DS_1 group by RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            try
            {
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.AreEqual("Värdekomponenten Value2 är inte numerisk. Operationen kan därför inte utföras.", ex.Message);
            }
        }

        [TestMethod]
        public void SumGroupByOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            try
            {
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Värdekomponenten Value2 är inte numerisk. Operationen kan därför inte utföras.", ex.Message);
            }
        }

        [TestMethod]
        public void MaxByOneComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- sum(DS_1 group by RefDate);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            try
            {
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Värdekomponenten Value2 är inte numerisk. Operationen kan därför inte utföras.", ex.Message);
            }
        }
    }
}
