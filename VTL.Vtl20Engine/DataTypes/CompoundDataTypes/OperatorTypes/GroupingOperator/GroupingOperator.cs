using System.Linq;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator
{
    public abstract class GroupingOperator : Operator
    {
        protected static DataType PerformGrouping(DataSetType dataSet, string[] keptComponentNames)
        {
            var removedComponentNames = dataSet.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier &&
                            !keptComponentNames.Contains(c.Name)).Select(c => c.Name).ToArray();

            var componentList = dataSet.DataSetComponents.OrderBy(c => c.Name).ToList();

            foreach (var componentName in removedComponentNames)
            {
                var component = componentList.FirstOrDefault(c => c.Name.Equals(componentName));
                componentList.Remove(component);
            }

            var result = new DataSetType(componentList.ToArray());
            var componentCount = componentList.Count;
            foreach (var dataPoint in dataSet.DataPoints)
            {

                var newDataPoint = new DataPointType(componentCount);
                for (int i = 0; i < componentCount; i++)
                {
                    newDataPoint[result.IndexOfComponent(componentList[i])] =
                        dataPoint[dataSet.IndexOfComponent(componentList[i])];
                }
                result.Add(newDataPoint);
            }
            result.SortDataPoints();
            return result;
        }
    }
}