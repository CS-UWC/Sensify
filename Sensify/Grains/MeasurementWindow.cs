namespace Sensify.Grains;

[GenerateSerializer]
[Alias("Sensify.Grains.MeasurementWindow")]
public readonly record struct MeasurementWindow
{
    public static readonly MeasurementWindow None = default;

    [Id(0)]
    public required MeasurementWindowType Type { get;  init; }
    [Id(1)]
    public TimeSpan WindowDuration { get; }
    [Id(2)]
    public int WindowSize { get; }

}
