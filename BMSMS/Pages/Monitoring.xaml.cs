using BMSMS.CustomControls;
using BMSMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BMSMS.Pages
{
    /// <summary>
    /// Page to view data read from the CAN bus
    /// </summary>
    public sealed partial class Monitoring : Page
    {
        public MainViewModel ViewModel => MainWindow.CurrentWindow.ViewModel;

        public List<VoltageCell> voltages = new List<VoltageCell>();
        private List<TemperatureCell> temperatures = new List<TemperatureCell>();

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
        }

        public void updateVoltages()
        {

        }

    }
}
