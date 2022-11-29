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

namespace VTL.Vtl20Engine.Test.AggregateOperatorTests
{
    [TestClass]
    public class GroupByTest
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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Value2",
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
                                new IntegerType(1),
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
                                new IntegerType(1),
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
                                new IntegerType(1),
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
                                new IntegerType(1),
                                new IntegerType(1000),
                                new IntegerType(400)
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
                                new IntegerType(110)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2013"),
                                new StringType("Gross Prod."),
                                new IntegerType(2),
                                new IntegerType(800),
                                new IntegerType(210)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Population"),
                                new IntegerType(2),
                                new IntegerType(250),
                                new IntegerType(310)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Gross Prod."),
                                new IntegerType(2),
                                new IntegerType(1000),
                                new IntegerType(null)
                            }
                        )
                    }))
            };
        }

        

        [TestCategory("Unit")]
        [TestMethod]
        public void Sum_GroupByNewComponent()
        {
            string expression = @"t := DS_1 [calc identifier NR_transaktion := ""D39""]; " +
                "p := t;" +
                "u := p;" +
                "DS_r <- sum(u group by MeasName, Id, NR_transaktion);";
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute(expression, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);

            var index = Array.IndexOf(result.DataSetComponents,
                result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Id")));


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)1, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"D39", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)1800, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)600, dataPointEnumerator.Current[4]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)2, dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Gross Prod.", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"D39", dataPointEnumerator.Current[2]);
                Assert.AreEqual((IntegerType)1800, dataPointEnumerator.Current[3]);
                Assert.AreEqual((IntegerType)210, dataPointEnumerator.Current[4]);

                Assert.IsTrue(dataPointEnumerator.MoveNext());
                Assert.IsFalse(dataPointEnumerator.MoveNext());
            }

        }
        
    }
}