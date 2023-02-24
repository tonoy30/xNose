using System;
namespace xNose.Example.Test
{
	public class CalculatorTest
	{
		[Fact]
		public void TestAdd()
		{
			if (true)
			{
				Assert.Equal(1 + 1, 2);
			}
		}
		[Fact]
		public void TestCalculateGrade()
		{
			var grade = 90;

			if (grade >= 90)
			{
				Console.WriteLine("You got an A!");
			}
			else if (grade >= 80)
			{
				Console.WriteLine("You got a B!");
			}
			else if (grade >= 70)
			{
				Console.WriteLine("You got a C!");
			}
			else if (grade >= 60)
			{
				Console.WriteLine("You got a D!");
			}
			else if (grade >= 50)
			{
				Console.WriteLine("You got an E!");
			}
			else if (grade >= 40)
			{
				Console.WriteLine("You got an F!");
			}
			else if (grade >= 30)
			{
				Console.WriteLine("You got an F!!");
			}
			else if (grade >= 20)
			{
				Console.WriteLine("You got an F!!");
			}
			else
			{
				Console.WriteLine("You failed!");
			}
		}
	}
}
