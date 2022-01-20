using Microsoft.UI.Xaml.Controls;

namespace BMSMS.CustomControls
{
    public sealed partial class TemperatureCell : UserControl
    {
        public int ThermistorNumber { get; set; }

        private float _temp;

        public float Temp
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value;
                textBox.Text = $"{value.ToString()} °C";
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
