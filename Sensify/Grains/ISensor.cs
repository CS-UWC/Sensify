namespace Sensify.Grains;

[Alias("Sensify.Grains.ISensor")]
public interface ISensor : IGrainWithStringKey
{
    Task<string> GetIdAsync();
    Task<SensorInfo> GetSensorInfoAsync();
    Task UpdateMeasurementAsync(string hexPayload);
    Task SetSensorNameAsync(string name);
    Task SetSensorTypeAsync(string type);
    Task SetPayloadDecoderAsync(string decoderName);
    Task<T> GetMeasurementsAsync<T>(SensorMeasurementDateRange dateRange = default, MeasurementWindow window = default);

}