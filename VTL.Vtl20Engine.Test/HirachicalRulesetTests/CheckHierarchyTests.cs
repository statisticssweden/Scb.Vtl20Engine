using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Exceptions;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.HirachicalRulesetTests
{
    [TestClass]
    public class CheckHierarchyTests
    {
        [TestMethod]
        public void CheckHierarchy_Example1()
        {
            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R010:   A = J + K + L                   errorlevel 5
                              ; R020:   B = M + N + O                   errorlevel 5
                              ; R030:   C = P + Q       errorcode XX    errorlevel 5
                              ; R040:   D = R + S                       errorlevel 1
                              ; R050:   E = J                           errorlevel 0
                              ; R060:   F = Y + W + Z                   errorlevel 7
                              ; R070:   G = B + C
                              ; R080:   H = D + E                       errorlevel 0
                              ; R090:   I = D + G       errorcode YY    errorlevel 0
                              ; R100:   M >= N                          errorlevel 5
                              ; R110:   M <= G                          errorlevel 5
                  end hierarchical ruleset;";

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(11)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("C"),
                        new IntegerType(0)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("G"),
                        new IntegerType(19)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("I"),
                        new IntegerType(14)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6)
                    }),
                }));


            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 partial_null all);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(11, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.DataPointCount);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R010"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R020"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R030"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R040"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R050"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R060"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R070"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R080"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R090"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R100"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R110"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-17), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

            }
        }

        [TestMethod]
        public void CheckHierarchy_SeveralIdentifiers()
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
                    new StringType("A10"),
                    new StringType("2018-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(118554)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2018-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(128161)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2018-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(124417)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2018-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(137047)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2019-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(124907)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2019-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(132038)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2019-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(127613)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("A10"),
                    new StringType("2019-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(137375)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2018-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2018-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2018-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2018-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2019-Q1"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2019-Q2"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2019-Q3"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
                new DataPointType(new ScalarType[]
                {
                    new StringType("B10"),
                    new StringType("2019-Q4"),
                    new StringType("Förbrukning"),
                    new StringType("LP"),
                    new IntegerType(0)
                }),
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
                    new IntegerType(45440)
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
                    new IntegerType(68500)
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
                            A10: A10 = A1 + A2 + A3 errorcode ErrorA errorlevel 10;
                            B10: B10 = B1 + B2 + B3 errorcode ErrorB errorlevel 10
                        end hierarchical ruleset;

                    A10_temp <- (check_hierarchy(   sum (A group by A_row_number, NR_Referensperiod),   
                        Produktion
                        rule
                        A_row_number
                        always_zero
                        dataset
                        all)); ";

            var DS_r = sut.Execute(vtl, new[] { new Operand { Alias = "A", Data = dataset } }).FirstOrDefault(r => r.Alias.Equals("A10_temp"));

            var ruleset = hierarchicalRulesets["Produktion"];
            Assert.AreEqual("Produktion", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(2, rules.Length);

            var result = DS_r.GetValue() as DataSetType;

            var a_row_number = Array.IndexOf(result.ComponentSortOrder, "A_row_number");
            var nr_Referensperiod = Array.IndexOf(result.ComponentSortOrder, "NR_Referensperiod");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q1"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q2"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(9), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType("ErrorA"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(10), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q3"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q4"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q1"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(2), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType("ErrorA"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(10), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q2"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q3"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q4"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("A10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q1"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q2"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q3"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2018-Q4"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q1"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q2"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q3"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[a_row_number]);
                Assert.AreEqual(new StringType("2019-Q4"), dataPointEnumerator.Current[nr_Referensperiod]);
                Assert.AreEqual(new StringType("B10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new IntegerType(null), dataPointEnumerator.Current[errorlevel]);
            }
        }

        [TestMethod]
        public void CheckHierarchy_Output_invalid()
        {
            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R010:   A = J + K + L                   errorlevel 5
                              ; R020:   B = M + N + O                   errorlevel 5
                              ; R030:   C = P + Q       errorcode XX    errorlevel 5
                              ; R040:   D = R + S                       errorlevel 1
                              ; R050:   E = J                           errorlevel 0
                              ; R060:   F = Y + W + Z                   errorlevel 7
                              ; R070:   G = B + C
                              ; R080:   H = D + E                       errorlevel 0
                              ; R090:   I = D + G       errorcode YY    errorlevel 0
                              ; R100:   M >= N                          errorlevel 5
                              ; R110:   M <= G                          errorlevel 5
                  end hierarchical ruleset;";

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(11)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("C"),
                        new IntegerType(0)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("G"),
                        new IntegerType(19)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("I"),
                        new IntegerType(14)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6)
                    }),
                }));


            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 partial_null invalid);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(11, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.DataPointCount);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var me_1 = Array.IndexOf(result.ComponentSortOrder, "Me_1");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(19), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R070"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R100"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new NumberType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

            }
        }

        [TestMethod]
        public void CheckHierarchy_Output_all_measures()
        {
            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R010:   A = J + K + L                   errorlevel 5
                              ; R020:   B = M + N + O                   errorlevel 5
                              ; R030:   C = P + Q       errorcode XX    errorlevel 5
                              ; R040:   D = R + S                       errorlevel 1
                              ; R050:   E = J                           errorlevel 0
                              ; R060:   F = Y + W + Z                   errorlevel 7
                              ; R070:   G = B + C
                              ; R080:   H = D + E                       errorlevel 0
                              ; R090:   I = D + G       errorcode YY    errorlevel 0
                              ; R100:   M >= N                          errorlevel 5
                              ; R110:   M <= G                          errorlevel 5
                  end hierarchical ruleset;";

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(11)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("C"),
                        new IntegerType(0)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("G"),
                        new IntegerType(19)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("I"),
                        new IntegerType(14)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6)
                    }),
                }));


            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 partial_null all_measures);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(11, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.DataPointCount);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var me_1 = Array.IndexOf(result.ComponentSortOrder, "Me_1");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R010"), dataPointEnumerator.Current[ruleid]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(11), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R020"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R030"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("R040"), dataPointEnumerator.Current[ruleid]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("R050"), dataPointEnumerator.Current[ruleid]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue()); ;
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("R060"), dataPointEnumerator.Current[ruleid]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue()); ;
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(19), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R070"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("R080"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(14), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R090"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R100"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R110"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-17), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

            }
        }

        [TestMethod]
        public void Check_hierarchy_input_rule_priority()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);


            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(6)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("E"),
                        new IntegerType(6)
                    }),
                }));


            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R1: A = B + C
                              ; R2: C = D
                              ; R3: E = A + C
                  end hierarchical ruleset;";

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 always_zero dataset_priority all);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));
            ;

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(3, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.DataPointCount);

            var id_1 = result.OriginalIndexOfComponent("Id_1");
            var id_2 = result.OriginalIndexOfComponent("Id_2");
            var me_1 = result.OriginalIndexOfComponent("Me_1");
            var ruleid = result.OriginalIndexOfComponent("ruleid");
            var bool_var = result.OriginalIndexOfComponent("bool_var");
            var imbalance = result.OriginalIndexOfComponent("imbalance");
            var errorcode = result.OriginalIndexOfComponent("errorcode");
            var errorlevel = result.OriginalIndexOfComponent("errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);

            }
        }


        [TestMethod]
        public void CheckHierarchy_NoRuleId()
        {
            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R010:   A = J + K + L                   errorlevel 5
                              ; B = M + N + O                   errorlevel 5
                              ; C = P + Q       errorcode XX    errorlevel 5
                              ; D = R + S                       errorlevel 1
                              ; E = J                           errorlevel 0
                              ; F = Y + W + Z                   errorlevel 7
                              ; G = B + C
                              ; H = D + E                       errorlevel 0
                              ; R090:   I = D + G       errorcode YY    errorlevel 0
                              ; M >= N                          errorlevel 5
                              ; M <= G                          errorlevel 5
                  end hierarchical ruleset;";

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(11)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("C"),
                        new IntegerType(0)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("G"),
                        new IntegerType(19)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("I"),
                        new IntegerType(14)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6)
                    }),
                }));


            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 partial_null all);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(11, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.DataPointCount);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R010"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("2"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("3"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("4"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("5"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("6"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("7"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("8"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("R090"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(null), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("10"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new StringType("11"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-17), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

            }
        }

        [TestMethod]
        public void Check_hierarchy_errorcode_with_space()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);
            var nl = Environment.NewLine;

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(6)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("D"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("E"),
                        new IntegerType(6)
                    }),
                }));


            var define = $"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is{nl}";
            define += $"exempel1: A = B + C{nl}";
            define += $"; exempel namn2: C = D {nl}";
            define += $"; exempel3: E = A + C {nl}";
            define += $"end hierarchical ruleset;";

            var exception = Assert.ThrowsException<VTLParserException>(() =>
                sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 always_zero dataset_priority all);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }));
            //Assert.AreEqual("Felaktigt formaterat regelnamn: exempelnamn2. Tillåtna tecken är a-z, 0-9, _ och .", exception.Message);
        }

        [TestMethod]
        public void CheckHierarchy_Output_always_zero_all_measures()
        {
            var define = @"define hierarchical ruleset HR_1(valuedomain rule VD_1 ) is
                                R010:   A = J + K + L                   errorlevel 5
                              ; R020:   B = M + N + O                   errorlevel 5
                              ; R030:   C = P + Q       errorcode XX    errorlevel 5
                              ; R040:   D = R + S                       errorlevel 1
                              ; R050:   E = J                           errorlevel 0
                              ; R060:   F = Y + W + Z                   errorlevel 7
                              ; R070:   G = B + C
                              ; R080:   H = D + E                       errorlevel 0
                              ; R090:   I = D + G       errorcode YY    errorlevel 0
                              ; R100:   M >= N                          errorlevel 5
                              ; R110:   M <= G                          errorlevel 5
                  end hierarchical ruleset;";

            var ds1 = MockComponent.MakeDataSet(new List<MockComponent>
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
                        new StringType("A"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("B"),
                        new IntegerType(11)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("C"),
                        new IntegerType(0)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("G"),
                        new IntegerType(19)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("H"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("I"),
                        new IntegerType(14)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("M"),
                        new IntegerType(2)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("N"),
                        new IntegerType(5)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("O"),
                        new IntegerType(4)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("P"),
                        new IntegerType(7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("Q"),
                        new IntegerType(-7)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("S"),
                        new IntegerType(3)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("T"),
                        new IntegerType(9)
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("U"),
                        new NullType()
                    }),
                    new DataPointType(new ScalarType[]
                    {
                        new StringType("2010"),
                        new StringType("V"),
                        new IntegerType(6)
                    }),
                }));


            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            var DS_r = sut.Execute($"{define} DS_r <- check_hierarchy (DS_1, HR_1 rule Id_2 always_zero all_measures);"
                , new[] { new Operand { Alias = "DS_1", Data = ds1 } }).FirstOrDefault(r => r.Alias.Equals("DS_r"));

            var ruleset = hierarchicalRulesets["HR_1"];
            Assert.AreEqual("HR_1", ruleset.Name);
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(11, rules.Length);

            var result = DS_r.GetValue() as DataSetType;
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.DataPointCount);

            var id_1 = Array.IndexOf(result.ComponentSortOrder, "Id_1");
            var id_2 = Array.IndexOf(result.ComponentSortOrder, "Id_2");
            var me_1 = Array.IndexOf(result.ComponentSortOrder, "Me_1");
            var ruleid = Array.IndexOf(result.ComponentSortOrder, "ruleid");
            var bool_var = Array.IndexOf(result.ComponentSortOrder, "bool_var");
            var imbalance = Array.IndexOf(result.ComponentSortOrder, "imbalance");
            var errorcode = Array.IndexOf(result.ComponentSortOrder, "errorcode");
            var errorlevel = Array.IndexOf(result.ComponentSortOrder, "errorlevel");

            using (var dataPointEnumerator = result.DataPoints.GetEnumerator())
            {
                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("A"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(5), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R010"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("B"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(11), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R020"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("C"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R030"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("D"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R040"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(3), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.AreEqual(new NumberType(1), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("E"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R050"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("F"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R060"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(0), dataPointEnumerator.Current[imbalance]);
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("G"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(19), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R070"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(8), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("H"), dataPointEnumerator.Current[id_2]);
                Assert.IsFalse(dataPointEnumerator.Current[me_1].HasValue());
                Assert.AreEqual(new StringType("R080"), dataPointEnumerator.Current[ruleid]);
                Assert.IsFalse(dataPointEnumerator.Current[bool_var].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[imbalance].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorcode].HasValue());
                Assert.IsFalse(dataPointEnumerator.Current[errorlevel].HasValue());

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("I"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(14), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R090"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new IntegerType(-5), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType("YY"), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(0), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R100"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(false), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-3), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(5), dataPointEnumerator.Current[errorlevel]);

                dataPointEnumerator.MoveNext();
                Assert.AreEqual(new StringType("2010"), dataPointEnumerator.Current[id_1]);
                Assert.AreEqual(new StringType("M"), dataPointEnumerator.Current[id_2]);
                Assert.AreEqual(new IntegerType(2), dataPointEnumerator.Current[me_1]);
                Assert.AreEqual(new StringType("R110"), dataPointEnumerator.Current[ruleid]);
                Assert.AreEqual(new BooleanType(true), dataPointEnumerator.Current[bool_var]);
                Assert.AreEqual(new NumberType(-17), dataPointEnumerator.Current[imbalance]);
                Assert.AreEqual(new StringType(null), dataPointEnumerator.Current[errorcode]);
                Assert.AreEqual(new NumberType(null), dataPointEnumerator.Current[errorlevel]);

            }
        }

    }
}
