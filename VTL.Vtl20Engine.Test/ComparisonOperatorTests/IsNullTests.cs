using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.ComparisonOperatorTests
{
    [TestClass]
    public class IsNullTests
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
                            new MockComponent(typeof(IntegerType))
                            {
                                Name = "Id",
                                Role = ComponentType.ComponentRole.Identifier
                            },
                            new MockComponent(typeof(TimePeriodType))
                            {
                                Name = "tid",
                                Role = ComponentType.ComponentRole.Measure
                            },
                            new MockComponent(typeof(IntegerType))
                            {
                                Name ="Nummer",
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
                                    new DateType(null),
                                    new IntegerType(123)
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new IntegerType(3),
                                    new DateType(null),
                                    new IntegerType(null)
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new IntegerType(5),
                                    new DateType(DateTime.Now),
                                    new IntegerType(885)
                                }
                            )
                    }))
            };

        }

        #region scalartests

        [TestCategory("Unit")]
        [TestMethod]
        public void IsNull_scalar_false()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var res = sut.Execute("res <- isnull(a);", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new BooleanType(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void IsNull_scalar_true()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType(null) };
            var res = sut.Execute("res <- isnull(a);", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new BooleanType(true);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
        #endregion scalartests;
        #region datasettests
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_dataSets_twoMeasures()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- isnull(DS_1);", new[] { _ds_1 }));
            Assert.AreEqual("Operatorn isnull kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_dataSets_IntMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := isnull(Nummer) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual(result.DataSetComponents[1].Name, "Me_3");
            var me3Index = result.IndexOfComponent("Me_3");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[me3Index];
                Assert.AreEqual(value1, new BooleanType(false));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[me3Index];
                Assert.AreEqual(value2, new BooleanType(true));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[me3Index];
                Assert.AreEqual(value3, new BooleanType(false));
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_dataSets_DateMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var DS_r = sut.Execute("DS_2:= DS_1 [drop Nummer];DS_r <- isnull(DS_2);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual(result.DataSetComponents[1].Name, "bool_var");
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value1, new BooleanType(true));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value2, new BooleanType(true));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value3, new BooleanType(false));
            }
        }
        #endregion datasettests;
    }
}
 