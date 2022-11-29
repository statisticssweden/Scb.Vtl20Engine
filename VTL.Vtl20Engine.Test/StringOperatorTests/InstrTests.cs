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
using VTL.Vtl20Engine.Exceptions;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.StringOperatorTests
{

    [TestClass]
    public class InstringTest
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
                            Name = "Nummer",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Namn",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Pattern",
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
                                new StringType("Karin Fägerlind rimmar riktigt risigt"),
                                new StringType("ri")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new StringType("Kalle Anka"),
                                new StringType("kal")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(5),
                                new StringType("Leonard Larsson solar som en sorts sorglig sotare"),
                                new StringType("so")
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
                            Name = "Nummer2",
                            Role = ComponentType.ComponentRole.Identifier
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Namn2",
                            Role = ComponentType.ComponentRole.Measure
                        },
                        new MockComponent(typeof(StringType))
                        {
                            Name = "Pattern2",
                            Role = ComponentType.ComponentRole.Measure
                        }
                    },
                    new SimpleDataPointContainer(new HashSet<DataPointType>()
                    {
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(-81),
                                new StringType("Karin Fägerlind rimmar riktigt risigt"),
                                new StringType("ri")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(3),
                                new StringType("Kalle Anka"),
                                new StringType("kal")
                            }
                        ),
                        new DataPointType
                        (
                            new ScalarType[]
                            {
                                new IntegerType(5),
                                new StringType("Leonard Larsson solar som en sorts sorglig sotare"),
                                new StringType("so")
                            }
                        )
                    }))
            };
        }

        #region scalar_two_parameters
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_scalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var res = sut.Execute("res <- instr(a, \"r\" );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new IntegerType(3);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_scalar_noMatch_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var res = sut.Execute("res <- instr(a, \"z\" );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new IntegerType(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_wrongIntScalar_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new IntegerType(421) };
            var res = sut.Execute("res <- instr(a, \"r\");", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Instr är inte implemenenterad för denna datatyp.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_wrongIntPattern_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var b = new Operand
            { Alias = "b", Data = new IntegerType(421) };
            var res = sut.Execute("res <- instr(a, b);", new[] { a, b })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Andra parametern i instr måste vara ett strängvärde.", ex.Message);
        }

        #endregion scalar_two_parameters
        #region scalar_three_parameters
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_ThreeParameters_scalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var res = sut.Execute("res <- instr(a, \"r\",3 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new IntegerType(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_ThreeParameters_scalar_wrongStartType_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("karin") };
            var res = sut.Execute("res <- instr(a, \"r\",\"r\" );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_Three_Parameters_scalar_readableErrorWhenUnderscore()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",\"_\",3 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);
        }

        #endregion scalar_three_parameters
        #region scalar_four_parameters
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scalar()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",1,3 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new IntegerType(17);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scalar_startpos()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",3,3 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));

            var result = res.GetValue() as ScalarType;
            var expected = new IntegerType(15);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_ThreeParameters_scalar_wrong_startposValue_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",0,3 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);
        }
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scalar_wrong_occurenceValue_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",2,-8 );", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Fjärde parametern i instr måste vara ett positivt heltal.", ex.Message);
        }
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scalar_wrong_occurenceType_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            var res = sut.Execute("res <- instr(a, \"r\",2,\"r\");", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res"));
            var ex = Assert.ThrowsException<VtlException>(() => res.GetValue());
            Assert.AreEqual("Fjärde parametern i instr måste vara ett positivt heltal.", ex.Message);
        }

        /*
         * Dessa test funkar inte med nya G4-filen.
         * 
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scala_startEmpty_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            string exp = "Kunde inte hantera VTL-kommandot res<-instr(a,\"r\",,1);.";
            var ex = Assert.ThrowsException<VTLParserException>(() => sut.Execute("res <- instr(a, \"r\",,1);", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res")));
            Assert.AreEqual(exp, ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_scalar_occurenceEmpty_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var a = new Operand
            { Alias = "a", Data = new StringType("Karin Maria Fägerlind") };
            string exp = "Kunde inte hantera VTL-kommandot res<-instr(a,\"r\",2,);.";
            var ex = Assert.ThrowsException<VTLParserException>(() => sut.Execute("res <- instr(a, \"r\",2,);", new[] { a })
                .FirstOrDefault(r => r.Alias.Equals("res")));
            Assert.AreEqual(exp, ex.Message);
        }
        */

        #endregion scalar_four_parameters
        #region datasetComponent
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_datasetComponent()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS2 := DS_1 [drop Pattern]; DS_r  <- instr(DS2, \"ar\");", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual("int_var", result.DataSetComponents[1].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value1, new IntegerType(2));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value2, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value3, new IntegerType(5));
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_dataComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Pattern) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value1, new IntegerType(3));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value2, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value3, new IntegerType(13));
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_ThreeParameters_dataComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Pattern,Nummer) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value1, new IntegerType(3));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value2, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value3, new IntegerType(9));
            }
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_dataComponents()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Pattern,1,Nummer) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value1 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value1, new IntegerType(3));
                dataPointEnumerator.MoveNext();
                var value2 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value2, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value3 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value3, new IntegerType(36));
            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_DataComponents_2WrongType_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Nummer,Nummer,Nummer) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Andra parametern i instr måste vara ett strängvärde.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_DataComponents_3WrongType_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Pattern,Pattern,Nummer) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_DataComponents_4WrongType_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_1 [calc Me_3 := instr(Namn,Pattern,1,Pattern) ];", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Fjärde parametern i instr måste vara ett positivt heltal.", ex.Message);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_DataComponents_3ErrorValue_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2 [calc Me_3 := instr(Namn2,Pattern2,Nummer2,1) ];", new[] { _ds_2 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);
        }


        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_DataComponents_4ErrorValue_throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS_r <- DS_2 [calc Me_3 := instr(Namn2,Pattern2,1,Nummer2) ];", new[] { _ds_2 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Fjärde parametern i instr måste vara ett positivt heltal.", ex.Message);
        }
        #endregion datasetComponent 
        #region datasetComponent
        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_TwoParameters_dataset()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS2 := DS_1 [drop Pattern]; DS_r <- instr(DS2, \"i\");", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual("int_var", result.DataSetComponents[1].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value11 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value11, new IntegerType(4));
                dataPointEnumerator.MoveNext();
                var value21 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value21, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value31 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value31, new IntegerType(41));

            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_DatasetSeveralMessueres_NoCalc()
        {
            var sut = new VtlEngine(new DataContainerFactory());
            var ex = Assert.ThrowsException<VtlException>(() => sut.Execute("DS_r <- instr(DS_1, \"i\", 2 , 3);", new[] { _ds_1 }));
            Assert.AreEqual("Operatorn instr kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.", ex.Message);
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_dataset()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute(" DS2 := DS_1 [drop Pattern]; DS_r <- instr(DS2,\"i\",2,3);", new[] { _ds_1 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var result = DS_r.GetValue() as DataSetType;
            Assert.AreEqual("int_var", result.DataSetComponents[1].Name);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                var value11 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value11, new IntegerType(17));
                dataPointEnumerator.MoveNext();
                var value21 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value21, new IntegerType(0));
                dataPointEnumerator.MoveNext();
                var value31 = dataPointEnumerator.Current[1];
                Assert.AreEqual(value31, new IntegerType(0));

            }
        }

        [TestCategory("Unit")]
        [TestMethod]
        public void Instring_FourParameters_dataset_dataset_as_parameter__throwsE()
        {
            var sut = new VtlEngine(new DataContainerFactory());

            var DS_r = sut.Execute("DS2 := DS_1 [drop Pattern]; DS_r <- instr(DS2, \"i\", DS_1 , 3);", new[] { _ds_1, _ds_2 }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            var ex = Assert.ThrowsException<VtlException>(() => DS_r.GetValue());
            Assert.AreEqual("Tredje parametern i instr måste vara ett positivt heltal.", ex.Message);

        }
        #endregion dataset 
    }
}
