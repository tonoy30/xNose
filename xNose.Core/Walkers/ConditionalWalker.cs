using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xNose.Core.Walkers
{
	public class ConditionalWalker : CSharpSyntaxWalker
    {
        public StringBuilder Warnings { get; } = new StringBuilder();

        const string warningMessageFormat =
          "'if' with equal 'then' and 'else' blocks is found in file {0} at line {1}";

        public static bool ApplyRule(IfStatementSyntax ifStatement)
        {
            if (ifStatement.Else is null)
            {
                return true;
            }

            var thenBody = ifStatement.Statement;
            var elseBody = ifStatement.Else.Statement;

            return SyntaxFactory.AreEquivalent(thenBody, elseBody);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            if (ApplyRule(node))
            {
                int lineNumber = node.GetLocation()
                                     .GetLineSpan()
                                     .StartLinePosition.Line + 1;

                Warnings.AppendLine(string.Format(warningMessageFormat,
                                                  node.SyntaxTree.FilePath,
                                                  lineNumber));
            }
            base.VisitIfStatement(node);
        }
        public void StartWalker(SyntaxNode syntaxNode)
        {
            Warnings.Clear();
            Visit(syntaxNode);
        }
    }
}

