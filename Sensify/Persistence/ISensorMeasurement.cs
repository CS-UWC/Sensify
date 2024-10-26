namespace Sensify.Persistence;

public interface ISensorMeasurement
{
    public string SensorId { get; }
    public DateTime Timestamp { get; }
    public object? Measurement { get; }
}
