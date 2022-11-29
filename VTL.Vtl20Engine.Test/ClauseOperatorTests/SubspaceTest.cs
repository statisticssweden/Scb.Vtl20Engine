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

namespace VTL.Vtl20Engine.Test.ClauseOperatorTests
{
    [TestClass]
    public class SubspaceTestTests
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
                        new MockComponent(typeof(NumberType))
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
                            Name = "Id_3",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(IntegerType))
                        {
                            Name = "Me_1",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "At_1",
                            Role = ComponentType.ComponentRole.Attribure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                        {
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(1),
                                    new StringType("A"),
                                    new StringType("XX"),
                                    new IntegerType(20),
                                    new StringType("F")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(1),
                                    new StringType("A"),
                                    new StringType("YY"),
                                    new IntegerType(1),
                                    new StringType("F")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(1),
                                    new StringType("B"),
                                    new StringType("XX"),
                                    new IntegerType(4),
                                    new StringType("E")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(1),
                                    new StringType("B"),
                                    new StringType("YY"),
                                    new IntegerType(9),
                                    new StringType("F")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(2),
                                    new StringType("A"),
                                    new StringType("XX"),
                                    new IntegerType(7),
                                    new StringType("F")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(2),
                                    new StringType("A"),
                                    new StringType("YY"),
                                    new IntegerType(5),
                                    new StringType("E")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(2),
                                    new StringType("B"),
                                    new StringType("XX"),
                                    new IntegerType(12),
                                    new StringType("F")
                                }
                            ),
                            new DataPointType
                            (
                                new ScalarType[]
                                {
                                    new NumberType(2),
                                    new StringType("B"),
                                    new StringType("YY"),
                                    new IntegerType(15),
                                    new StringType("F")
                                }
                            )
                        }
                    ))
            };

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_FindTwoIndex()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_1 = 1, Id_2 = ""A"" ];", new[] {_ds_1})
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Id_3", result.DataSetComponents[0].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Me_1", result.DataSetComponents[1].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Attribure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("At_1", result.DataSetComponents[2].Name);


            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPoints.Length);


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 20, dataPointEnumerator.Current[1]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType) 1, dataPointEnumerator.Current[1]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_FindThreeIndex()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_1 = 1, Id_2 = ""B"", Id_3 = ""YY"" ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Me_1", result.DataSetComponents[0].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Attribure, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("At_1", result.DataSetComponents[1].Name);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.DataPoints.Length);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)9, dataPointEnumerator.Current[0]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_AddingTwoSubspaces()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_2 = ""A"" ] + DS_1 [ sub Id_2 = ""B"" ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[0].Role);
            Assert.AreEqual(typeof(NumberType), result.DataSetComponents[0].DataType);
            Assert.AreEqual("Id_1", result.DataSetComponents[0].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Identifier, result.DataSetComponents[1].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[1].DataType);
            Assert.AreEqual("Id_3", result.DataSetComponents[1].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Measure, result.DataSetComponents[2].Role);
            Assert.AreEqual(typeof(IntegerType), result.DataSetComponents[2].DataType);
            Assert.AreEqual("Me_1", result.DataSetComponents[2].Name);

            Assert.AreEqual(ComponentType.ComponentRole.Attribure, result.DataSetComponents[3].Role);
            Assert.AreEqual(typeof(StringType), result.DataSetComponents[3].DataType);
            Assert.AreEqual("At_1", result.DataSetComponents[3].Name);


            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPoints.Length);


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)24, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)10, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)19, dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();
                Assert.AreEqual((IntegerType)20, dataPointEnumerator.Current[2]);
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_IdentifierDoesNotExist()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var command = @"DS_r := DS_1 [ sub Id_4 = ""A"" ];";

            var e = Assert.ThrowsException<VtlException>(() => sut.Execute(command, new[] { _ds_1 }));
            Assert.AreEqual("Id_4 kunde inte hittas.", e.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_SupplyIntExpectString()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_1 = 1, Id_2 = 1, Id_3 = ""YY"" ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Identifier Id_2 har inte samma datatyp som sökt data", e.Message);

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_SupplyStringExpectInt()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_1 = ""1"", Id_2 = ""B"", Id_3 = ""YY"" ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Identifier Id_1 har inte samma datatyp som sökt data", e.Message);

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_IdentifierComponentSpecifiedTwice()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(@"DS_r := DS_1 [ sub Id_2 = ""1"", Id_2 = ""B"" ];", new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Sökt komponent får bara förekomma en gång", e.Message);

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_NoIdentifierRole()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var command = @"DS_r := DS_1 [ sub At_1 = ""A"" ];";

            var DS_r = sut.Execute(command, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var e = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Sökt komponent måste ha rollen IDENTIFIER.", e.Message);

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_GetComponentNames()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var command = @"DS_r := DS_1[sub Id_1 = 1, Id_2 = ""B""];";

            var DS_r = sut.Execute(command, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetComponentNames();
            
            Assert.AreEqual(3, result.Length);
            Assert.IsTrue(result.Contains("Id_3"));
            Assert.IsTrue(result.Contains("Me_1"));
            Assert.IsTrue(result.Contains("At_1"));
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_GetIdentifierNames()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var command = @"DS_r := DS_1[sub Id_1 = 1, Id_2 = ""B""];";

            var DS_r = sut.Execute(command, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetIdentifierNames();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result.Contains("Id_3"));

        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Sub_GetMeasureNames()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var command = @"DS_r := DS_1[sub Id_1 = 1, Id_2 = ""B""];";

            var DS_r = sut.Execute(command, new[] { _ds_1 })
                .FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var result = DS_r.GetMeasureNames();

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(result.Contains("Me_1"));

        }
    }
}