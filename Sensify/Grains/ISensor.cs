namespace Sensify.Grains;

[Alias("Sensify.Grains.ISensor`1")]
public interface ISensor<TMeasurement> : IGrainWithStringKey, ISensor
{
    Task<TMeasurement> GetTypedMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);

}

public interface ISensor : IGrainWithStringKey
{
    Task<string> GetIdAsync();
    Task<SensorInfo> GetSensorInfoAsync();
    Task UpdateSensorInfoAsync(UpdateSensorInfo update);
    Task UpdateMeasurementAsync(string hexPayload);
    Task<Dictionary<object, object>> GetMeasurementsAsync(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);

}