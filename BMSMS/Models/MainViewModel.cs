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

        private string stateOfCharge;
        public string StateOfCharge
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

        public CoreDispatcher dispatcher;
    }
}
