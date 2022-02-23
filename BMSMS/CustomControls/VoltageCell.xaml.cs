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
                    border.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    border.Background = new SolidColorBrush(Colors.Green);
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
