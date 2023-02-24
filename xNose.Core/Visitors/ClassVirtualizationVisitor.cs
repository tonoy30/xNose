using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace xNose.Core.Visitors
{
    public class ClassVirtualizationVisitor : CSharpSyntaxRewriter
    {
        const string pattern = @"Test";

        public Dictionary<ClassDeclarationSyntax, List<MethodDeclarationSyntax>> ClassWithMethods { get; set; } = new Dictionary<ClassDeclarationSyntax, List<MethodDeclarationSyntax>>();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
           
            var startOrEndWithTest = Regex.IsMatch(node.Identifier.ValueText, pattern);
            
            if(startOrEndWithTest)
            {
				var root = node.SyntaxTree.GetRoot();
                var methods = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m => Regex.IsMatch(m.Identifier.ValueText, pattern)).ToList();
                ClassWithMethods.TryAdd(node, methods);
				return node;
            }
            return null;
        }
    }
}
