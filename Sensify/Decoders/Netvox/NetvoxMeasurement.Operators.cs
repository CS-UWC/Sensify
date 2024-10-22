using Sensify.Extensions;

namespace Sensify.Decoders.Netvox;
public partial record NetvoxMeasurement
{
    public static NetvoxMeasurement operator +(NetvoxMeasurement lhs, NetvoxMeasurement rhs)
    {
        return new NetvoxMeasurement
        {
            Battery = lhs is null ? rhs?.Battery : lhs.Battery.Add(rhs?.Temperature),
            Acceleration = lhs is null ? rhs?.Acceleration : lhs.Acceleration.Add(rhs?.Acceleration),
            Velocity = lhs is null ? rhs?.Velocity : lhs.Velocity.Add(rhs?.Velocity),
            Temperature = lhs is null ? rhs?.Temperature : lhs.Temperature.Add(rhs?.Temperature),
            Temperature1 = lhs is null ? rhs?.Temperature1 : lhs.Temperature1.Add(rhs?.Temperature1),
            Temperature2 = lhs is null ? rhs?.Temperature2 : lhs.Temperature2.Add(rhs?.Temperature2),
            Temperature3 = lhs is null ? rhs?.Temperature3 : lhs.Temperature3.Add(rhs?.Temperature3),
        };

    }
}
