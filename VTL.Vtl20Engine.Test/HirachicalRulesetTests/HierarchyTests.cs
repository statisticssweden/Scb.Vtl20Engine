using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.HierarchicalAggregation;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.HirachicalRulesetTests
{
    [TestClass]
    public class HierarchyTests
    {
        private string _define;
        private DataSetType _ds1, _ds2, _ds3;

        [TestInitialize]
        public void TestSetup()
        {


            _define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                A = J + K + L
                              ; B = M + N + O
                              ; C = P + Q
                              ; D = R + S
                              ; E = T + U + V
                              ; F = Y + W + Z
                              ; G = B + C
                              ; H = D + E
                              ; I = D + G
                              ; J = J
                  end hierarchical ruleset;
            define hierarchical ruleset HR_2(valuedomain rule VD_2 ) is
                          P1 = D1 + P2 + D2 - D3
                        ; P3 = P1 + P311
                  end hierarchical ruleset;
            define hierarchical ruleset HR_3(valuedomain rule VD_3 ) is
                          A = T + P - V
                        ; B = V
                        ; C = -V
                        ; D = A - V
                        ; E = A - B
                        ; F = A - C
                        ; G = F - E
                        ; H = -C - G

                  end hierarchical ruleset;
                  define hierarchical ruleset HR_4(variable rule VD_4 ) is
                                A = T + P
                              ; B = B
                              ; C = -B
                              ; D = A + B
                              ; E = A - B + B - B
                              ; F = A - C
                              ; G = F - E
                              ; H = B - C - G

                  end hierarchical ruleset; ";


            _ds1 = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_1",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2",
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
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new IntegerType(6),
                        new StringType("Ko")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2),
                        new StringType("Dx")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5),
                        new StringType("Pz")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4),
                        new StringType("Pz")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7),
                        new StringType("Pz")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7),
                        new StringType("Pz")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3),
                        new StringType("Ay")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9),
                        new StringType("Bq")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new IntegerType(null),
                        new StringType("Nj")
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6),
                        new StringType("Ko")
                    })
                }));

            _ds2 = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_1",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Me_1",
                        Role = ComponentType.ComponentRole.Measure
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D1"),
                        new IntegerType(6)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P2"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D2"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P311"),
                        new IntegerType(4)
                    })
                }));

            _ds3 = MockComponent.MakeDataSet(new List<MockComponent>
                {
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_1",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(StringType))
                    {
                        Name = "Id_2",
                        Role = ComponentType.ComponentRole.Identifier
                    },
                    new MockComponent(typeof(IntegerType))
                    {
                        Name = "Me_1",
                        Role = ComponentType.ComponentRole.Measure
                    }
                },
                new SimpleDataPointContainer(new HashSet<DataPointType>()
                {
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D1"),
                        new IntegerType(6)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2011"),
                        new StringType("P2"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D2"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D3"),
                        new IntegerType(10)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2011"),
                        new StringType("D2"),
                        new IntegerType(null)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P311"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P1"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2011"),
                        new StringType("P1"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2011"),
                        new StringType("P311"),
                        new IntegerType(4)
                    })
                }));

        }


        [TestMethod]
        public void Hierarchy_IncludedInRule()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var engine = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);
            engine.Execute($"{_define}", new Operand[0]);

            var ruleset = hierarchicalRulesets["HR_1"];
            var sut = new Hierarchy(null, ruleset, "Id_2", InputModeHierarchy.Rule, ValidationMode.AlwaysZero, OutputModeHierarchy.Computed);

            Assert.IsNull(sut.IncludedInRule("A", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("I"))));
            Assert.IsNotNull(sut.IncludedInRule("K", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("A"))));
            Assert.IsNotNull(sut.IncludedInRule("M", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("I"))));
            Assert.IsNotNull(sut.IncludedInRule("P", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("G"))));
            Assert.IsNull(sut.IncludedInRule("D", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("I"))));
            Assert.IsNotNull(sut.IncludedInRule("R", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("I"))));
            Assert.IsNull(sut.IncludedInRule("W", ruleset.Rules.First(r => r.LeftCodeItem.CodeItemName.Equals("I"))));
        }

        [TestMethod]
        public void Hierarchy_GetNested()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var engine = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);
            engine.Execute($"{_define}", new Operand[0]);

            var ruleset = hierarchicalRulesets["HR_2"];
            var sut = new Hierarchy(null, ruleset, "Id_2", InputModeHierarchy.Rule, ValidationMode.AlwaysZero, OutputModeHierarchy.Computed);

            Assert.AreEqual(4, sut.GetNestedRule("P1").Count());
            Assert.AreEqual(5, sut.GetNestedRule("P3").Count());
        }
        /*
        [TestMethod]
        public void Hierarchy_SortDataSet()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var engine = new VtlEngine(hierarchicalRulesets);
            engine.Execute($"{_define}", new Operand[0]);

            var ruleset = hierarchicalRulesets["HR_1"];
            var sut = new Hierarchy(null, ruleset, "Id_1", Input.Rule, Mode.AlwaysZero, Output.Computed);
            sut.SortDataSet(_ds1);
            Assert.AreEqual(0,
                Array.IndexOf(_ds1.DataSetComponents,
                    _ds1.DataSetComponents.FirstOrDefault(c => c.Name.Equals("Id_1"))));
        }
        */
        [TestMethod]
        public void Hierarchy_Example1()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_1 rule Id_2 always_zero);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(10, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.DataPointCount);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(14), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("J"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_input_dataset()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_1 rule Id_2 always_zero dataset);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(10, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.DataPointCount);


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("J"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_input_rule_priority()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_1 rule Id_2 always_zero rule_priority);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(10, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(14), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("J"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_output_all()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_1 rule Id_2 always_zero _ all);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(10, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            //Assert.AreEqual(19, result.DataPointCount);


            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(14), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("J"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();

                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(2), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("N"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("O"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("P"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(7), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("Q"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(-7), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("S"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(3), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("T"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(9), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("U"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("V"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(6), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_test_med_marcus_output_all()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_2, HR_2 rule Id_2 always_zero _ all);"
                , new[] { new Operand { Alias = "DS_2", Data = _ds2 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_2"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.DataPointCount);
        }

        [TestMethod]
        public void Hierarchy_test_med_marcus_output_computed()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_2, HR_2 rule Id_2 always_zero);"
                , new[] { new Operand { Alias = "DS_2", Data = _ds2 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_2"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(13), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(17), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_test_med_marcus_input_dataset()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_3, HR_2 rule Id_2 always_zero dataset);"
                , new[] { new Operand { Alias = "DS_3", Data = _ds3 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_2"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(1), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(13), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_test_med_marcus_input_rule()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_3, HR_2 rule Id_2 always_zero rule);"
                , new[] { new Operand { Alias = "DS_3", Data = _ds3 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_2"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(1), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_test_med_marcus_input_rule_priority()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_3, HR_2 rule Id_2 always_zero rule_priority);"
                , new[] { new Operand { Alias = "DS_3", Data = _ds3 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_2"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.DataPointCount);

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(1), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2011"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("P3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(11), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_ResultIsInteger()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_1 rule Id_2 always_zero);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(10, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual("Me_1", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(IntegerType));
        }

        [TestMethod]
        public void Hierarchy_Bug102885()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);
            
            var dataset = MockComponent.MakeDataSet(new List<MockComponent>
            {
                new MockComponent(typeof(StringType))
                {
                    Name = "A_row_number",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "NR_Referensperiod",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "Text",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(StringType))
                {
                    Name = "Typ",
                    Role = ComponentType.ComponentRole.Identifier
                },
                new MockComponent(typeof(IntegerType))
                {
                    Name = "NR_Varde",
                    Role = ComponentType.ComponentRole.Measure
                }
            },
            new SimpleDataPointContainer(new HashSet<DataPointType>()
            {
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2018-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(42291)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2018-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(48408)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2018-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(43976)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2018-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(53662)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2019-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(45442)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2019-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(49323)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2019-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(45275)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A1"),
                    new StringType("2019-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(53599)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2018-Q1"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(65491)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2018-Q2"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(68509)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2018-Q3"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(70221)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2018-Q4"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(69807)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2019-Q1"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(67696)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2019-Q2"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(71010)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2019-Q3"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(72188)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A2"),
                    new StringType("2019-Q4"),
                    new StringType("Löner"),
                    new StringType("LP"),
                    new IntegerType(70067)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2018-Q1"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(10772)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2018-Q2"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(11244)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2018-Q3"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(10220)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2018-Q4"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(13578)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2019-Q1"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(11769)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2019-Q2"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(11705)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2019-Q3"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(10150)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A3"),
                    new StringType("2019-Q4"),
                    new StringType("Sociala avgifter"),
                    new StringType("LP"),
                    new IntegerType(13709)
                }),
            }));

            var vtl = @"define hierarchical ruleset Produktion(variable rule v) is
                        A10 = A1 + A2 + A3 + A4;
                            B10 = B1 + B2 + B3 + B4
                        end hierarchical ruleset;

                    A10_temp <- (hierarchy(  sum (A group by A_row_number, NR_Referensperiod),  
                        Produktion
                        rule
                        A_row_number
                        always_zero
                        rule
                        computed)); ";

            var DS_r = sut.Execute(vtl, new[] { new Operand { Alias = "A", Data = dataset } }).FirstOrDefault(r => r.Alias.Equals("A10_temp"));
            
            var ruleset = hierarchicalRulesets["Produktion"];
            Assert.AreEqual("Produktion", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            Assert.AreEqual("NR_Varde", result.DataSetComponents[2].Name);
            Assert.IsTrue(result.DataSetComponents[2].DataType == typeof(IntegerType));

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(118554), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(128161), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(124417), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(137047), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(124907), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(132038), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(127613), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(137375), dataPointEnumerator.Current[2]);
                dataPointEnumerator.MoveNext();

                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2018-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q1"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q2"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q3"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[0]);
                Assert.AreEqual(new StringType("2019-Q4"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_Bugg103961()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{_define} DS_r <- hierarchy (DS_1, HR_3 rule Id_2 always_zero);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_3"];
            Assert.AreEqual("HR_3", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(8, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.DataPointCount);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(10), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(6), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(-6), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(4), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(16), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(12), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(-6), dataPointEnumerator.Current[2]);
            }
        }

        [TestMethod]
        public void Hierarchy_Bugg111449()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);
            //B:=DS_1 [filter Id_2=\"V\"]+1;
            //temp:=calc [identifier Id_1:=/"B/"] union (B)

            var vtlText = "Bt:=(DS_1 [filter Id_2=\"V\"]) [calc identifier NyttId:=\"B\"]; B:= sum(Bt group by Id_1, NyttId); " +
            "Tt:=(DS_1 [filter Id_2=\"T\"]) [calc identifier NyttId:=\"T\"]; T:= sum(Tt group by Id_1, NyttId);" +
            "Pt:=(DS_1 [filter Id_2=\"P\"]) [calc identifier NyttId:=\"P\"]; P:= sum(Pt group by Id_1, NyttId);" +
            "temp:=union(B, T, P);";
            var DS_r = sut.Execute($"{_define} {vtlText} DS_r <- hierarchy (temp, HR_4 rule NyttId always_zero computed);"
                , new[] { new Operand { Alias = "DS_1", Data = _ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            


            var ruleset = hierarchicalRulesets["HR_4"];
            Assert.AreEqual("HR_4", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(8, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.DataPointCount);
            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(16), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(6), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(-6), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(22), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(10), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(22), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(12), dataPointEnumerator.Current[2]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[1]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[2]);
            }
        }
    }
}
