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
    }
}