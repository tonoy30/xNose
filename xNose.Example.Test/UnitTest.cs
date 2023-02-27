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
        }

        [Fact(Skip = "Skip This For Now")]
        public void SkippedTest()
        {
            Assert.Equal(1 + 1, 2);
        }
    }
}