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

        //used for timer interrupt to set voltages and temps
        DispatcherTimer dispatcherTimer;

        public Monitoring()
        {
            this.InitializeComponent();

            DispatcherTimerSetup();

            //Generate Voltage Grid Table
            int numVoltCols = 18;
            int numVoltRows = 5;
            int cellCounter = 0;

            for (int i = 0; i < numVoltCols; ++i)
            {
                voltagesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int j = 0; j < numVoltRows; ++j)
                {
                    voltages.Add(new VoltageCell(cellCounter));
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

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            //IsEnabled defaults to false
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
        }
        void dispatcherTimer_Tick(object sender, object e)
        {

            for (int i = 0; i < MainViewModel.totalCells; ++i)
            {
                voltages[i].Voltage = ViewModel.CellVoltages[i];
                voltages[i].IsBalancing = Convert.ToBoolean(ViewModel.CellBalancing[i]);
            }

            for (int i = 0; i < MainViewModel.totalThermistors; ++i)
            {
                temperatures[i].Temp = ViewModel.Temperatures[i];
            }
        }
    }
}