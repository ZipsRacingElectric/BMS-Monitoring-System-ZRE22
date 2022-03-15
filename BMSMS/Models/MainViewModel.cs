using BMSMS.Constants;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace BMSMS.Models
{
    public class MainViewModel
    {
        public static int totalCells = 90;
        public static int totalThermistors = 45;

        public static double thermistorScale = -33.303;
        public static double thermistorOffset = 76.311;

        public double stateOfCharge;

        public double current;

        public double totalVoltage;

        public double highestTemp;

        public double[] CellVoltages = new double[totalCells];

        public int[] CellBalancing = new int[totalCells];

        public double[] Temperatures = new double[totalThermistors];

        public string CANState = CANToolState.Disconnected;

        public bool ToolConnected = false;

        public bool MessageReceived = false;

    }
}
