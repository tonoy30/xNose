using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace xNose.Core.Visitors
{
    public class ClassVirtualizationVisitor : CSharpSyntaxRewriter
    {
        public List<ClassDeclarationSyntax> Classes { get; set; } = new List<ClassDeclarationSyntax>();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
            Classes.Add(node); // save your visited classes
            return node;
        }
    }
}
