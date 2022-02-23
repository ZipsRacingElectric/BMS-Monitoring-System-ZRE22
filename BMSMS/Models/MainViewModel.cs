using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace BMSMS.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private double stateOfCharge;
        public double StateOfCharge
        {
            get { return stateOfCharge; }
            set { SetProperty(ref stateOfCharge, value); OnPropertyChanged(nameof(stateOfCharge)); }
        }

        private string current;
        public string Current
        {
            get { return current; }
            set { SetProperty(ref current, value); OnPropertyChanged(nameof(current)); }
        }

        private string totalVoltage;
        public string TotalVoltage
        {
            get { return totalVoltage; }
            set { SetProperty(ref totalVoltage, value); OnPropertyChanged(nameof(totalVoltage)); }
        }

        private string highestTemp;
        public string HighestTemp
        {
            get { return highestTemp; }
            set { SetProperty(ref highestTemp, value); OnPropertyChanged(nameof(highestTemp)); }
        }

        public static int totalCells = 90;
        public static int totalThermistors = 45;

        public static double thermistorScale = -33.303;
        public static double thermistorOffset = 76.311;

        public double[] CellVoltages = new double[totalCells];
        public int[] CellBalancing = new int[totalCells];
        public double[] Temperatures = new double[totalThermistors];

        public CoreDispatcher dispatcher;
    }
}
