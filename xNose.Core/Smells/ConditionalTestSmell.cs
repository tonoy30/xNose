using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace xNose.Core.Smells
{
    public class ConditionalTestSmell : ASmell
    {
        public override bool HasSmell()
        {
            var root = GetRoot();

            var conditionalCount = root.DescendantNodes()
                                   .OfType<IfStatementSyntax>()
                                   .Count()
                            + root.DescendantNodes()
                                    .OfType<ElseClauseSyntax>()
                                    .Count()
                            + root.DescendantNodes()
                                    .OfType<ForEachStatementSyntax>()
                                    .Count()
                            + root.DescendantNodes()
                                    .OfType<WhileStatementSyntax>()
                                    .Count()
                           
                            + root.DescendantNodes()
                                    .OfType<ForStatementSyntax>()
                                    .Count()
                            + root.DescendantNodes()
                                    .OfType<SwitchStatementSyntax>()
                                    .Count();

            return conditionalCount > 0;
        }

        public override string Name()
        {
            return nameof(ConditionalTestSmell);
        }

    }
}
