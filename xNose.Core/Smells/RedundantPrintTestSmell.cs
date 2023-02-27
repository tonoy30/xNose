using System;
using System.Linq;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
    public class RedundantPrintTestSmell : ASmell
    {

        public override bool HasSmell()
        {
            var root = GetRoot();
            var methodWalker = new MethodBodyWalker();
            methodWalker.Visit(root);
            return methodWalker.Expressions
            .Any(ex => ex.Contains("console.write", StringComparison.InvariantCultureIgnoreCase));
        }

        public override string Name()
        {
            return nameof(RedundantPrintTestSmell);
        }
    }
}

