using System.Linq;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
    public class AssertionRouletteTestSmell : ASmell
    {
        const string pattern = @"Assert";

        public override bool HasSmell()
        {
            var root = GetRoot();
            var assertCount = 0;
            var methodBodyWalker = new MethodBodyWalker();
            methodBodyWalker.Visit(root);
            if (methodBodyWalker.Expressions.Any())
            {
                foreach (var expression in methodBodyWalker.Expressions)
                {
                    if (expression.StartsWith(pattern))
                    {
                        assertCount++;
                    }
                }
            }
            return assertCount > 2;
        }

        public override string Name()
        {
            return nameof(AssertionRouletteTestSmell);
        }
    }
}