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
        public List<MethodDeclarationSyntax> Methods { get; set; } = new List<MethodDeclarationSyntax>();

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
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
            var startOrEndWithTest = Regex.IsMatch(node.Identifier.ValueText, pattern);
            if(startOrEndWithTest)
            {
                Methods.Add(node);
                return node;
            }
            return null;
        }
    }
}
