namespace Sensify.Decoders.Synetica;

public enum SyneticaDataUpType : byte
{
    Temperature = 0x01,
    Humidity = 0x02,
    AmbientLight = 0x03,
    Pressure = 0x04,
    VolatileOrganicCompounds = 0x05,
    Voc = 0x12,
    Co2e = 0x3F,
    BattVolt = 0x42,
    SoundMin = 0x50,
    SoundAvg = 0x51,
    SoundMax = 0x52
}