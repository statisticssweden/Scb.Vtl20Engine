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
    public class UnaryOperatorsTests
    {
        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_ScalarWithUnaryMinus()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(1.0m) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- -a;", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(-1.0m, (NumberType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_MultiplyScalarWithNegativeSelf()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(-2.0m) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a*-a;", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(-4.0m, (NumberType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_ExpressionWithUnaryMinus()
        {
            var a = new Operand
                { Alias = "a", Data = new NumberType(1.0m) };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- 10*-a;", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(-10.0m, (NumberType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_ScalarWithUnaryPlus()
        {
            var a = new Operand
                {Alias = "a", Data = new NumberType(1.0m)};
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- +a;", new[] {a}).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue();

            Assert.IsTrue(result is NumberType);
            Assert.AreEqual(1.0m, (NumberType) result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_DataSetWithUnaryMinus()
        {
            var dataSet = MockComponent.MakeDataSet(new List<MockComponent>
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "Name",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "Value",
                    Role = ComponentType.ComponentRole.Measure
                }
            },
            new SimpleDataPointContainer(new HashSet<DataPointType>()
            {
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("a"),
                        new IntegerType(2),
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("b"),
                        new IntegerType(-3),
                    }
                )
            }));

            var a = new Operand
                {Alias = "a", Data = dataSet};
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- -a;", new[] {a}).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "a", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) (-2), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "b", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) (3), dataPointEnumerator.Current[1]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_ComponentWithUnaryMinus()
        {
            var DS_1 = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Ref. Date",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Meas. Name",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Value1",
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
                            new IntegerType(200)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2013"),
                            new StringType("Gross Prod."),
                            new IntegerType(800)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Population"),
                            new IntegerType(250)
                        }),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("2014"),
                            new StringType("Gross Prod."),
                            new IntegerType(1000)
                        }
                    )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1[calc COMP_r := -Value1];", new[] { DS_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Meas. Name", result.DataSetComponents[0].Name);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Ref. Date", result.DataSetComponents[1].Name);
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
                Assert.AreEqual((IntegerType) (-800), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) (-1000), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) (-200), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) (-250), dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void UnaryOperators_MultiplyWithNegativeSelf()
        {
            var dataSet = MockComponent.MakeDataSet(new List<MockComponent> 
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "Name",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "Value",
                    Role = ComponentType.ComponentRole.Measure
                }
            },
            new SimpleDataPointContainer(new HashSet<DataPointType>()
            {
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("a"),
                        new IntegerType(2),
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("b"),
                        new IntegerType(-3),
                    }
                )
            }));

            var a = new Operand
            { Alias = "a", Data = dataSet };
            var sut = new VtlEngine(new DataContainerFactory());

            var c = sut.Execute("c <- a*-a;", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "a", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) (-4), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType) "b", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType) (-9), dataPointEnumerator.Current[1]);
            }
        }
    }
}