using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.StringOperatorTests
{
    [TestClass]
    public class ConcatTest
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
                        new MockComponent(typeof(StringType))
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
                                new StringType("hello")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("B"),
                                new StringType("hi")
                            }
                        )
                    }))
            };

            _ds_2 = new Operand
            {
                Alias = "DS_2",
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
                        new MockComponent(typeof(StringType))
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
                                new StringType("world"),
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(2),
                                new StringType("B"),
                                new StringType("there")
                            }
                        )
                    }))
            };
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Concat_constantScalars()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var res = sut.Execute("r <- \"hello\" || \" world\";", new Operand[0])
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = res.GetValue() as ScalarType;

            Assert.IsNotNull(result);
            Assert.AreEqual((StringType)"hello world", result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Concat_constantDatasetAndScalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var res = sut.Execute("r <- DS_1 || \" world\";", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = res.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hello world", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hi world", dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Concat_constantDatasets()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var res = sut.Execute("r <- DS_1 || DS_2;", new[] { _ds_1, _ds_2 })
                .FirstOrDefault(r => r.Alias.Equals("r"));

            var result = res.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"helloworld", dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"hithere", dataPointEnumerator.Current[2]);
            }
        }
    }
}
