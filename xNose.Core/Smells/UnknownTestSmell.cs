using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
    public class UnknownTestSmell : ASmell
    {
        const string pattern = @"Assert";

        public override bool HasSmell()
        {
            var assertCount = 0;
            var root = GetRoot();
            var methodWalker = new MethodBodyWalker();
            methodWalker.Visit(root);
            foreach (var expression in methodWalker.Expressions)
            {
                if (expression.StartsWith(pattern))
                {
                    assertCount++;
                }
            }
            if (assertCount == 0)
            {
                //var members = root.DescendantNodes().OfType<InvocationExpressionSyntax>();
                //Console.WriteLine($"MEthod ");
                //foreach(var invocationExpressionSyntax in members)
                //{
                //    var x = methodWalker.VisitNamespaceDeclaration;
                //    Console.WriteLine($"{x.ToString()}");
                //}
                var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Select(x => x.Expression.ToString());
                foreach (string invocation in invocations)
                {
                    //Console.WriteLine($"{invocation}");
                    List<string> expressions = invocation.Split('.').ToList();
                    bool result = expressions.Any(i => i.Contains("Verify") || i.Contains("Should") || i.Contains("When") || i.Contains("Given") || i.Contains("And") || i.Contains("Then") || i.Contains("Test") || i.Contains("Assert", StringComparison.CurrentCultureIgnoreCase) || (i.Contains("MatchSnapshot")));
                    if (result)
                        return false;
                    if (otherMethodTestSmell != null && otherMethodTestSmell.ContainsKey(invocation))
                    {
                        if (!otherMethodTestSmell[invocation][nameof(UnknownTestSmell)])
                            return false;
                    }
                }


            }
            return assertCount == 0;

        }

        public override string Name()
        {
            return nameof(UnknownTestSmell);
        }
    }
}