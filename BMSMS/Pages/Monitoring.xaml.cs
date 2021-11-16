using BMSMS.CustomControls;
using BMSMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public MainViewModel ViewModel => MainWindow.CurrentWindow.ViewModel;

        public List<VoltageCell> voltages = new List<VoltageCell>();
        private List<TemperatureCell> temperatures = new List<TemperatureCell>();

        string voltageReadings = "2";

        //For debug testing
        Random rand = new Random();

        public Monitoring()
        {
            this.InitializeComponent();

            //Generate Voltage Grid Table

            int numVoltCols = 18;
            int numVoltRows = 5;
            int cellCounter = 0;

            for (int i = 0; i < numVoltCols; ++i)
            {
                voltagesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int j = 0; j < numVoltRows; ++j)
                {
                    Binding b = new Binding()
                    {
                        Mode = BindingMode.OneWay,
                        Source = ViewModel.StateOfCharge
                    };

                    voltages.Add(new VoltageCell(cellCounter, b));
                    voltagesGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    Grid.SetColumn(voltages[cellCounter], i);
                    Grid.SetRow(voltages[cellCounter], j);

                    voltagesGrid.Children.Add(voltages[cellCounter]);
                    ++cellCounter;
                }
            }

            //Generate Temperature Grid
            int numTempCols = 9;
            int numTempRows = 5;
            int TempCounter = 0;

            for (int i = 0; i < numTempCols; ++i)
            {
                tempGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int j = 0; j < numTempRows; ++j)
                {
                    temperatures.Add(new TemperatureCell(TempCounter));
                    tempGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    Grid.SetColumn(temperatures[TempCounter], i);
                    Grid.SetRow(temperatures[TempCounter], j);

                    tempGrid.Children.Add(temperatures[TempCounter]);
                    ++TempCounter;
                }
            }

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            voltageReadings = rand.Next(5).ToString();
        }

        public void updateVoltages()
        {

        }

    }
}
