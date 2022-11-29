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

namespace VTL.Vtl20Engine.Test.StringOperatorTests
{
    [TestClass]
    public class LengthTests
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
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new StringType("2016"),
                                new StringType(null),
                                new StringType(null)
                            }
                        )
                    }))
            };

        }
        [TestCategory("Unit")]
        [TestMethod]
        public void Length_String()
        {
            var a = new Operand
            { Alias = "a", Data = new StringType("Längden på en sträng") };
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c <- length(a);", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as IntegerType;

            Assert.IsNotNull(result);
            Assert.AreEqual(20, (IntegerType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_Null()
        {
            var a = new Operand
            { Alias = "a", Data = new StringType(null) };
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c <- length(a);", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as IntegerType;
            Assert.IsFalse(result.HasValue());
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_EmptyString()
        {
            var a = new Operand
            { Alias = "a", Data = new StringType("") };
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c <- length(a);", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as IntegerType;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, (IntegerType)result);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Length_NestedExpression()
        {
            var a = new Operand
            { Alias = "a", Data = new IntegerType(132710) };
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c <- length(cast(a, string));", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as IntegerType;
            Assert.IsNotNull(result);
            Assert.AreEqual(6, (IntegerType)result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_ThrowErrorIfNotString()
        {
            var a = new Operand
            { Alias = "a", Data = new IntegerType(3874) };
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c <- length(a);", new[] { a }).FirstOrDefault(r => r.Alias.Equals("c"));
            var ex = Assert.ThrowsException<VtlException>(() => c.GetValue());
            Assert.AreEqual("Operatorn length kan bara utföras på strängar.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_ThrowErrorIfMoreThanOneMeasures()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("c <- length(DS_1);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("c")));
            Assert.AreEqual("Operatorn length kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_OneMeasure()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var c =  sut.Execute("DS_2 <- DS_1[drop stad]; c <- length(DS_2);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("c")) ;
            var result = c.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            Assert.AreEqual("int_var", result.DataSetComponents[1].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)15, dataPointEnumerator.Current[1]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)10, dataPointEnumerator.Current[1]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)15, dataPointEnumerator.Current[1]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[0]);
                Assert.IsFalse(dataPointEnumerator.Current[1].HasValue());

            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Length_GetComponentMethods()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("DS_2 <- DS_1[drop stad]; c <- length(DS_2);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("c"));
            var resComponentNames = c.GetComponentNames();
            var resIdentifierNames = c.GetIdentifierNames();
            var resMeasureNames = c.GetMeasureNames();
            var componentNames = new string[] { "RefDate", "int_var" };
            var identifierNames = new string[] { "RefDate"};
            var measureNames = new string[] { "int_var" };
            Assert.AreEqual(resComponentNames[0], componentNames[0]);
            Assert.AreEqual(resComponentNames[1], componentNames[1]);
            Assert.AreEqual(resIdentifierNames[0], identifierNames[0]);
            Assert.AreEqual(resMeasureNames[0], measureNames[0]);
        }

        [TestMethod]
        public void Length_NamedMeasures()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var c = sut.Execute("c<- DS_1[calc Me_2:= length(Namn)]; ", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("c"));
            var result = c.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2013", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)15, dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Karin Fägerlind", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Örebro", dataPointEnumerator.Current[3]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2014", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)10, dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Kalle Anka", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Ankeborg", dataPointEnumerator.Current[3]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2015", dataPointEnumerator.Current[0]);
                Assert.AreEqual((IntegerType)15, dataPointEnumerator.Current[1]);
                Assert.AreEqual((StringType)"Leonard Larsson", dataPointEnumerator.Current[2]);
                Assert.AreEqual((StringType)"Altersbruk", dataPointEnumerator.Current[3]);


                dataPointEnumerator.MoveNext();
                Assert.AreEqual((StringType)"2016", dataPointEnumerator.Current[0]);
                Assert.IsFalse(dataPointEnumerator.Current[1].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[2].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[3].HasValue());

            }
        }
    }
}
