using System;
using System.Linq;

namespace xNose.Core.Smells
{
	public class ExpectedExceptionTestSmell : ASmell
	{
		const string key = "ExpectedException";

		public override bool HasSmell()
		{
			return Node.AttributeLists.SelectMany(a => a.Attributes).Select(b => b.Name.ToString()).Any(c => string.Equals(c, key));
		}
		

		public override string Name()
		{
			return nameof(ExpectedExceptionTestSmell);
		}
	}
}

