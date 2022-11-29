using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.Test.Mocks;

namespace VTL.Vtl20Engine.Test.HirachicalRulesetTests
{
    [TestClass]
    public class DefineHierachicalRulesetTests
    {
        [TestMethod]
        public void DefineHierachicalRuleset_defineRules()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory(), hierarchicalRulesets);

            sut.Execute(
                @"define hierarchical ruleset sex_hr (valuedomain rule sex) is 
                  TOTAL = MALE + FEMALE
                  end hierarchical ruleset; "
                , new Operand[0]);

            var ruleset = sut.HierarchicalRulesets["sex_hr"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual("TOTAL", rules[0].LeftCodeItem.CodeItemName);
            Assert.AreEqual(CodeItemRelationType.Coincides, rules[0].LeftCodeItem.Relation);
            var rightCodeItems = rules[0].RightCodeItems.ToArray();
            Assert.AreEqual("MALE", rightCodeItems[0].CodeItemName);
            Assert.AreEqual(CodeItemOperator.None, rightCodeItems[0].Operator);
            Assert.AreEqual("FEMALE", rightCodeItems[1].CodeItemName);
            Assert.AreEqual(CodeItemOperator.With, rightCodeItems[1].Operator);
        }

        [TestMethod]
        public void DefineHierachicalRuleset_ruleName()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory())
            {
                HierarchicalRulesets = hierarchicalRulesets
            };

            sut.Execute(
                @"define hierarchical ruleset sex_hr (valuedomain rule sex) is 
                  theRule: TOTAL = MALE + FEMALE
                  end hierarchical ruleset; "
                , new Operand[0]);

            var ruleset = sut.HierarchicalRulesets["sex_hr"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual("theRule", rules[0].RuleName);
        }

        [TestMethod]
        public void DefineHierachicalRuleset_whenCondition()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory())
            {
                HierarchicalRulesets = hierarchicalRulesets
            };

            sut.Execute(
                @"define hierarchical ruleset sex_hr (valuedomain rule sex) is 
                  when apa > 5 then TOTAL = MALE + FEMALE;
                  when apa <= 5 then TOTAL = MALE + FEMALE
                  end hierarchical ruleset; "
                , new Operand[0]);

            var ruleset = sut.HierarchicalRulesets["sex_hr"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual("apa>5", rules[0].WhenCondition);
            Assert.AreEqual("apa<=5", rules[1].WhenCondition);
        }

        [TestMethod]
        public void DefineHierachicalRuleset_error()
        {
            var hierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            var sut = new VtlEngine(new DataContainerFactory())
            {
                HierarchicalRulesets = hierarchicalRulesets
            };

            sut.Execute(
                @"define hierarchical ruleset sex_hr (valuedomain rule sex) is 
                  TOTAL = MALE + FEMALE errorcode ""fel"" errorlevel 1
                  end hierarchical ruleset; "
                , new Operand[0]);

            var ruleset = sut.HierarchicalRulesets["sex_hr"];
            var rules = ruleset.Rules.ToArray();
            Assert.AreEqual(@"""fel""", rules[0].ErrorCode);
            Assert.AreEqual(1, rules[0].ErrorLevel);
        }
    }
}
