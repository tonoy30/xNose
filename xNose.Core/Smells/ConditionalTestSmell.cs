using Microsoft.CodeAnalysis.CSharp.Syntax;
using xNose.Core.Walkers;

namespace xNose.Core.Smells
{
    public class ConditionalTestSmell : ASmell
	{
        public override bool HasSmell()
        {
            var tree = Node.Body.SyntaxTree;
            var conditionalWalker = new ConditionalWalker();
            conditionalWalker.StartWalker(tree.GetRoot());
            var warnings = conditionalWalker.Warnings;

            return warnings.Length != 0; 
        }

        public override string Name()
        {
            return nameof(ConditionalTestSmell);
        }
        
    }
}

