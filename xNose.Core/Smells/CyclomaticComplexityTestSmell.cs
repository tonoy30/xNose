using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace xNose.Core.Smells
{
	public class CyclomaticComplexityTestSmell : ASmell
	{
		
		public override bool HasSmell()
		{
			var root = GetRoot();

			var complexity = root.DescendantNodes()
								   .OfType<IfStatementSyntax>()
								   .Count()
							+ root.DescendantNodes()
									.OfType<ElseClauseSyntax>()
									.Count()
							+ root.DescendantNodes()
									.OfType<SwitchStatementSyntax>()
									.Count() + 1;
			return complexity > 10;

		}

		public override string Name()
		{
			return nameof(CyclomaticComplexityTestSmell);
		}
	}
}
