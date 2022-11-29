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

namespace VTL.Vtl20Engine.Test.NumericOperatorTests
{
    [TestClass]
    public class ExpTest
    {

        private Operand DataSet1()
        {
            var ds_1 = new Operand
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
                        new MockComponent(typeof(NumberType))
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
                 new SimpleDataPointContainer(new HashSet<DataPointType>
                     {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("A"),
                                new IntegerType(5),
                                new NumberType((decimal)0.7545)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(10),
                                new StringType("B"),
                                new IntegerType(0),
                                new NumberType((decimal)13.45)
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(11),
                                new StringType("A"),
                                new IntegerType(-1),
                                new NumberType((decimal)1.87)
                            }
                        ),
                    }))
            };
            return ds_1;
        }

        [TestMethod]
        public void Test_Exp_Example1()
        {
            var ds1 = DataSet1();
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- exp(DS_1)", new Operand[] { ds1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var apa = Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType));

                Assert.AreEqual(148.413, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.001);
                Assert.AreEqual(2.126547, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.000001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(1, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)));
                Assert.AreEqual(693842.3, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.1);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(0.36787, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(6.488296, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.000001);
            }
        }

        [TestMethod]
        public void Test_Exp_Example2()
        {
            var ds1 = DataSet1();
            var sut = new VtlEngine(new DataContainerFactory());

            var dsr = sut.Execute("DS_r <- DS_1 [calc Me_1 := exp( Me_1 )] ", new Operand[] { ds1 })
                .FirstOrDefault(ds_r => ds_r.Alias.Equals("DS_r"));

            var result = dsr.GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var apa = Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType));

                Assert.AreEqual(148.413, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.001);
                Assert.AreEqual(0.7545, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.0001);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(1, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)));
                Assert.AreEqual(13.45, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.01);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(0.36787, Convert.ToDouble((decimal)(dataPointEnumerator.Current[2] as NumberType)), (double)0.00001);
                Assert.AreEqual(1.87, Convert.ToDouble((decimal)(dataPointEnumerator.Current[3] as NumberType)), (double)0.01);
            }
        }
    }
}
