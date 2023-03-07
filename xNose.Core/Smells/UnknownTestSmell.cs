using System;
using System.Collections.Generic;
using System.Linq;
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
                var invocations = root.DescendantNodes().OfType<InvocationExpressionSyntax>()
                .Select(x => x.Expression.ToString());
                foreach (string invocation in invocations)
                {
                    List<string> expressions = invocation.Split('.').ToList();
                    bool result = expressions.Any(i => i.Contains("Verify"));
                    if (result)
                        return false;
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