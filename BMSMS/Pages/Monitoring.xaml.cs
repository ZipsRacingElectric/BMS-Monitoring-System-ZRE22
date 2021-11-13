using BMSMS.CustomControls;
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

namespace BMSMS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Monitoring : Page
    {

        private List<VoltageCell> voltages = new List<VoltageCell>();

        Random rand = new Random();


        public Monitoring()
        {
            this.InitializeComponent();

            //Generate Voltage Grid Table

            int numCols = 18;
            int numRows = 5;
            int cellCounter = 0;

            for (int i = 0; i < numCols; ++i)
            {
                voltagesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int j = 0; j < numRows; ++j)
                {
                    voltages.Add(new VoltageCell(cellCounter));
                    voltagesGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    Grid.SetColumn(voltages[cellCounter], i);
                    Grid.SetRow(voltages[cellCounter], j);

                    voltagesGrid.Children.Add(voltages[cellCounter]);
                    ++cellCounter;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            voltages[rand.Next(voltages.Count)].Voltage = rand.Next(5);
        }
    }
}
