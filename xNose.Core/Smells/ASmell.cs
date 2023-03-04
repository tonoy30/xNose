using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xNose.Core.Smells
{
    public abstract class ASmell
    {
        public MethodDeclarationSyntax Node { get; set; }

        public abstract string Name();

        public abstract bool HasSmell();

        public virtual SyntaxNode GetRoot()
        {
			return CSharpSyntaxTree.ParseText(Node.Body?.ToFullString()).GetRoot();
		}
	}
}
