using Common.Enums;
using Common.Utils;
using DatasetCatalog.ExternalFunctionExecutor;
using DataSetCatalog.ExternalExecution.Contracts;
using DataSetCatalog.ExternalExecution.Vtl;
using DataSetCatalog.ExternalFunction.Contracts;
using DataSetCatalog.ExternalFunction.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.GeneralPurposeOperatorTests
{
    [TestClass]
    public class EvalTests
    {
        private Mock<IExternalFunctionStore> _externalExecution;

        private Operand OperandForTest()
        {
            var Operand = new Operand
            {
                Alias = "DS_1",
                Data = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "ref_area",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "time",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "obs_value",
                        Role = ComponentType.ComponentRole.Measure
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "obs_status",
                        Role = ComponentType.ComponentRole.Attribure
                    }
                },
                new SimpleDataPointContainer(
                    new HashSet<DataPointType>
                    {
                        new DataPointType(
                            new ScalarType[]
                            {
                                new StringType("Area"),
                                new StringType("Tid"),
                                new IntegerType(1),
                                new StringType("Obs")
                            }
                        )
                    }
                ))
            };
            return Operand;
        }

        [TestMethod]
        public void Eval_Vtl()
        {
            _externalExecution = new Mock<IExternalFunctionStore>();
            _externalExecution.Setup(e => e.GetExternalFunction("add")).Returns(
                new ExternalFunction
                {
                    Implementations = new[]
                    {
                        new Implementation
                        {
                            Type = ExternalFunctionTypeEnum.VTL,
                            CommandSource = "r <- a1 + a2;",
                            Result =
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 0,
                                    ParameterName = "r"
                                },
                            Parameters = new[]
                            {
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 1,
                                    ParameterName = "a1"
                                },
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 2,
                                    ParameterName = "a2"
                                },
                            }
                        }
                    }
                });

            var serviceProvider = new Mock<IServiceProviderWrapper>();
            serviceProvider.Setup(sp => sp.GetServices<IExternalExecution>()).Returns(new[] 
            {
                new VtlExecutor(new DataContainerFactory())
            });
            var sut = new VtlEngine(new DataContainerFactory())
            {
                ExteranExternalFunctionExecutor = new ExternalFunctionExecutor(_externalExecution.Object, serviceProvider.Object)
            };

            var DS_r = sut.Execute("r <- eval ( add ( a1, a2) language \"vtl\");", new[]
            {
                new Operand() {Alias = "a1", Data = new IntegerType(2)},
                new Operand() {Alias = "a2", Data = new IntegerType(3)}
            });
            var result = DS_r.FirstOrDefault(r => r.Persistant).GetValue();
            
            Assert.AreEqual(new IntegerType(5), result);
        }

        [TestMethod]
        public void Eval_AddEvalResultToDs()
        {
            _externalExecution = new Mock<IExternalFunctionStore>();
            _externalExecution.Setup(e => e.GetExternalFunction("add")).Returns(
                new ExternalFunction
                {
                    Implementations = new[]
                    {
                        new Implementation
                        {
                            Type = ExternalFunctionTypeEnum.VTL,
                            CommandSource = "r <- a1 + a2;",
                            Result =
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 0,
                                    ParameterName = "r"
                                },
                            Parameters = new[]
                            {
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 1,
                                    ParameterName = "a1"
                                },
                                new Parameter()
                                {
                                    DataType = DataTypeEnum.Integer,
                                    Order = 2,
                                    ParameterName = "a2"
                                },
                            }
                        }
                    }
                });

            var serviceProvider = new Mock<IServiceProviderWrapper>();
            serviceProvider.Setup(sp => sp.GetServices<IExternalExecution>()).Returns(new[]
            {
                new VtlExecutor(new DataContainerFactory())
            });
            var sut = new VtlEngine(new DataContainerFactory())
            {
                ExteranExternalFunctionExecutor = new ExternalFunctionExecutor(_externalExecution.Object, serviceProvider.Object)
            };

            var DS_r = sut.Execute("r <- DS_1 + eval ( add ( a1, a2));", new[]
            {
                OperandForTest(),
                new Operand() {Alias = "a1", Data = new IntegerType(2)},
                new Operand() {Alias = "a2", Data = new IntegerType(3)}
            });

            var result = DS_r.FirstOrDefault(r => r.Persistant).GetValue() as DataSetType;

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new IntegerType(6), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Eval_Test1()
        {
            _externalExecution = new Mock<IExternalFunctionStore>();
            var ds_1 = OperandForTest();

            var sut = new VtlEngine(new DataContainerFactory());
            var command = "d <- eval( SQL3( DS_1 ) language \"SQL\" returns integer)";


            var res = sut.Execute(command, new Operand[] {ds_1}).FirstOrDefault(r => r.Persistant);
        }

        [TestMethod]
        public void Eval_Test()
        {
            _externalExecution = new Mock<IExternalFunctionStore>();
            var ds_1 = OperandForTest();

            var sut = new VtlEngine(new DataContainerFactory());

            var res = sut.Execute(
                    @"d <- eval( SQL3( DS_1 ) language ""SQL""
                      returns dataset { identifier<number> ref_area,
                      identifier<date> time,
                      measure<number> obs_value,
                      attribute<string> obs_status } )"
                , new Operand[] { ds_1 }).FirstOrDefault(r => r.Persistant);
        }

        [TestMethod]
        public void Eval_ParseEval()
        {
            _externalExecution = new Mock<IExternalFunctionStore>();
            var ds_1 = OperandForTest();
            var sut = new VtlEngine(new DataContainerFactory());

            var returns =
                "returns dataset { identifier<number> ref_area, identifier<date> time, measure<number> obs_value, attribute<string> obs_status }";
            var command = $"d <- eval( SQL3( DS_1 ) language \"SQL\" {returns})";


            var res = sut.Execute(command, new Operand[] {ds_1}).FirstOrDefault(r => r.Persistant);
            /*
            var components = res.GetComponentNames();
            Assert.AreEqual(4, components.Length);

            var identifiers = res.GetIdentifierNames();
            Assert.AreEqual(2, identifiers.Length);

            var measures = res.GetMeasureNames();
            Assert.AreEqual(1, measures.Length);
            */
        }
    }
}