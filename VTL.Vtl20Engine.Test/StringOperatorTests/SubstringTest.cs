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

namespace VTL.Vtl20Engine.Test.StringOperatorTests
{
    [TestClass]
    public class SubstringTest
    {
        private Operand _ds_1;
        private Operand _ds_2;

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
                            Name = "Namn",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "stad",
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
                                new StringType("Karin Fägerlind"),
                                new StringType("Örebro")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Kalle Anka"),
                                new StringType("Ankeborg")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2015"),
                                new StringType("Leonard Larsson"),
                                new StringType("Altersbruk")
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
                            Name = "Namn",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Nummer",
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
                                new StringType("Karin Fägerlind"),
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2014"),
                                new StringType("Kalle Anka"),
                                new IntegerType(7)
                            }
                        )
                    }))
            };
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_TwoParameters()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_1, 6);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)" Fägerlind", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"o", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)" Anka", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"org", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"rd Larsson", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"sbruk", dataPointEnumerator.Current[2]);
            }
        }



        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_OnlyLengthParam()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_1, _, 6);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Karin ", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Kalle ", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Ankebo", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Leonar", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Alters", dataPointEnumerator.Current[2]);
            }
        }



        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_ComponentStart()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_2#Namn, DS_2#Nummer);", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            Assert.AreEqual(2, result.DataSetComponents.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"arin Fägerlind", dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Anka", dataPointEnumerator.Current[1]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_ComponentStringStartThrowsException()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_2#Namn, DS_2#Namn);", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Startpositionen måste vara ett heltal större än 0.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_ComponentLength()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_2#Namn, 1, DS_2#Nummer);", new[] { _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);
            Assert.AreEqual(2, result.DataSetComponents.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Ka", dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Kalle A", dataPointEnumerator.Current[1]);
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_StartZero()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_1, 0);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue() as DataSetType);
            Assert.AreEqual("Startpositionen måste vara ett heltal större än 0.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_ThreeParameters()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_1, 6, 4);", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)" Fäg", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"o", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)" Ank", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"org", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"rd L", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"sbru", dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_SpecificComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [ calc str:= substr(stad,1,5) ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Karin Fägerlind", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Örebr", dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Kalle Anka", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Ankeborg", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Ankeb", dataPointEnumerator.Current[3]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((StringType)"Leonard Larsson", dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Altersbruk", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Alter", dataPointEnumerator.Current[3]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_OnlyOneArgument()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute("DS_r <- substr(DS_1);", new[] { _ds_1 })
                    .FirstOrDefault(r => r.Alias.Equals("DS_r"));

                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Du måste ha minst två parametrar till funktionen substring.", ex.Message);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_TooManyArgument()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute("DS_r <- substr(DS_1,1,2,3,4,5,7);", new[] { _ds_1 })
                    .FirstOrDefault(r => r.Alias.Equals("DS_r"));

                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Du får inte ha mer än tre parametrar till funktionen substring.", ex.Message);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_NonNumericAs2ndArgument()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute("DS_r <- substr(DS_1,karin,5);", new[] { _ds_1 })
                    .FirstOrDefault(r => r.Alias.Equals("DS_r"));

                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Beräkningen kan inte exekveras eftersom dataset eller komponent med namn karin inte kan hittas.", ex.Message);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_NonNumericAs3rdArgument()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute("DS_r <- substr(DS_1,2,karin);", new[] { _ds_1 })
                    .FirstOrDefault(r => r.Alias.Equals("DS_r"));

                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Beräkningen kan inte exekveras eftersom dataset eller komponent med namn karin inte kan hittas.", ex.Message);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_NumericAs1stdArgument()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            try
            {
                var DS_r = sut.Execute("DS_r <- substr(DS_2, 1, 2);", new[] { _ds_2 })
                    .FirstOrDefault(r => r.Alias.Equals("DS_r"));
                var result = DS_r.GetValue() as DataSetType;
                Assert.Fail();
                Assert.IsNull(result);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Substr kan endast användas på värden av typen sträng.", ex.Message);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_StartPosLargerThanStringLength()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- substr(DS_1,7);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value = dataPointEnumerator.Current[2];
                var isEmptyString = value.ToString().Length == 0;
                Assert.IsTrue(isEmptyString);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_WithCalcWhereStartExceedsStringLength()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 [calc measure StringMeasure := RefDate]; DS_r <- substr(temp#StringMeasure,7);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.IsTrue(string.IsNullOrEmpty(dataPointEnumerator.Current[1] as StringType));
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_WithCalcWhereStartExceedsStringLength2()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("temp := DS_1 [calc measure StringMeasure := Namn]; DS_r <- substr(temp#StringMeasure,11,22);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual("rlind",(dataPointEnumerator.Current[1] as StringType).ToString());

                dataPointEnumerator.MoveNext();
                Assert.IsTrue(string.IsNullOrEmpty(dataPointEnumerator.Current[1] as StringType));
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Substring_DataContining_()
        {
            var UFS = new Operand
            {
                Alias = "UFS",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Referensperiod",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "UFSunderart",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "UFSsektor",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "UFSanslag",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Varde",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016-Q1"),
                                new StringType("1049"),
                                new StringType("S13111"),
                                new StringType("2018_1401001"),
                                new NumberType(1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016-Q1"),
                                new StringType("1049"),
                                new StringType("S13112"),
                                new StringType("2018_1401001"),
                                new NumberType(1m)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016-Q1"),
                                new StringType("1049"),
                                new StringType("S13113"),
                                new StringType("2018_1401001"),
                                new NumberType(1m)
                            }
                        )
                    }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var vtl = "A1:= UFS[filter Referensperiod = \"2016-Q1\" and UFSunderart = \"1049\" and UFSsektor in { \"S13111\", \"S13112\", \"S13113\"}];";
            vtl += "A2 <-substr(A1#UFSanslag, 6, 7) ;";

            var DS_r = sut.Execute(vtl, new[] { UFS }).FirstOrDefault(r => r.Alias.Equals("A2"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual(3, result.DataPointCount);
        }
    }
}
