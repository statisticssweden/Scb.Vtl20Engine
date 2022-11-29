namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType
{
    public class CodeItemRelation : CompoundType
    {
        public string CodeItemName { get; set; }
        public CodeItemCondition Condition { get; set; }
        public CodeItemOperator Operator { get; set; }
        public CodeItemRelationType Relation { get; set; }
    }
}
