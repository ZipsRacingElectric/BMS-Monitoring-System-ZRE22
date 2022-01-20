using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace BMSMS.CustomControls
{
    public sealed partial class VoltageCell : UserControl
    {
        public int CellNumber { get; set; }

        private float _voltage;
        public float Voltage
        {
            get
            {
                return _voltage;
            }

            set
            {
                _voltage = value;
                textBox.Text = value.ToString();
            }
        }

        public VoltageCell(int cellNumber, Binding b)
        {
            this.InitializeComponent();
            CellNumber = cellNumber;

            textBox.SetBinding(TextBlock.TextProperty, b);
        }
    }
}
