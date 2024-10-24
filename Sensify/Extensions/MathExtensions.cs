using System.Numerics;
using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float BitCastAsFloat(this ReadOnlySpan<byte> bytes)
        => Unsafe.BitCast<int, float>((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
}