using Kvaser.CanLib;
using System;
using System.Threading;
using System.Diagnostics;
using BMSMS.Constants;
using BMSMS.Models;

namespace BMSMS.CAN
{
    public class CANListener
    {
        private float[] voltages = new float[90];
        public async void ListenAsync()
        {
            int handle;

            //Initialize CAN Library and connect to hardware device
            Canlib.canInitializeLibrary();

            handle = getPhysicalDevice();

            CANMessage tempMsg = new();
            Canlib.canStatus status;

            while (true)
            {
                // Call the canReadWait method to wait for a message on the
                // channel. This method has a timeout parameter which in this
                // case is set to 100 ms. If a message is received during this
                // time, it will return the status code canOK and the message
                // will be written to the output parameters. If no message is
                // received, it will return canERR_NOMSG.
                
                status = Canlib.canReadWait(handle, out tempMsg.id, tempMsg.data, out tempMsg.dlc, out tempMsg.flags, out tempMsg.timestamp, 100); // should the timeout be changed?
                switch (status)
                {
                    case Canlib.canStatus.canOK:
                        handleCANMessage(tempMsg);
                        MainWindow.CurrentWindow.ViewModel.MessageReceived = true;
                        MainWindow.CurrentWindow.ViewModel.Log += $"ID: 0x{tempMsg.id.ToString("X3")}     Message: {tempMsg.data[0].ToString("X2")} {tempMsg.data[1]} {tempMsg.data[2]} {tempMsg.data[3]} {tempMsg.data[4]} {tempMsg.data[5]} {tempMsg.data[6]} {tempMsg.data[7]}\n";
                        break;

                    case Canlib.canStatus.canERR_NOMSG:
                        //TODO: Add Error handling
                        break;

                    // lost connection to device
                    case Canlib.canStatus.canERR_HARDWARE:
                        Debug.WriteLine("Connection to device has been interrupted. Attempting to reconnect...");
                        Canlib.canBusOff(handle);
                        Canlib.canClose(handle);
                        MainWindow.CurrentWindow.ViewModel.ToolConnected = false;
                        handle = getPhysicalDevice();
                        break;

                    default:
                        break;
                }
            }
        }

        //Get a handle for a physical device connected to the PC. Loops until a device is found.
        private int getPhysicalDevice()
        {
            int canChannelCount = 0;

            while (true)
            {
                //Get number of connected channels (includes virtual channels)
                Canlib.canEnumHardwareEx(out canChannelCount);

                //Subtract one since indexing starts at 0.
                for (int i = 0; i < canChannelCount - 1; ++i)
                {
                    Canlib.canStatus status = Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_CARD_HARDWARE_REV, out object hardwareRev);
                    if (status >= 0 && (System.Int64)hardwareRev > 0)
                    {
                        Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_DEVDESCR_ASCII, out object device_name);
                        Canlib.canGetChannelData(i, Canlib.canCHANNELDATA_CHAN_NO_ON_CARD, out object device_channel);

                        Debug.WriteLine("Physical Device found! Found channel: {0} {1} {2}", i, device_name, ((UInt32)device_channel + 1));

                        int handle = Canlib.canOpenChannel(i, Canlib.canOPEN_OVERRIDE_EXCLUSIVE); //Change to Canlib.canOPEN_EXCLUSIVE when done testing

                        //TODO: add error checking to these statuses
                        status = Canlib.canSetBusParams(handle, Canlib.canBITRATE_1M, 0, 0, 0, 0);

                        // Next, take the channel on bus using the canBusOn method. This
                        // needs to be done before we can send a message.
                        status = Canlib.canBusOn(handle);

                        MainWindow.CurrentWindow.ViewModel.ToolConnected = true;

                        return handle;
                    }
                }

