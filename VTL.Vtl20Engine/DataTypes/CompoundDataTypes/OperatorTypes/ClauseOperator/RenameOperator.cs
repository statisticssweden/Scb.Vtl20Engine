using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class RenameOperator : Operator
    {
        protected Operand InOperand;
        protected List<Tuple<string, string>> Thesaurus;

        public RenameOperator(Operand inOperand, List<Tuple<string, string>> thesaurus)
        {
            InOperand = inOperand;
            Thesaurus = thesaurus;
        }

        internal override DataType PerformCalculation()
        {
            if (InOperand.GetValue() is DataSetType dataSet)
            {
                var result = new DataSetType(dataSet);
                var duplicates = new List<string>();
                var unknown = new List<string>();
                foreach (var translation in Thesaurus)
                {
                    if (result.DataSetComponents.Any(c => c.Name.Equals(translation.Item2)))
                    {
                        throw new Exception($"{translation.Item2} finns redan i datasetet.");
                    }
                    if (translation.Item2.Length > Constants.MAX_COMPONENT_NAME_LENGTH)
                    {
                        throw new Exception(
                            $"{translation.Item2} är för långt. Komponentnamn får inte vara längre än {Constants.MAX_COMPONENT_NAME_LENGTH} tecken.");
                    }

                    var components = result.DataSetComponents.Where(c => c.Name.Equals(translation.Item1)).ToArray();
                    var componentNames = result.DataSetComponents.Select(c => c.Name).ToArray();

                    if(!string.Equals(translation.Item1, translation.Item2, StringComparison.OrdinalIgnoreCase) &&
                        result.ComponentSortOrder.Contains(translation.Item2, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Kan inte döpa om komponenten {translation.Item1} till {translation.Item2} eftersom en komponent med det namnet redan finns i datasetet.");
                    }
                    if (components.Length == 1)
                    {
                        result.RenameComponent(translation.Item1, translation.Item2);
                    }
                    else
                    {
                        components = result.DataSetComponents.Where(c =>
                            c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(translation.Item1)).ToArray();
                        if (components.Length == 1)
                        {
                            var componentName = components[0].Name;
                            var prefix = componentName.Substring(0, componentName.IndexOf("#"));
                            result.RenameComponent(prefix + "#" + translation.Item1, prefix + "#" + translation.Item2);
                        }
                    }
                    result.SortComponents();

                    if (components.Length > 1)
                    {
                        duplicates.Add(translation.Item1);
                    }
                    if (components.Length == 0)
                    {
                        unknown.Add(translation.Item1);
                    }

                }

                if (duplicates.Any())
                {

                    throw new Exception(
                        $"Flera komponenter med namnet {string.Join(", ", duplicates)} hittades i datasetet och det oklart vilken som avses.");
                }

                if (unknown.Any())
                {
                    throw new Exception($"{string.Join(", ", unknown)} hittades inte i datasetet.");
                }

                return result;
            }
            else
            {
                throw new Exception("Rename kan bara utföras på dataset.");
            }
        }

        internal override string[] GetComponentNames()
        {
            var componentNames = InOperand.GetComponentNames();

            foreach (var translation in Thesaurus)
            {
                bool found = false;
                for (var i = 0; i < componentNames.Length; i++)
                {
                    if (componentNames[i].Contains("#") && !translation.Item1.Contains("#"))
                    {
                        if (componentNames[i].Substring(componentNames[i].IndexOf("#", StringComparison.Ordinal) + 1)
                            .Equals(translation.Item1))
                        {
                            componentNames[i] = componentNames[i].Replace(translation.Item1, translation.Item2);
                            found = true;
                            break;
                        }
                    }
                    else
                    {
                        if (componentNames[i].Equals(translation.Item1))
                        {
                            componentNames[i] = translation.Item2;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    throw new Exception($"Hittar inte komponenten {translation.Item1}");
                }
            }

            return componentNames;
        }

        internal override string[] GetIdentifierNames()
        {
            //Validering
            GetComponentNames();

            var identifierNames = InOperand.GetIdentifierNames();
            foreach (var translation in Thesaurus)
            {
                var index = Array.IndexOf(identifierNames, translation.Item1);
                if (index != -1)
                {
                    identifierNames[index] = translation.Item2;
                }
            }

            return identifierNames;
        }

        internal override string[] GetMeasureNames()
        {
            //Validering
            GetComponentNames();

            var measureNames = InOperand.GetMeasureNames();
            foreach (var translation in Thesaurus)
            {
                var index = Array.IndexOf(measureNames, translation.Item1);
                if (index != -1)
                {
                    measureNames[index] = translation.Item2;
                }
            }

            return measureNames;
        }
    }
}