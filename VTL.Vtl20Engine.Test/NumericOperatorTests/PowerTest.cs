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

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class PowerTest
    {
        [TestMethod]
        public void Power_ScalareExponent()
        {
            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(10000),
                            }
                        )
                    }))
            };
            var x = new Operand[1] { ds };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(ds, 2)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(100000000), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Power_ComponentExponent()
        {

            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(10000),
                            }
                        )
                    }))
            };
            var exponent = new Operand() { Data = new NumberType(2), Alias = "exponent" };
            var x = new Operand[2] { ds, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(ds, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(100000000), dataPointEnumerator.Current[1]);
            }
        }
        [TestMethod]
        public void Power_ComponentExponentAndCalc()
        {

            var ds = new Operand
            {
                Alias = "ds",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(NumberType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("A"),
                                new NumberType(2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("B"),
                                new NumberType(-2),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("C"),
                                new NumberType(10000),
                            }
                        )
                    }))
            };
            var exponent = new Operand() { Data = new NumberType(1), Alias = "exponent" };
            var x = new Operand[2] { ds, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(ds, exponent+1)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(100000000), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void Power_IntVariables()
        {

            var baseComponent = new Operand() { Data = new IntegerType(3), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(3), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(27), result);
        }
        [TestMethod]
        public void Power_DecimalsBase()
        {

            var baseComponent = new Operand() { Data = new NumberType(3.5m), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(3), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(42.875m), result);
        }
        [TestMethod]
        public void Power_DecimalsExponent()
        {

            var baseComponent = new Operand() { Data = new IntegerType(2), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(2.5m), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            var decimalResult = (decimal) result * 100000;
            var integerResult = Convert.ToInt32(decimalResult);

            Assert.AreEqual(565685, integerResult);
        }

        [TestMethod]
        public void Power_Numbers()
        {
            var x = new Operand[0];
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(3,3)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(27), result);
        }

        [TestMethod]
        public void Power_ExponentZero()
        {

            var baseComponent = new Operand() { Data = new IntegerType(5), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(0), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(1), result);
        }
        [TestMethod]
        public void Power_NegativeExponent()
        {

            var baseComponent = new Operand() { Data = new IntegerType(5), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(-1), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(0.2m), result);
        }
        [TestMethod]
        public void Power_NegativeBase()
        {

            var baseComponent = new Operand() { Data = new IntegerType(-5), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(3.0m), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(-125), result);
        }
        [TestMethod]
        public void Power_NegativeBaseDecimalsExponent()
        {

            var baseComponent = new Operand() { Data = new IntegerType(-5), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(1.0001m), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Negativa tal får endast upphöjas till heltal.", e.Message);
        }

        [TestMethod]
        public void Power_NullBase()
        {

            var baseComponent = new Operand() { Data = new NumberType(null), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(3), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(null), result);
        }

        [TestMethod]
        public void Power_NullExponent()
        {

            var baseComponent = new Operand() { Data = new NumberType(3), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(null), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = dsr.GetValue() as NumberType;
            Assert.AreEqual(new NumberType(null), result);
        }

        [TestMethod]
        public void Power_ZeroBaseNegativeExponent()
        {

            var baseComponent = new Operand() { Data = new NumberType(0), Alias = "baseComponent" };
            var exponent = new Operand() { Data = new NumberType(-1), Alias = "exponent" };
            var x = new Operand[2] { baseComponent, exponent };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- power(baseComponent, exponent)", x)
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var e = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Beräkningen resulterar i division med 0", e.Message);
        }
    }
}