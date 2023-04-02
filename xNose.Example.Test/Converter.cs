namespace xNose.Example.Test
{
    public class Converter
    {
        public static int ConvertToCelsius(int v)
        {
            return (v - 32) * 5 / 9;
        }

        public static int ConvertToFahrenheit(int v)
        {
            return (v * 9 / 5) + 32;
        }
        public static bool IsCycleExist(int nodeNumber)
        {
            return nodeNumber > 0;
        }
        public int Method1() { return 0; }
        public int MEthod2() { return 1; }
    }
}