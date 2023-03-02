using System;
using System.Linq;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
	public class SensitiveEqualitySmell : ASmell
	{
        public override bool HasSmell()
        {
            var root = GetRoot();
            var methodBodyWalker = new MethodBodyWalker();
            methodBodyWalker.Visit(root);
            var invocations = methodBodyWalker.Invocations
                 .Where(x => x.Expression.ToString().Contains("Assert", StringComparison.InvariantCultureIgnoreCase));
            foreach (var invocation in invocations)
            {
                if (invocation.ArgumentList.ToString().Contains("tostring", StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        public override string Name()
        {
            return nameof(SensitiveEqualitySmell);
        }
    }
}

