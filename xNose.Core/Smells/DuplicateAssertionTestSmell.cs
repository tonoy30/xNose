using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace xNose.Core.Smells
{
    public class DuplicateAssertionTestSmell : ASmell
    {
        public override bool HasSmell()
        {
            var freq = new Dictionary<string, int>();
            var root = GetRoot();

            var invocations = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Select(x => x.Expression.ToString())
                .Where(x => !x.Contains("Assert", StringComparison.InvariantCultureIgnoreCase));

            foreach (var invocation in invocations)
            {
                if (freq.ContainsKey(invocation))
                {
                    freq[invocation] += 1;
                }
                else
                {
                    freq.Add(invocation, 1);
                }
            }
            return freq.Any(x => freq[x.Key] > 1);
        }

        public override string Name()
        {
            return nameof(DuplicateAssertionTestSmell);
        }
    }
}

