namespace BMSMS.Constants
{
    enum CANMessageId
    {
        //Voltages
        Voltage0_3 = 0x401,
        Voltage4_7 = 0x402,
        Voltage8_11 = 0x403,
        Voltage12_15 = 0x404,
        Voltage16_19 = 0x405,
        Voltage20_23 = 0x406,
        Voltage24_27 = 0x407,
        Voltage28_31 = 0x408,
        Voltage32_35 = 0x409,
        Voltage36_39 = 0x40A,
        Voltage40_43 = 0x40B,
        Voltage44_47 = 0x40C,
        Voltage48_51 = 0x40D,
        Voltage52_55 = 0x40E,
        Voltage56_59 = 0x40F,
        Voltage60_63 = 0x410,
        Voltage64_67 = 0x411,
        Voltage68_71 = 0x412,
        Voltage72_75 = 0x413,
        Voltage76_79 = 0x414,
        Voltage80_83 = 0x415,
        Voltage84_87 = 0x416,
        Voltage88_89 = 0x417,

        FuseBlown = 0x421,

        CellBalancing0_63 = 0x422,
        CellBalancing64_89 = 0x423,

        Temp0_3 = 0x424,
        Temp4_7 = 0x425,
        Temp8_11 = 0x426,
        Temp12_15 = 0x427,
        Temp16_19 = 0x428,
        Temp20_23 = 0x429,
        Temp24_27 = 0x42A,
        Temp28_31 = 0x42B,
        Temp32_35 = 0x42C,
        Temp36_39 = 0x42D,
        Temp40_43 = 0x42E,
        Temp44 = 0x42F,

        AccumulatorData = 0x440
    }
}
