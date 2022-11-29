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
    public class AbsTest
    {
        [TestMethod]
        public void AbsDataset()
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
                            new NumberType(1.5m)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("B"),
                            new NumberType(-1.4m)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("C"),
                            new NumberType(0.9m)
                        }
                    )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- abs(ds)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(1.5m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(1.4m), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new NumberType(0.9m), dataPointEnumerator.Current[1]);
            }
        }

        [TestMethod]
        public void AbsDatasetWithStringMeasureThrowsException()
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
                    new MockComponent(typeof(StringType))
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
                            new StringType("1.5m")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("B"),
                            new StringType("1.5m")
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("C"),
                            new StringType("1.5m")
                        }
                    )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- abs(ds)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var ex = Assert.ThrowsException<VtlException>(() => dsr.GetValue());
            Assert.AreEqual("Värdekomponenten Me_1 har datatypen StringType vilken inte är tillåten för operatorn Abs.",
                ex.Message);
        }

        [TestMethod]
        public void AbsIntegers()
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
                            new IntegerType(19)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("B"),
                            new IntegerType(-23)
                        }
                    ),
                    new DataPointType
                    (
                        new ScalarType[]
                        {
                            new StringType("C"),
                            new IntegerType(-99)
                        }
                    )
                }))
            };

            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("r <- abs(ds)", new[] {ds})
                .FirstOrDefault(r => r.Alias.Equals("r"));
            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(19), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(23), dataPointEnumerator.Current[1]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(99), dataPointEnumerator.Current[1]);
            }
        }
    }
}