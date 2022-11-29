using System.Collections.Generic;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType
{
    public class HierarchicalRule : CompoundType
    {
        public string RuleName { get; set; }
        public CodeItemRelation LeftCodeItem { get; set; }
        public IEnumerable<CodeItemRelation> RightCodeItems { get; set; }
        public string WhenCondition { get; set; }
        public string ErrorCode { get; set; }
        public int? ErrorLevel { get; set; }
    }
}
