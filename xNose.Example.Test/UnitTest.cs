using Shouldly;
namespace xNose.Example.Test
{
    public class UnitTest
    {
        public UnitTest()
        {
            Console.WriteLine("123");
        }
        [Fact]
        public void EmptyTest()
        {

        }

        [Fact]
        public void AssertionRouletteTest()
        {
            Assert.Equal(1 + 1, 2);
            Assert.Equal(1 + 2, 3);
            Assert.Equal(2 + 2, 4);
        }

        [Fact]
        public void SleepTest()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Hello");
            Assert.Equal(true, true);
        }

        [Fact(Skip = "Skip This For Now")]
        public void SkippedTest()
        {
            Assert.Equal(1 + 1, 2);
        }
        [Fact]
        public void TestConversion()
        {
            int result = Converter.ConvertToCelsius(32);
            Assert.Equal(result, 0);

            int result2 = Converter.ConvertToCelsius(68);
            Assert.Equal(result2, 20);

            int result3 = Converter.ConvertToFahrenheit(0);
            Assert.Equal(result3, 32);

            int result4 = Converter.ConvertToFahrenheit(20);
            Assert.Equal(result4, 68);
        }
        [Fact]
        public void BoolInAssertEqualTest()
        {
            var isCycleExist = Converter.IsCycleExist(5);
            Assert.Equal(true, isCycleExist);
        }
        [Fact]
        public void EqualInAssertTest()
        {
            Assert.True(Converter.IsCycleExist(4) == true);
            Assert.False(Converter.IsCycleExist(5) == false);
        }
        [Fact]
        public void EagerTest()
        {
            var test = new Converter();
            int x = test.Method1();
            x.ShouldBe(2);
        }
        [Fact]
        public void TestMethod()
        {
            var test = new Converter();
            int x = test.Method1();
            abrakidabra(x);
        }
        private void abrakidabra(int x)
        {
            int tt = 2;
            Assert.Equal(tt, x);
        }
    }
}