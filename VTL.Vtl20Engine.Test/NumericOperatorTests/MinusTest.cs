using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class MinusTest
    {
        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_subtractNumberAndIntegerWithVTLCode()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(3.5m) };
            var b = new Operand
                { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(1.5m, (NumberType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_subtractTwoIntegers()
        {
            var a = new IntegerType(1);
            var b = new IntegerType(2);

            var c = a - b;

            Assert.IsTrue(c is IntegerType);
            Assert.AreEqual(-1, c);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_subtractTwoIntegersWithVTLCode()
        {
            var a = new Operand
                { Alias = "a", Data = new IntegerType(5) };
            var b = new Operand
                { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is IntegerType);
            Assert.AreEqual(3, (IntegerType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_multiLineVTLCode()
        {
            var a = new Operand
                { Alias = "a", Data = new IntegerType(5) };
            var b = new Operand
                { Alias = "b", Data = new IntegerType(8) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- d - e;\r\nd := b - e;\r\ne := b - a;", new[] { a, b })
                .FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is IntegerType);
            Assert.AreEqual(2, (IntegerType)result);
        }
        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_FiveIntegersWithVTLCode()
        {
            var a = new Operand
                { Alias = "a", Data = new IntegerType(1) };
            var b = new Operand
                { Alias = "b", Data = new IntegerType(2) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- 1 + a * 3 - b - 2;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is IntegerType);
            Assert.AreEqual(0, (IntegerType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_subtractNullFromNumber()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(1.0m) };
            var b = new Operand
                { Alias = "b", Data = new NumberType(null) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as NumberType;

            Assert.IsNotNull(result);
            Assert.IsNull((decimal?)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_subtractTwoDecimals()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(1.01m) };
            var b = new Operand
                { Alias = "b", Data = new NumberType(0.001m) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as NumberType;

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(1.009m, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_IntegerToInteger()
        {
            var a = new Operand
            { Alias = "a", Data = new IntegerType(3) };
            var b = new Operand
            { Alias = "b", Data = new IntegerType(1) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsTrue(c.GetValue() is IntegerType);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_DecimalToDecimal()
        {
            var a = new Operand
            { Alias = "a", Data = new NumberType(new decimal(2.6)) };
            var b = new Operand
            { Alias = "b", Data = new NumberType(new decimal(1.6)) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));

            Assert.IsTrue(c.GetValue() is NumberType);
            Assert.IsFalse(c.GetValue() is IntegerType);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_BooleanFriendlyErrorMessage()
        {
            var a = new Operand
            { Alias = "a", Data = new NumberType(new decimal(1.4)) };
            var b = new Operand
            { Alias = "b", Data = new BooleanType(false) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a - b;", new[] { a, b }).FirstOrDefault(r => r.Alias.Equals("c"));

            var e = Assert.ThrowsException<VtlException>(() => c.GetValue());
            Assert.AreEqual("Värdet False har datatypen BooleanType vilken inte är tillåten för operatorn Minus.", e.Message);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_BooleanFriendlyErrorMessageDataSets()
        {
            var _ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },

                        new MockComponent(typeof(BooleanType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                 new SimpleDataPointContainer(new HashSet<DataPointType>()
                 {
                        new DataPointType
                        (
                            new  ScalarType[]
                            {
                                new IntegerType(1),
                                new BooleanType(true)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new BooleanType(false)
                            }
                        )
                }))
            };

            var _ds_2 = new Operand
            {
                Alias = "DS_2",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_1",
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
                                new IntegerType(2)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new IntegerType(4)
                            }
                        )
                      }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 - DS_2;", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Värdekomponenten Me_1 har datatypen BooleanType vilken inte är tillåten för operatorn Minus.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Minus_BooleanFriendlyErrorMessageDataSetAndScalar()
        {
            var _ds_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                    {
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Id_1",
                            Role = ComponentType.ComponentRole.Identifier
                        },

                        new MockComponent(typeof(BooleanType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                 new SimpleDataPointContainer(new HashSet<DataPointType>()
                 {
                        new DataPointType
                        (
                            new  ScalarType[]
                            {
                                new IntegerType(1),
                                new BooleanType(true)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new BooleanType(false)
                            }
                        )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 - 10", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Värdekomponenten Me_1 har datatypen BooleanType vilken inte är tillåten för operatorn Minus.", e.Message);
        }
    }
}
