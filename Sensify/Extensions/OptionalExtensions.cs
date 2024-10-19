using System.Runtime.CompilerServices;
using Sensify.Decoders.Common;

namespace Sensify.Extensions;

public static class OptionalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Empty<T>() => new(default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Value<T>(T? value) => new(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this Optional<T> optional) => optional.Value is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> When<T>(this T obj, Factory<T?, bool> @when) => @when(obj) ? Value(obj) : Empty<T>();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> WhenNot<T>(this T obj, Factory<T?, bool> @when) => !@when(obj) ? Value(obj) : Empty<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<R> WhenNotNull<T, R>(this T obj, Factory<T, R> @when) => obj is not null ? Value(@when(obj)) : Empty<R>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Then<T>(this Optional<T> optional, Action<T> action)
    {
        if (optional.IsEmpty()) return optional;

        action(optional.Value!);

        return optional;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<R> Then<T, R>(this Optional<T> optional, Func<T, R> action)
    {
        if (optional.IsEmpty()) return Empty<R>();

        return Value(action(optional.Value!));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Optional<T> Else<T>(this Optional<T> optional, Action action)
    {
        if (!optional.IsEmpty()) return optional;

        action();
        return optional;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Else<T>(this Optional<T> optional, Func<T> action)
    {
        if (!optional.IsEmpty()) return optional.Value!;

        return action();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run<T>(this Optional<T> optional, Action<T?> action)
    {
        action(optional.Value);
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask Run<T>(this Optional<T> optional, Func<T?, ValueTask> action)
    {
        return action(optional.Value);
    }    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task Run<T>(this Optional<T> optional, Func<T?, Task> action)
    {
        return action(optional.Value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Run<T>(this T obj, Action<T> action)
    {
        action(obj);
    }    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask Run<T>(this T obj, Func<T, ValueTask> action)
    {
        return action(obj);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task Run<T>(this T obj, Func<T, Task> action)
    {
        return action(obj);
    }
}