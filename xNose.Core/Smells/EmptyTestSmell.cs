using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xNose.Core.Smells
{
    public class EmptyTestSmell : ASmell
    {
        public MethodDeclarationSyntax Node { get; set; }
        public override string Name()
        {
            return "Empty Test Smell";
        }

        public override bool HasSmell()
        {
            return Node is not null && Node.Body.Statements.Count == 0;
        }

    }
}
