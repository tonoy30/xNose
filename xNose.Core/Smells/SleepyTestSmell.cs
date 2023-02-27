using xNose.Core.Walkers;
using System.Linq;
using System;

namespace xNose.Core.Smells
{
    public class SleepyTestSmell : ASmell
    {
        public override bool HasSmell()
        {
            var root = GetRoot();
            var methodWalker = new MethodBodyWalker();
            methodWalker.Visit(root);
            return methodWalker.Expressions
            .Any(ex => ex.Contains("thread.sleep", StringComparison.InvariantCultureIgnoreCase));

        }

        public override string Name()
        {
            return nameof(SleepyTestSmell);
        }
    }
}