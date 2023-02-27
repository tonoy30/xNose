using System.Linq;
using System;

namespace xNose.Core.Smells
{
    public class IgnoreTestSmell : ASmell
    {
        public override bool HasSmell()
        {
            return Node.AttributeLists
            .SelectMany(a => a.Attributes)
            .Select(b => b.Name.ToString())
            .Any(c => c.Contains("skip", StringComparison.InvariantCultureIgnoreCase))
            || Node.AttributeLists.ToFullString().Contains("skip", StringComparison.InvariantCultureIgnoreCase);
        }

        public override string Name()
        {
            return nameof(IgnoreTestSmell);
        }
    }
}