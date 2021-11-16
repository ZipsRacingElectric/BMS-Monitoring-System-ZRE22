using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
