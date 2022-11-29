using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.BinaryNumericOperator;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class BinaryNumericOperatorTests
    {
        [TestMethod]
        public void BinaryNumericOperator_CalculatesCorrectResultStructure()
        {
            var D1 = MockComponent.MakeDataSet(new List<MockComponent>
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "A",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "B",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "C",
                    Role = ComponentType.ComponentRole.Measure
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "D",
                    Role = ComponentType.ComponentRole.Identifier
                }
            },
            new SimpleDataPointContainer(new HashSet<DataPointType>
            {
                new DataPointType(
                    new ScalarType[]
                    {
                        new StringType("2013"),
                        new StringType("Population"),
                        new IntegerType(200),
                        new StringType("Population")
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2013"),
                        new StringType("Gross Prod."),
                        new IntegerType(800),
                        new StringType("Population")
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2014"),
                        new StringType("Population"),
                        new IntegerType(250),
                        new StringType("Population")
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2014"),
                        new StringType("Gross Prod."),
                        new IntegerType(1000),
                        new StringType("Population")
                    }
                )
            }));

            var D2 = MockComponent.MakeDataSet(new List<MockComponent>
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "A",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "D",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "C",
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
                        new IntegerType(300)
                    }
                ),

                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2013"),
                        new StringType("Gross Prod."),
                        new IntegerType(900)
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2014"),
                        new StringType("Population"),
                        new IntegerType(350)
                    }
                ),
                new DataPointType
                (
                    new ScalarType[]
                    {
                        new StringType("2014"),
                        new StringType("Gross Prod."),
                        new IntegerType(1000)
                    }
                )
            }));

            var sut = new Operand
            {
                Data = new Addition(
                    new Operand
                    {
                        Data = D1
                    },
                    new Operand()
                    {
                        Data = D2
                    })
            };
            var vtlEngine = new VtlEngine(new DataContainerFactory());

            var result = sut.GetValue() as DataSetType;

            Assert.AreEqual("A", result.DataSetComponents[0].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual("B", result.DataSetComponents[1].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual("D", result.DataSetComponents[2].Name);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[2].Role);
            Assert.AreEqual("C", result.DataSetComponents[3].Name);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[3].DataType);
            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[3].Role);
        }
    }
}