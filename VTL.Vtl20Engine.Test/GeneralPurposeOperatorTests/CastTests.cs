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

namespace VTL.Vtl20Engine.Test.GeneralPurposeOperatorTests
{
    [TestClass]
    public class CastTests
    {
        private Operand _ds_1;

        [TestInitialize]
        public void init()
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
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
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
                                new IntegerType(123)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("B"),
                                new IntegerType(45)
                            }
                        )
                    }))
            };
        }



        [TestMethod]
        public void CastTests_CastIntAsNumber()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(3, number);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(3m), result);
        }
        [TestMethod]
        public void CastTests_CastIntAsBoolean()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(3, boolean);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new BooleanType(true), result);
        }


        [TestMethod]
        public void CastTests_CastIntAsString()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(1, string);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new StringType("1"), result);
        }

        [TestMethod]
        public void CastTests_CastNumericAsInt()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(3455, integer);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new IntegerType(3455), result);
        }

        [TestMethod]
        public void CastTests_CastNumericWithDecimalsAsInt()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(34.55, integer);", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("Indatavärdet till cast hade ett felaktigt format.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastNumberWithoutDecimalsAsInt()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("a := 34.55; b := round(a,0); m2 <- cast(b, integer);", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Alias == "m2").GetValue() as ScalarType;
            Assert.AreEqual(new IntegerType(35), result);
        }

        [TestMethod]
        public void CastTests_CastNumericAsBoolean()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(0.0, boolean);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new BooleanType(false), result);
        }

        [TestMethod]
        public void CastTests_CastNumericAsString()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(45.547, string);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new StringType("45.547"), result);
        }

        [TestMethod]
        public void CastTests_CastNumericAsString_NumberOfDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast((0.1/3), string);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new StringType("0.0333333333333333333333333333"), result);
        }


        [TestMethod]
        public void CastTests_CastNumericAsStringEmptyDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(45.57000000000000, string);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new StringType("45.57"), result);
        }

        [TestMethod]
        public void CastTests_CastNumericAsStringAndBackToNumeric()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast((cast(45.547, string)), number, \"DD.DDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(45.547)), result);
        }

        [TestMethod]
        public void CastTests_CastBooleanAsInt()
        {
            var b1 = new Operand()
            {
                Alias = "b1",
                Data = new BooleanType(true)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(b1, integer);", new[] { b1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new IntegerType(1), result);
        }

        [TestMethod]
        public void CastTests_CastFalseBooleanAsNumber()
        {
            var b2 = new Operand()
            {
                Alias = "b2",
                Data = new BooleanType(false)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(b2, number);", new[] { b2 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(0.0)), result);
        }

        [TestMethod]
        public void CastTests_CastFalseBooleanAsString()
        {
            var b2 = new Operand()
            {
                Alias = "b2",
                Data = new BooleanType(false)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(b2, string);", new[] { b2 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("False"), result);
        }

        [TestMethod]
        public void CastTests_CastStringAsNumeric()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"DD.DDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(1.655)), result);
        }
        [TestMethod]
        public void CastTests_CastStringAsNumeric1Decimal()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"DD.D\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(1.7)), result);
        }

        [TestMethod]
        public void CastTests_CastStringAsNumericManyDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"DD.DDDDDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(1.655000)), result);
        }

        [TestMethod]
        public void CastTests_CastStringAsNumericWithoutDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"DD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(2)), result);
        }

        [TestMethod]
        public void CastTests_CastStringAsNumericWithArbitraryDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"D+.D*\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new NumberType(new decimal(1.655)), result);
        }

        [TestMethod]
        public void CastTests_CastStringAsNumericZeroDecimals()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"DD.\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("Masken är inkorrekt.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastStringToNumericUnabletoParse()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"APA\", number, \"DD.DDDDDD\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.IsTrue(ex.Message == "Indatasträngen hade ett felaktigt format." || ex.Message == "Input string was not in a correct format.");
        }


        [TestMethod]
        public void CastTests_CastStringToNumericWrongMask()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"APA\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("Masken är inkorrekt.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastStringToNumericWrongMaskDAfterQuantifier()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"D+D.DDD\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("Masken är inkorrekt.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastStringToNumericWrongMaskMultipleQuantifiers()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1.655\", number, \"D+*.DDD\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("Masken är inkorrekt.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastStringAsInt()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1456\", integer);", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new IntegerType(1456), result);
        }


        [TestMethod]
        public void CastTests_CastStringAsBool()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"False\", boolean);", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault().GetValue());
            Assert.AreEqual("String kan inte castas till boolean.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastStringAsIntFFalseNotation()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("m2 <- cast(\"1456\", int);", new Operand[0]));
            Assert.AreEqual("Datatyp okänd", ex.Message);
        }

        [TestMethod]
        public void CastTest_CastIntDatasetAsStringDataSet()
        {

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(DS_1, string);", new Operand[] { _ds_1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as DataSetType;
            Assert.IsNotNull(result);

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"123", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"45", dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void CastTests_CastIntAsStringWithCalc()
        {
            var expression = $"ds2 <- DS_1[calc Id_3 := cast(10,string)];";

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute(expression, new Operand[] { _ds_1 });
            var dataSetResult = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as DataSetType;

            var componentResult = dataSetResult.DataSetComponents.FirstOrDefault(x => x.Name.Equals("Id_3"));
            var resEnumerator = componentResult.GetEnumerator();
            resEnumerator.MoveNext();
            Assert.AreEqual(new StringType("10"), resEnumerator.Current);
        }

        [TestMethod]
        public void CastTests_CastStringToQuarterlyTimePeriod1()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000Q1\", time_period, \"YYYY\\QQ\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToQuarterlyTimePeriod2()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000-Q1\", time_period, \"YYYY-\\QQ\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToQuarterlyTimePeriod3()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000-1\", time_period, \"YYYY-Q\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToQuarterlyTimePeriod4()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"Q1-2000\", time_period, \"\\QQ-YYYY\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToQuarterlyTimePeriod5()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000Q01\", time_period, \"YYYY\\QQQ\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Quarter, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDailyTimePeriod1()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000M01D01\", time_period, \"YYYY\\MMM\\DDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Day, 1), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDailyTimePeriod2()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000.01.01\", time_period, \"YYYY\\.MM\\.DD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Day, 1), result);
        }
        [TestMethod]
        public void CastTests_CastStringToDailyTimePeriod22()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000.12.31\", time_period, \"YYYY.MM.DD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Day, 366), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDailyTimePeriod3()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000.02.20\", time_period, \"YYYY\\.MM\\.DD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Day, 51), result);
        }

        [TestMethod]
        public void CastTests_CastStringToMonthSuccess()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000M02\", time_period, \"YYYY\\MMM\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Month, 2), result);
        }

        [TestMethod]
        public void CastTests_CastStringToMonthSuccessII()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000.12\", time_period, \"YYYY.MM\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Month, 12), result);
        }

        [TestMethod]
        public void CastTests_CastStringToMonthSuccessIII()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"200012\", time_period, \"YYYYMM\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(2000, Duration.Month,12), result);
        }

        [TestMethod]
        public void CastTests_CastStringToMonthFailure()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2000M02\", time_period, \"YYYY\\MM\");", new Operand[0]);
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType);
            Assert.AreEqual("Masken stämmer inte för värdet som castas.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString1()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Quarter, 4)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYYP{ppp}\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015Q4"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString2()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Month, 12)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYYP{ppp}\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015M12"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString3()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Day, 365)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYYP{ppp}\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015D365"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString4()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Month, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY\\MMM\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015M03"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString5()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Quarter, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY\\QQQ\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015Q03"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString6()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Quarter, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY\\Q{QQQQQQ}Q\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015Q3"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToString7()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Quarter, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY-\\QQ\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015-Q3"), result);
        }

        [TestMethod]
        public void CastTests_CastDateToTimePeriod()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new DateType(new DateTime(1980, 2, 2))
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, time_period);", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new TimePeriodType(1980, Duration.Day, 33), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToStringMonthFailure()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Month, 12)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY-\\MM\");", new Operand[] { t1 });
            var ex = Assert.ThrowsException<VtlException>(() => Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType);
            Assert.AreEqual("Masken stämmer inte för värdet som castas.", ex.Message);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToStringMonthSuccess()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Month, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY-\\M{M}M\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015-M3"), result);
        }

        [TestMethod]
        public void CastTests_CastTimePeriodToStringMonthSuccessII()
        {
            var t1 = new Operand()
            {
                Alias = "t1",
                Data = new TimePeriodType(2015, Duration.Month, 3)
            };

            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(t1, string, \"YYYY-MM\");", new Operand[] { t1 });
            var result = Ds_r.FirstOrDefault(r => r.Persistant).GetValue() as ScalarType;
            Assert.AreEqual(new StringType("2015-03"), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDate1()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"1980M06D20\", date, \"YYYY\\MMM\\DDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new DateType(new DateTime(1980, 6, 20)), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDate2()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"20151110\", date, \"YYYYMMDD\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new DateType(new DateTime(2015, 11, 10)), result);
        }

        [TestMethod]
        public void CastTests_CastStringToDate3()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var Ds_r = sut.Execute("m2 <- cast(\"2015-Q2\", date, \"YYYY-\\QQ\");", new Operand[0]);
            var result = Ds_r.FirstOrDefault().GetValue() as ScalarType;
            Assert.AreEqual(new DateType(new DateTime(2015, 4, 1)), result);
        }

    }
}
