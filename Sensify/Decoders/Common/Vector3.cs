using System.Numerics;

namespace Sensify.Decoders.Common;

[GenerateSerializer]
[Alias("Sensify.Decoders.Common.Vector3`1")]
public record Vector3<Type>(Type X, Type Y, Type Z) 
    : IAdditiveIdentity<Vector3<Type>, Vector3<Type>>,
    IAdditionOperators<Vector3<Type>, Vector3<Type>, Vector3<Type>>,
    ISubtractionOperators<Vector3<Type>, Vector3<Type>, Vector3<Type>>,
    IMultiplicativeIdentity<Vector3<Type>, Vector3<Type>>,
    IMultiplyOperators<Vector3<Type>, Type, Vector3<Type>>
    where Type : IAdditionOperators<Type, Type, Type>,
    IAdditiveIdentity<Type, Type>,
    ISubtractionOperators<Type, Type, Type>,
    IMultiplicativeIdentity<Type, Type>,
    IMultiplyOperators<Type, Type, Type>
{
    public static Vector3<Type> AdditiveIdentity { get; } = new Vector3<Type>(Type.AdditiveIdentity, Type.AdditiveIdentity, Type.AdditiveIdentity);

    public static Vector3<Type> MultiplicativeIdentity { get; } = new Vector3<Type>(Type.MultiplicativeIdentity, Type.MultiplicativeIdentity, Type.MultiplicativeIdentity);

    public static Vector3<Type> operator +(Vector3<Type> left, Vector3<Type> right)
    {
        return new Vector3<Type>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    public static Vector3<Type> operator -(Vector3<Type> left, Vector3<Type> right)
    {
        return new Vector3<Type>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    public static Vector3<Type> operator *(Vector3<Type> left, Type right)
    {
        return new Vector3<Type>(left.X * right, left.Y * right, left.Z * right);
    }
}

