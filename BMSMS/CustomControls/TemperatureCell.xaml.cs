using BMSMS.Utilities;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Drawing;

namespace BMSMS.CustomControls
{
    public sealed partial class TemperatureCell : UserControl
    {
        public int ThermistorNumber { get; set; }

        private double _temp;
        private ColorGradient tempGradient = new ColorGradient(0, 60, Color.Green, Color.Red, 255, false);

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
                if(_temp != 0)
                {
                    border.Background = new SolidColorBrush(tempGradient.getCurrentColor(_temp));
                }
                else
                {
                    border.Background = new SolidColorBrush(Colors.Gray);
                }
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
