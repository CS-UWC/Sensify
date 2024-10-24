namespace Sensify.Grains.Sensors.Common;

[GenerateSerializer]
[Alias("Sensify.Grains.Sensors.Common.MeasurementWindow")]
public readonly record struct MeasurementWindow
{
    public static readonly MeasurementWindow None = default;

    [Id(0)]
    public required MeasurementWindowType Type { get; init; }
    [Id(1)]
    public TimeSpan WindowDuration { get; init; }
    [Id(2)]
    public int WindowSize { get; init; }
    [Id(3)]
    public bool Stream { get; init; }

}
