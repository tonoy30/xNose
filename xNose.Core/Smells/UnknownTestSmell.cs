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
            return assertCount == 0;

        }

        public override string Name()
        {
            return nameof(UnknownTestSmell);
        }
    }
}