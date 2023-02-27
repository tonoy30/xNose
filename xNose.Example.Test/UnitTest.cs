namespace xNose.Example.Test
{
    public class UnitTest
    {
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
    }
}