using System.Collections.Generic;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType
{
    public abstract class HierarchicalRuleset : Ruleset
    {
        public string Name { get; set; }
        public string Signature { get; set; }
        public ICollection<HierarchicalRule> Rules { get; set; }
    }
}
