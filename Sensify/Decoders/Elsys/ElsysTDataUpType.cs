namespace Sensify.Decoders;

internal enum ElsysTDataUpType : byte{
    Temp = 0x01, //temp 2 bytes -3276.8°C -->3276.7°C
    Rh = 0x02, //Humidity 1 byte  0-100%
    Acc = 0x03, //acceleration 3 bytes X,Y,Z -128 --> 127 +/-63=1G
    Light = 0x04, //Light 2 bytes 0-->65535 Lux
    Motion = 0x05, //No of motion 1 byte  0-255
    Co2 = 0x06, //Co2 2 bytes 0-65535 ppm
    Vdd = 0x07, //VDD 2byte 0-65535mV
    Analog1 = 0x08, //VDD 2byte 0-65535mV
    Gps = 0x09, //3bytes lat 3bytes long binary
    Pulse1 = 0x0A, //2bytes relative pulse count
    Pulse1Abs = 0x0B, //4bytes no 0->0xFFFFFFFF
    ExtTemp1 = 0x0C, //2bytes -3276.5C-->3276.5C
    ExtDigital = 0x0D, //1bytes value 1 or 0
    ExtDistance = 0x0E, //2bytes distance in mm
    AccMotion = 0x0F, //1byte number of vibration/motion
    IrTemp = 0x10, //2bytes internal temp 2bytes external temp -3276.5C-->3276.5C
    Occupancy = 0x11, //1byte data
    WaterLeak = 0x12, //1byte data 0-255
    GridEye = 0x13, //65byte temperature data 1byte ref+64byte external temp
    Pressure = 0x14, //4byte pressure data (hPa)
    Sound = 0x15, //2byte sound data (peak/avg)
    Pulse2 = 0x16, //2bytes 0-->0xFFFF
    Pulse2Abs = 0x17, //4bytes no 0->0xFFFFFFFF
    Analog2 = 0x18, //2bytes voltage in mV
    ExtTemp2 = 0x19, //2bytes -3276.5C-->3276.5C
    ExtDigital2 = 0x1A, // 1bytes value 1 or 0
    ExtAnalogUV = 0x1B, // 4 bytes signed int (uV)
    TVOC = 0x1C, // 2 bytes (ppb)
    Debug = 0x3D // 4bytes debug
}