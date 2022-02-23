using Microsoft.UI.Xaml.Controls;

namespace BMSMS.CustomControls
{
    public sealed partial class TemperatureCell : UserControl
    {
        public int ThermistorNumber { get; set; }

        private double _temp;

        public double Temp
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value;
                textBox.Text = value.ToString("0.00");
            }
        }
        public TemperatureCell( int thermistorNumber)
        {
            this.InitializeComponent();
            ThermistorNumber = thermistorNumber;

            textBox.Text = $"Therm {ThermistorNumber}";
        }
    }
}
