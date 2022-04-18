using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace BMSMS.CustomControls
{
    public sealed partial class VoltageCell : UserControl
    {
        public int CellNumber { get; set; }

        private double _voltage;
        public double Voltage
        {
            get
            {
                return _voltage;
            }

            set
            {
                _voltage = value;
                voltageBox.Text = value.ToString("0.000");
            }
        }

        private bool _isBalancing;
        public bool IsBalancing
        {
            get
            {
                return _isBalancing;
            }
            set 
            {
                _isBalancing = value;
                if (_isBalancing)
                {
                    border.Background = new SolidColorBrush(Colors.Aqua);
                    voltageBox.Foreground = new SolidColorBrush(Colors.Black);
                }
                else if (_voltage != 0)
                {
                    border.Background = new SolidColorBrush(Colors.Green);
                    voltageBox.Foreground = new SolidColorBrush(Colors.White);

                }
                else
                {
                    border.Background = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        public VoltageCell(int cellNumber)
        {
            this.InitializeComponent();
            CellNumber = cellNumber;
        }
    }
}
