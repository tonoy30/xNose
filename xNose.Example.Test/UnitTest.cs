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
    }
}