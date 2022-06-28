using BMSMS.Constants;
using BMSMS.CustomControls;
using BMSMS.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using BMSMS.Utilities;
using System.Collections.Generic;
using System.Drawing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BMSMS.Pages
{
    /// <summary>
    /// Page to view data read from the CAN bus
    /// </summary>
    public sealed partial class Monitoring : Page
    {
        private ColorGradient tempGradient = new ColorGradient(0, 60, Color.Green, Color.Red, 255, false);
        private MainViewModel ViewModel => MainWindow.CurrentWindow.ViewModel;

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

            for (int i = 0; i < numVoltRows; ++i)
            {
                voltagesGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                for (int j = 0; j < numVoltCols; ++j)
                {
                    voltages.Add(new VoltageCell(cellCounter));
                    voltagesGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    Grid.SetColumn(voltages[cellCounter], j);
                    Grid.SetRow(voltages[cellCounter], i);

                    voltagesGrid.Children.Add(voltages[cellCounter]);
                    ++cellCounter;
                }
            }

            //Generate Temperature Grid
            int numTempCols = 9;
            int numTempRows = 5;
            int TempCounter = 0;

            for (int i = 0; i < numTempRows; ++i)
            {
                tempGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                for (int j = 0; j < numTempCols; ++j)
                {
                    temperatures.Add(new TemperatureCell(TempCounter));
                    tempGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });                   
                    Grid.SetColumn(temperatures[TempCounter], j);
                    Grid.SetRow(temperatures[TempCounter], i);

                    tempGrid.Children.Add(temperatures[TempCounter]);
                    ++TempCounter;
                }
            }
        }

        private void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);

            //IsEnabled defaults to false
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            double totalVoltage = 0;
            double highestTemp = -99;
            double lowestVoltage = 99;
            double highestVoltage = -99;

            for (int i = 0; i < MainViewModel.totalCells; ++i)
            {
                voltages[i].Voltage = ViewModel.CellVoltages[i];
                voltages[i].IsBalancing = Convert.ToBoolean(ViewModel.CellBalancing[i]);

                totalVoltage += ViewModel.CellVoltages[i];

                if (voltages[i].Voltage < lowestVoltage)
                {
                    lowestVoltage = voltages[i].Voltage;
                }

                if (voltages[i].Voltage > highestVoltage)
                {
                    highestVoltage = voltages[i].Voltage;
                }
            }

            for (int i = 0; i < MainViewModel.totalThermistors; ++i)
            {
                temperatures[i].Temp = ViewModel.Temperatures[i];

                if (ViewModel.Temperatures[i] > highestTemp)
                {
                    highestTemp = ViewModel.Temperatures[i];
                }
            }

            soc.Text = $"{ViewModel.stateOfCharge.ToString("0.00")} %";
            current.Text = $"{ViewModel.current.ToString("0.00")} A";
            voltage.Text = $"{totalVoltage.ToString("0.00")} V";
            lowVoltage.Text = $"{lowestVoltage.ToString("0.00")} V";
            highVoltage.Text = $"{highestVoltage.ToString("0.00")} V";
            delta.Text = $"{(highestVoltage - lowestVoltage).ToString("0.00")} V";
            hightemp.Text = $"{highestTemp.ToString("0.00")} °C";
            hightemp.Foreground = new SolidColorBrush(tempGradient.getCurrentColor(highestTemp));

            if (ViewModel.ToolConnected)
            {
                if (ViewModel.MessageReceived)
                {
                    status.Foreground = new SolidColorBrush(Colors.Green);
                    status.Text = CANToolState.Active;
                }
                else
                {
                    status.Foreground = new SolidColorBrush(Colors.Yellow);
                    status.Text = CANToolState.Stale;
                }
            }
            else
            {
                status.Foreground = new SolidColorBrush(Colors.Red);
                status.Text = CANToolState.Disconnected;
            }

            if(ViewModel.tempFault)
            {
                tempFault.Foreground = new SolidColorBrush(Colors.Red);
                tempFault.Text = "FAIL";
            }
            else
            {
                tempFault.Foreground = new SolidColorBrush(Colors.Green);
                tempFault.Text = "PASS";
            }

            if (ViewModel.voltageFault)
            {
                voltageFault.Foreground = new SolidColorBrush(Colors.Red);
                voltageFault.Text = "FAIL";
            }
            else
            {
                voltageFault.Foreground = new SolidColorBrush(Colors.Green);
                voltageFault.Text = "PASS";
            }

            if (ViewModel.selfTestFail)
            {
                selfTest.Foreground = new SolidColorBrush(Colors.Red);
                selfTest.Text = "FAIL";
            }
            else
            {
                selfTest.Foreground = new SolidColorBrush(Colors.Green);
                selfTest.Text = "PASS";
            }

            if (ViewModel.overCurrent)
            {
                overCurrent.Foreground = new SolidColorBrush(Colors.Red);
                overCurrent.Text = "FAIL";
            }
            else
            {
                overCurrent.Foreground = new SolidColorBrush(Colors.Green);
                overCurrent.Text = "PASS";
            }

            if (ViewModel.senseLineOverCurrent)
            {
                senseOverCurrent.Foreground = new SolidColorBrush(Colors.Red);
                senseOverCurrent.Text = "FAIL";
            }
            else
            {
                senseOverCurrent.Foreground = new SolidColorBrush(Colors.Green);
                senseOverCurrent.Text = "PASS";
            }

            ViewModel.MessageReceived = false;
        }
    }
}