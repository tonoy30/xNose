using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace xNose.Core.Visitors
{
    public class ClassVirtualizationVisitor : CSharpSyntaxRewriter
    {
        const string pattern = @"Test";

        public List<ClassDeclarationSyntax> Classes { get; set; } = new List<ClassDeclarationSyntax>();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);

            var startOrEndWithTest = Regex.IsMatch(node.Identifier.ValueText, pattern);
            
            if(startOrEndWithTest)
            {
                Classes.Add(node);
                return node;
            }
            return null;
        }
    }
}
