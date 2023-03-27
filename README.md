# Scb.Vtl20Engine
#Introduction
VTL20Engine is a standalone engine for execution of code written in the VTL programming language as it is defined in version 2.0. It is written in C# with .net standard 2.0 and uses Antlr 4 runtime. In this document the implementation of the VTL engine is described from a developer’s point of view. The purpose of this text is to describe the implementation and to help developers get going in extending the VTL engine.

#Overview
The execution of VTL code can be divided into three steps:
1)	The code is parsed using Antlr and an abstract search tree Is built.
2)	The abstract search tree is then traversed using an implementation of the visitor-pattern. Every node in the tree is visited and a chain of operator objects is built.
3)	The calculation is performed as every step in the execution chain performed. Only the nodes that is needed for the computation of the desired result are executed.
