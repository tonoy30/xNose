using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xNose.Core.Smells
{
    public class EagerTestSmell : ASmell
    {
        public override bool HasSmell()
        {
            var freq = new Dictionary<string, HashSet<string>>();
            var root = GetRoot();

            var invocations = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Select(x => x.Expression.ToString())
                .Where(x =>
                    !x.Contains("Assert", StringComparison.InvariantCultureIgnoreCase)
                    && !x.Contains("Console", StringComparison.InvariantCultureIgnoreCase)
                    && !x.Contains("Thread", StringComparison.InvariantCultureIgnoreCase));

            foreach (var invocation in invocations)
            {
                var invocationName = invocation.Split(".");
                if (invocationName.Length < 1) { continue; }

                var className = invocationName[0];
                var memberName = invocationName[1];
                if (freq.ContainsKey(className))
                {
                    var existingSet = freq[className];
                    existingSet.Add(memberName);
                    freq[className] = existingSet;
                }
                else
                {
                    var set = new HashSet<string> { memberName };
                    freq.Add(className, set);
                }
            }
            return freq.Any(x => freq[x.Key].Count > 1);
        }

        public override string Name()
        {
            return nameof(EagerTestSmell);
        }
    }
}