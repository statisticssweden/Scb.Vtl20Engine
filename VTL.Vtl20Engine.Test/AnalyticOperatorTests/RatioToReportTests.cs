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

namespace VTL.Vtl20Engine.Test.AnalyticOperatorTests
{
    [TestClass]
    public class RatioToReportTests
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
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_3",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2000),
                                new IntegerType(3),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2001),
                                new IntegerType(4),
                                new NumberType(3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2002),
                                new IntegerType(7),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("XX"),
                                new IntegerType(2003),
                                new IntegerType(6),
                                new NumberType(1),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2000),
                                new IntegerType(12),
                                new NumberType(0),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2001),
                                new IntegerType(8),
                                new NumberType(8),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2002),
                                new IntegerType(6),
                                new NumberType(5),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2003),
                                new IntegerType(14),
                                new NumberType(-3),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new StringType("YY"),
                                new IntegerType(2003),
                                new IntegerType(null),
                                new NumberType(null),
                            }
                        ),
                    }
                ))
            };
        }

        [TestMethod]
        public void RatioToReport_test1()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- ratio_to_report (DS_1 over (partition by Id_1, Id_2));", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.15m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.1m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.2m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.35m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.5m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.1m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.2m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.8m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.15m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) 0.5m, dataPointEnumerator.Current[4]);
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[4].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.35m, dataPointEnumerator.Current[3]);
                Assert.AreEqual((NumberType) (-0.3m), dataPointEnumerator.Current[4]);
            }
        }

        [TestMethod]
        public void RatioToReport_test2()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- ratio_to_report (DS_1 over (partition by Id_3, Id_1));", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;
            var id1 = result.OriginalIndexOfComponent("Id_1");
            var id2 = result.OriginalIndexOfComponent("Id_2");
            var id3 = result.OriginalIndexOfComponent("Id_3");
            var me1 = result.OriginalIndexOfComponent("Me_1");
            var me2 = result.OriginalIndexOfComponent("Me_2");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType)0.2m, dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) 1m, dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) (1m / 3m), dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) (3m / 11m), dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) (7m / 13m), dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) 0.5m, dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) (-0.5m), dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) 0.8m, dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) 0m, dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) (2m / 3m), dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) (8m / 11m), dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) (6m / 13m), dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) 0.5m, dataPointEnumerator.Current[me2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3]);
                Assert.IsFalse(dataPointEnumerator.Current[me1].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[me2].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3]);
                Assert.AreEqual((NumberType) 0.7m, dataPointEnumerator.Current[me1]);
                Assert.AreEqual((NumberType) 1.5m, dataPointEnumerator.Current[me2]);
            }
        }

        /*
         * DETTA BÖR SES ÖVER
         * 

        [TestMethod]
        public void RatioToReport_component()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc a := ratio_to_report (DS_1#Me_1 over (partition by Id_1, Id_2))];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.15m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.2m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.35m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.2m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.15m, dataPointEnumerator.Current[3]);
                dataPointEnumerator.MoveNext();
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((NumberType) 0.35m, dataPointEnumerator.Current[3]);
            }
        }
        
        [TestMethod]
        public void RatioToReport_component2()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc a := ratio_to_report (DS_1#Me_1 over (partition by Id_3))];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            var id2Index = Array.IndexOf(result.DataSetComponents,
                result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Id_2")));
            var id3Index = Array.IndexOf(result.DataSetComponents,
                result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Id_3")));
            var aIndex = Array.IndexOf(result.DataSetComponents,
                result.DataSetComponents.FirstOrDefault(c => c.Name.Equals("a")));

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 0.2m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 1m / 3m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 7m / 13m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"XX", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 0.3m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2000, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 0.8m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2001, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 2m / 3m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2002, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 6m / 13m, dataPointEnumerator.Current[aIndex]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3Index]);
                Assert.IsFalse(dataPointEnumerator.Current[aIndex].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"YY", dataPointEnumerator.Current[id2Index]);
                Assert.AreEqual((IntegerType)2003, dataPointEnumerator.Current[id3Index]);
                Assert.AreEqual((NumberType) 0.7m, dataPointEnumerator.Current[aIndex]);
            }
        }

        */

        [TestMethod]
        public void RatioToReport_filterRatioToReportSum()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var vtlCode = "A := DS_1 [filter substr(Id_2, 1, 1)=\"X\"];";
            vtlCode += "B := ratio_to_report (A over (partition by Id_3));";
            vtlCode += "C <- sum (B group by Id_1);";
            var DS_r = sut.Execute(vtlCode, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("C"));
            var result = DS_r.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "A", dataPointEnumerator.Current[0]);
                Assert.AreEqual((NumberType) 4m, dataPointEnumerator.Current[1]);
                Assert.AreEqual((NumberType) 4m, dataPointEnumerator.Current[2]);
            }
        }


        [TestMethod]
        public void RatioToReport_ThereShouldBeNoOriginalOrderComponentInResult()
        {

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- ratio_to_report (DS_1 over (partition by Id_1, Id_2));", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsFalse(result.DataSetComponents.Any(c => c.Name.Equals("Original_order")));
        }
    }
}
