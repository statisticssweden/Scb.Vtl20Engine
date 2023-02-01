using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using VTL.Vtl20Engine.Contracts;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.Exceptions;
using VTL.Vtl20Engine.Parser;

namespace VTL.Vtl20Engine
{
    public class VtlEngine : IVtlEngine
    {
        public Dictionary<string, HierarchicalRuleset> HierarchicalRulesets;
        public IExternalFunctionExecutor ExteranExternalFunctionExecutor;
        internal static IDataContainerFactory DataContainerFactory;

        public VtlEngine(IDataContainerFactory dataContainerFactory) : this(dataContainerFactory, null)
        {
        }

        public VtlEngine(IDataContainerFactory dataContainerFactory, Dictionary<string, HierarchicalRuleset> hierarchicalRulesets)
        {
            DataContainerFactory = dataContainerFactory;
            HierarchicalRulesets = hierarchicalRulesets;
        }

        public Operand[] Execute(string vtlTransformationScheme, Operand[] inputOperands)
        {
            VtlParser.StartContext context = ParseVtlCode(vtlTransformationScheme);
            
            var heap = inputOperands.ToList();
            var visitor = new VtlVisitorImpl(heap, false)
            {
                HierarchicalRulesets = HierarchicalRulesets,
                ExternalFunctionExecutor = ExteranExternalFunctionExecutor
            };

            ValidateContext(context);
            visitor.VisitStart(context);

            return heap.ToArray();
        }

        public Operand[] Validate(string vtlTransformationScheme, Operand[] inputOperands)
        {
            VtlParser.StartContext context = ParseVtlCode(vtlTransformationScheme);
            
            var heap = inputOperands.ToList();
            var visitor = new VtlVisitorImpl(heap, true)
            {
                HierarchicalRulesets = HierarchicalRulesets,
                ExternalFunctionExecutor = ExteranExternalFunctionExecutor
            };

            ValidateContext(context);
            visitor.VisitStart(context);

            return heap.ToArray();
        }

        private VtlParser.StartContext ParseVtlCode(string vtlTransformationScheme)
        {
            var inputStream = new AntlrInputStream(vtlTransformationScheme + "\n");
            var lexer = new VtlLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new VtlParser(tokens) { BuildParseTree = true };
            var context = parser.start();
            return context;
        }

        public void ValidateSyntax (string vtlTransformationScheme)
        {
            ValidateContext(ParseVtlCode(vtlTransformationScheme));
        }

        private void ValidateContext(VtlParser.StartContext context)
        {
            if (context.exception != null)
            {
                throw new VTLParserException($"Kunde inte hantera VTL-kommandot {context.GetText()}.", context.exception);
            }

            if (context.statement() == null || !context.statement().Any())
            {
                throw new VTLParserException($"Kunde inte hantera VTL-kommandot {context.GetText()}.", context.exception);
            }

            foreach (var statementContext in context.statement())
            {
                if (statementContext == null)
                { 
                    throw new Exception($"Kunde inte hantera VTL-kommandot {statementContext.GetText()}.");
                }

                if (statementContext.exception != null)
                {
                    throw new VTLParserException($"Kunde inte hantera VTL-kommandot {statementContext.GetText()}.", statementContext.exception); ;
                }
            }
        }
    }
}