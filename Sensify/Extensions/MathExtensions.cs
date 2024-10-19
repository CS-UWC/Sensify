using System.Numerics;

namespace Sensify.Extensions;

public static class MathExtensions
{
    public static T Max<T>(this T @this, T other) where T : IComparisonOperators<T, T, bool>
    {
        return @this > other ? @this : other;
    }
    
    public static T Min<T>(this T @this, T other) where T : IComparisonOperators<T, T, bool>
    {
        return @this < other ? @this : other;
    }
    
    public static TimeSpan Max(this TimeSpan @this, TimeSpan other)
    {
        return @this > other ? @this : other;
    }
    
    public static TimeSpan Min(this TimeSpan @this, TimeSpan other)
    {
        return @this < other ? @this : other;
    }
}