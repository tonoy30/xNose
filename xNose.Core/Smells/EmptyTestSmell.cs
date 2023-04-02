using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace xNose.Core.Smells
{
    public class EmptyTestSmell : ASmell
    {
        public override string Name()
        {
            return nameof(EmptyTestSmell);
        }

        public override bool HasSmell()
        {
            return Node is not null && Node.Body?.Statements.Count == 0;
        }

    }
}
