# Scb.Vtl20Engine
# Introduction
VTL20Engine is a standalone engine for execution of code written in the VTL programming language as it is defined in version 2.0. It is written in C# with .net standard 2.0 and uses Antlr 4 runtime. In this document the implementation of the VTL engine is described from a developer’s point of view. The purpose of this text is to describe the implementation and to help developers get going in extending the VTL engine.

# Overview
The execution of VTL code can be divided into three steps:
1)	The code is parsed using Antlr and an abstract search tree Is built.
2)	The abstract search tree is then traversed using an implementation of the visitor-pattern. Every node in the tree is visited and a chain of operator objects is built.
3)	The calculation is performed as every step in the execution chain performed. Only the nodes that is needed for the computation of the desired result are executed.

# Parsing

The parsing of the VTL code is performed using the third-party component Antlr4. Using the language definition files vtl.g4 and vtlTokens.g4 compiled by Eurostat (VTL SDMX task force), Antlr4 generates code for parsing the VTL code. This code should be regenerated when updated language definition files are released using the generateCode.bat script. The script is located together with all automatically generated and externally written code in the directory \Parser\Antlr.

The following code block shows how the parser in Antlr is called:
<code>
            var inputStream = new AntlrInputStream(inputVtlCode);
            var lexer = new VtlLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new VtlParser(tokens) {BuildParseTree = true};
            var context = parser.start();
</code>

First, a character stream is created. A lexer is then created that identifies all the individual characters in the stream. The characters are then grouped as keywords and symbols, also called tokens. Then a parser is created which builds up a tree with relationships between all tokens.
