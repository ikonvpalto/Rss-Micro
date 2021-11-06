namespace Common.Extensions
{
    public static class DoubleExtensions
    {
        private const double DoubleComparisonPrecision = 0.000001;

        public static bool ApproximatelyEquals(this double source, int other)
        {
            return other - DoubleComparisonPrecision <= source
                   && source <= other + DoubleComparisonPrecision;
        }
    }
}
