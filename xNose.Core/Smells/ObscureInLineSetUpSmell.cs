using System;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
	public class ObscureInLineSetUpSmell : ASmell
	{

        public override bool HasSmell()
        {
            var root = GetRoot();
            var methodWalker = new MethodBodyWalker();
            methodWalker.Visit(root);
            return methodWalker.LocalDeclarations.Count > 10;
        }

        public override string Name()
        {
            return nameof(ObscureInLineSetUpSmell);
        }
    }
}

