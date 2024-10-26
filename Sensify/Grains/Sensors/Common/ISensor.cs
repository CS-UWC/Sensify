namespace Sensify.Grains.Sensors.Common;

[Alias("Sensify.Grains.Sensors.Common.ISensor")]
public interface ISensor : IGrainWithStringKey, ISensorMethods
{

}
