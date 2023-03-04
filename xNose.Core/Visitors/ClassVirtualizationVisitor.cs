using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System;
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
            
            //if(startOrEndWithTest)
            {
				var root = node.SyntaxTree.GetRoot();
                var methods = root.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m => m.AttributeLists.SelectMany(a => a.Attributes).Select(b => b.Name.ToString()).Any(c=>c.Equals("Fact", StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();
                if (methods!=null && methods.Count!=0)
                {
                    ClassWithMethods.TryAdd(node, methods);
                    return node;
                }
                return null;
            }
            //return null;
        }
    }
}
