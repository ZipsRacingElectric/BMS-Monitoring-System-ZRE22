using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMSMS.Constants;

namespace BMSMS.CAN
{
    class CANMessage
    {
        public byte[] data = new byte[8];
        public int id;
        public int dlc;
        public int flags;
        public long timestamp;

        public CANMessage() { }
        public CANMessage(int id, byte[] data, int dlc, int flags, long timestamp)
        {
            switch ((CANMessageId)id)
            {
                case CANMessageId.Voltage0_3:
                    break;
                case CANMessageId.Voltage4_7:
                    break;
                case CANMessageId.Voltage8_11:
                    break;
                case CANMessageId.Voltage12_15:
                    break;
                case CANMessageId.Voltage16_19:
                    break;
                case CANMessageId.Voltage20_23:
                    break;
                case CANMessageId.Voltage24_27:
                    break;
                case CANMessageId.Voltage28_31:
                    break;
                case CANMessageId.Voltage32_35:
                    break;
                case CANMessageId.Voltage36_39:
                    break;
                case CANMessageId.Voltage40_43:
                    break;
                case CANMessageId.Voltage44_47:
                    break;
                case CANMessageId.Voltage48_51:
                    break;
                case CANMessageId.Voltage52_55:
                    break;
                case CANMessageId.Voltage56_59:
                    break;
                case CANMessageId.Voltage60_63:
                    break;
                case CANMessageId.Voltage64_67:
                    break;
                case CANMessageId.Voltage68_71:
                    break;
                case CANMessageId.Voltage72_75:
                    break;
                case CANMessageId.Voltage76_79:
                    break;
                case CANMessageId.Voltage80_83:
                    break;
                case CANMessageId.Voltage84_87:
                    break;
                case CANMessageId.Voltage88_89:
                    break;
                case CANMessageId.FuseBlown:
                    break;
                case CANMessageId.CellBalancing0_63:
                    break;
                case CANMessageId.CellBalancing64_89:
                    break;
                case CANMessageId.Temp0_3:
                    break;
                case CANMessageId.Temp4_7:
                    break;
                case CANMessageId.Temp8_11:
                    break;
                case CANMessageId.Temp12_15:
                    break;
                case CANMessageId.Temp16_19:
                    break;
                case CANMessageId.Temp20_23:
                    break;
                case CANMessageId.Temp24_27:
                    break;
                case CANMessageId.Temp28_31:
                    break;
                case CANMessageId.Temp32_35:
                    break;
                case CANMessageId.Temp36_39:
                    break;
                case CANMessageId.Temp40_43:
                    break;
                case CANMessageId.Temp44:
                    break;
                case CANMessageId.AccumulatorData:
                    break;
            }
        }
    }
}
