using Kvaser.CanLib;
using System;
using System.Threading;
using System.Diagnostics;

namespace BMSMS.CAN
{
    public class CANListener
    {
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
                
                status = Canlib.canReadWait(handle, out tempMsg.id, tempMsg.data, out tempMsg.dlc, out tempMsg.flags, out tempMsg.timestamp, 100);
                switch (status)
                {
                    case Canlib.canStatus.canOK:      
                        MainWindow.CurrentWindow.DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
                            () => { 
                                MainWindow.CurrentWindow.ViewModel.StateOfCharge = $"{(tempMsg.data[4] << 8 | tempMsg.data[3])/10f} %";
                                MainWindow.CurrentWindow.ViewModel.Current = $"{tempMsg.data[0] / 10f} A";
                            }
                        );
                        //TODO: Add DB storing here
                        break;

                    case Canlib.canStatus.canERR_NOMSG:
                        //TODO: Add Error handling
                        break;

                    //Lost connection to device
                    case Canlib.canStatus.canERR_HARDWARE:
                        Debug.WriteLine("Connection to device has been interrupted. Attempting to reconnect...");
                        Canlib.canBusOff(handle);
                        Canlib.canClose(handle);
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

                        int handle = Canlib.canOpenChannel(i, Canlib.canOPEN_EXCLUSIVE);

                        //TODO: add error checking to these statuses
                        status = Canlib.canSetBusParams(handle, Canlib.canBITRATE_1M, 0, 0, 0, 0);

                        // Next, take the channel on bus using the canBusOn method. This
                        // needs to be done before we can send a message.
                        status = Canlib.canBusOn(handle);
                        return handle;
                    }
                }

                //wait 5 seconds then check for a physical device again
                Debug.WriteLine("Physical Device not found. Trying again in 5 seconds...");
                Thread.Sleep(5000);
            }
        }
    }
}