                //wait 5 seconds then check for a physical device again
                Debug.WriteLine("Physical Device not found. Trying again in 5 seconds...");
                Thread.Sleep(5000);
            }
        }

        private void handleCANMessage(CANMessage message)
        {
            switch ((CANMessageId)message.id)
            {
                case CANMessageId.Voltage0_3:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[0] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[1] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[2] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[3] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage4_7:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[4] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[5] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[6] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[7] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage8_11:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[8] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[9] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[10] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[11] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage12_15:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[12] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[13] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[14] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[15] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage16_19:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[16] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[17] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[18] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[19] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage20_23:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[20] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[21] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[22] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[23] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage24_27:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[24] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[25] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[26] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[27] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage28_31:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[28] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[29] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[30] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[31] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage32_35:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[32] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[33] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[34] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[35] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage36_39:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[36] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[37] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[38] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[39] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage40_43:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[40] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[41] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[42] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[43] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage44_47:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[44] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[45] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[46] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[47] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage48_51:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[48] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[49] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[50] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[51] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage52_55:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[52] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[53] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[54] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[55] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage56_59:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[56] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[57] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[58] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[59] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage60_63:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[60] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[61] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[62] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[63] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage64_67:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[64] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[65] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[66] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[67] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage68_71:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[68] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[69] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[70] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[71] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage72_75:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[72] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[73] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[74] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[75] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage76_79:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[76] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[77] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[78] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[79] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage80_83:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[80] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[81] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[82] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[83] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage84_87:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[84] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[85] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[86] = (message.data[4] << 8 | message.data[5]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[87] = (message.data[6] << 8 | message.data[7]) / 10000f;
                    break;
                case CANMessageId.Voltage88_89:
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[88] = (message.data[0] << 8 | message.data[1]) / 10000f;
                    MainWindow.CurrentWindow.ViewModel.CellVoltages[89] = (message.data[2] << 8 | message.data[3]) / 10000f;
                    break;
                case CANMessageId.FuseBlown:

                    break;
                case CANMessageId.CellBalancing0_35:
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[0] = message.data[0] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[1] = (message.data[0] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[2] = (message.data[0] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[3] = (message.data[0] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[4] = (message.data[0] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[5] = (message.data[0] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[6] = (message.data[0] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[7] = (message.data[0] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[8] = message.data[1] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[9] = (message.data[1] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[10] = (message.data[1] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[11] = (message.data[1] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[12] = (message.data[1] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[13] = (message.data[1] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[14] = (message.data[1] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[15] = (message.data[1] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[16] = message.data[2] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[17] = (message.data[2] >> 1) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[18] = message.data[3] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[19] = (message.data[3] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[20] = (message.data[3] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[21] = (message.data[3] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[22] = (message.data[3] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[23] = (message.data[3] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[24] = (message.data[3] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[25] = (message.data[3] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[26] = message.data[4] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[27] = (message.data[4] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[28] = (message.data[4] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[29] = (message.data[4] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[30] = (message.data[4] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[31] = (message.data[4] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[32] = (message.data[4] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[33] = (message.data[4] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[34] = message.data[5] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[35] = (message.data[5] >> 1) & 0b1;

                    break;

                case CANMessageId.CellBalancing36_71:
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[36] = message.data[0] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[37] = (message.data[0] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[38] = (message.data[0] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[39] = (message.data[0] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[40] = (message.data[0] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[41] = (message.data[0] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[42] = (message.data[0] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[43] = (message.data[0] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[44] = message.data[1] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[45] = (message.data[1] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[46] = (message.data[1] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[47] = (message.data[1] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[48] = (message.data[1] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[49] = (message.data[1] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[50] = (message.data[1] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[51] = (message.data[1] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[52] = message.data[2] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[53] = (message.data[2] >> 1) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[54] = message.data[3] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[55] = (message.data[3] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[56] = (message.data[3] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[57] = (message.data[3] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[58] = (message.data[3] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[59] = (message.data[3] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[60] = (message.data[3] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[61] = (message.data[3] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[62] = message.data[4] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[63] = (message.data[4] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[64] = (message.data[4] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[65] = (message.data[4] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[66] = (message.data[4] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[67] = (message.data[4] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[68] = (message.data[4] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[69] = (message.data[4] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[70] = message.data[5] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[71] = (message.data[5] >> 1) & 0b1;

                    break;

                case CANMessageId.CellBalancing72_89:
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[72] = message.data[0] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[73] = (message.data[0] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[74] = (message.data[0] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[75] = (message.data[0] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[76] = (message.data[0] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[77] = (message.data[0] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[78] = (message.data[0] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[79] = (message.data[0] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[80] = message.data[1] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[81] = (message.data[1] >> 1) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[82] = (message.data[1] >> 2) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[83] = (message.data[1] >> 3) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[84] = (message.data[1] >> 4) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[85] = (message.data[1] >> 5) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[86] = (message.data[1] >> 6) & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[87] = (message.data[1] >> 7) & 0b1;

                    MainWindow.CurrentWindow.ViewModel.CellBalancing[88] = message.data[2] & 0b1;
                    MainWindow.CurrentWindow.ViewModel.CellBalancing[89] = (message.data[2] >> 1) & 0b1;

                    break;

                case CANMessageId.Temp0_3:
                    MainWindow.CurrentWindow.ViewModel.Temperatures[0] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[1] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[2] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[3] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                 
                case CANMessageId.Temp4_7:                                                                                 
                    MainWindow.CurrentWindow.ViewModel.Temperatures[4] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[5] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[6] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[7] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                 
                case CANMessageId.Temp8_11:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[8] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[9] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[10] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[11] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp12_15:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[12] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[13] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[14] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[15] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp16_19:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[16] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[17] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[18] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[19] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp20_23:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[20] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[21] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[22] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[23] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp24_27:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[24] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[25] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[26] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[27] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp28_31:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[28] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[29] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[30] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[31] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp32_35:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[32] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[33] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[34] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[35] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp36_39:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[36] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[37] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[38] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[39] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp40_43:                                                                                
                    MainWindow.CurrentWindow.ViewModel.Temperatures[40] = (message.data[0] << 8 | message.data[1]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[41] = (message.data[2] << 8 | message.data[3]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[42] = (message.data[4] << 8 | message.data[5]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    MainWindow.CurrentWindow.ViewModel.Temperatures[43] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;                                                                                                  
                case CANMessageId.Temp44:                                                                                   
                    MainWindow.CurrentWindow.ViewModel.Temperatures[44] = (message.data[6] << 8 | message.data[7]) / 10000f * MainViewModel.thermistorScale + MainViewModel.thermistorOffset;
                    break;
                case CANMessageId.AccumulatorData:
                    MainWindow.CurrentWindow.ViewModel.stateOfCharge = (message.data[1] << 8 | message.data[0]) / 10f;
                    
                    MainWindow.CurrentWindow.ViewModel.current = (sbyte)(message.data[5] << 8 | message.data[4]) / 10f; // cs low reading
                    MainWindow.CurrentWindow.ViewModel.highCurrent = (sbyte)(message.data[3] << 8 | message.data[2]) / 10f; // cs high reading
                    MainWindow.CurrentWindow.ViewModel.tempFault = Convert.ToBoolean((message.data[6] >> 0) & 0b1);
                    MainWindow.CurrentWindow.ViewModel.voltageFault = Convert.ToBoolean((message.data[6] >> 1) & 0b1);
                    MainWindow.CurrentWindow.ViewModel.selfTestFail = Convert.ToBoolean((message.data[6] >> 2) & 0b1);
                    MainWindow.CurrentWindow.ViewModel.overCurrent = Convert.ToBoolean((message.data[6] >> 3) & 0b1);
                    MainWindow.CurrentWindow.ViewModel.senseLineOverCurrent = Convert.ToBoolean((message.data[6] >> 4) & 0b1);
                    break;
                default:
                    break;

            }
        }
    }
}