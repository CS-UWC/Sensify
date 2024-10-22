namespace Sensify.Decoders.Netvox;

public enum NetvoxDataUpType : byte
{
    R718E_FirmwareInfo = 0x00,
    R718E_Acceleration = 0x01,
    R718E_Velocity = 0x02,
    R718CK2_Temperature = 0x1,
    R311A_Contact = 0x1
}
