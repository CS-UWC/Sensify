using System.Numerics;

namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.Location`1")]
public record Location<Type>(Type Latitude, Type Longitude)
    : IAdditiveIdentity<Location<Type>, Location<Type>>,
    IAdditionOperators<Location<Type>, Location<Type>, Location<Type>>,
    ISubtractionOperators<Location<Type>, Location<Type>, Location<Type>>,
    IMultiplicativeIdentity<Location<Type>, Location<Type>>,
    IMultiplyOperators<Location<Type>, Type, Location<Type>>
    where Type : IAdditionOperators<Type, Type, Type>,
    IAdditiveIdentity<Type, Type>,
    ISubtractionOperators<Type, Type, Type>,
    IMultiplicativeIdentity<Type, Type>,
    IMultiplyOperators<Type, Type, Type>
{
    public static Location<Type> AdditiveIdentity { get; } = new Location<Type>(Type.AdditiveIdentity, Type.AdditiveIdentity);

    public static Location<Type> MultiplicativeIdentity { get; } = new Location<Type>(Type.MultiplicativeIdentity, Type.MultiplicativeIdentity);

    public static Location<Type> operator +(Location<Type> left, Location<Type> right)
    {
    return new Location<Type>(left.Latitude + right.Latitude, left.Longitude + right.Longitude);
    }

    public static Location<Type> operator -(Location<Type> left, Location<Type> right)
    {
    return new Location<Type>(left.Latitude - right.Latitude, left.Longitude - right.Longitude);
    }

    public static Location<Type> operator *(Location<Type> left, Type right)
    {
    return new Location<Type>(left.Latitude * right, left.Longitude * right);
    }

}
