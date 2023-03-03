using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
    public class AssertionRouletteTestSmell : ASmell
    {
        const string pattern = @"Assert";

        public override bool HasSmell()
        {
            var root = GetRoot();
            var assertions = root.DescendantNodes()
                                   .OfType<InvocationExpressionSyntax>()
                                   .Where(i => i.Expression.ToString().StartsWith(pattern));

            var assertionCount = assertions.Count();
            //Console.WriteLine(string.Join(',', assertions.Select(i=>i.ToString()).ToArray()));
            if (assertionCount > 1)
            {
                var pairwiseCosineSimilarity = ComputePairwiseCosineSimilarity(assertions);

                if (pairwiseCosineSimilarity <= 0.4)
                {
                    return true;
                }
            }

            //var assertCount = 0;
            //var methodBodyWalker = new MethodBodyWalker();
            //methodBodyWalker.Visit(root);
            //if (methodBodyWalker.Expressions.Any())
            //{
            //    foreach (var expression in methodBodyWalker.Expressions)
            //    {
            //        if (expression.StartsWith(pattern))
            //        {
            //            assertCount++;
            //        }
            //    }
            //}
            //return assertCount > 2;
            return false;
        }

        public override string Name()
        {
            return nameof(AssertionRouletteTestSmell);
        }
        private double ComputePairwiseCosineSimilarity(IEnumerable<InvocationExpressionSyntax> assertions)
        {
            var vectors = new List<List<int>>();

            foreach (var assertion in assertions)
            {
                var vector = new List<int>();

                foreach (var arg in assertion.ArgumentList.Arguments)
                {
                    // Only consider arguments that are constants
                    if (arg.Expression is LiteralExpressionSyntax literal)
                    {
                        vector.Add(literal.Token.ValueText.GetHashCode());
                    }
                }

                vectors.Add(vector);
            }

            var pairwiseDotProducts = new List<double>();

            for (int i = 0; i < vectors.Count; i++)
            {
                for (int j = i + 1; j < vectors.Count; j++)
                {
                    var dotProduct = ComputeDotProduct(vectors[i], vectors[j]);
                    pairwiseDotProducts.Add(dotProduct);
                }
            }

            var cosineSimilarity = pairwiseDotProducts.Average();

            return cosineSimilarity;
        }

        private double ComputeDotProduct(IEnumerable<int> v1, IEnumerable<int> v2)
        {
            var dotProduct = v1.Zip(v2, (a, b) => a * b).Sum();
            return dotProduct;
        }
    }
}