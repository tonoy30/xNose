using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xNose.Core.Walkers
{
    public class MethodBodyWalker : CSharpSyntaxWalker
    {
        public List<string> Expressions { get; set; } = new List<string>();
        public List<string> LocalDeclarations { get; set; } = new List<string>();
        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            LocalDeclarations.Add(node.ToString().Trim());
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            Expressions.Add(node.ToString().Trim());
        }
    }
}