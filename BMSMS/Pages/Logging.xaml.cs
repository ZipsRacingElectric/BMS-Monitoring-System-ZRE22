using BMSMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BMSMS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Logging : Page
    {
        //used for timer interrupt to set voltages and temps
        DispatcherTimer dispatcherTimer;

        private MainViewModel ViewModel => MainWindow.CurrentWindow.ViewModel;

        public int counter = 0;

        public Logging()
        {
            this.InitializeComponent();
            if (ViewModel.IsPaused == true)
            {
                pause.Content = "Play";
            }
            else
            {
                pause.Content = "Pause";
            }
            log.Text = ViewModel.StaticLog;
            if(ViewModel.logFile == null)
            {
                warning.Text = "No file selected. Log is not being saved!";
            }
            else
            {
                warning.Text = $"Log writing to: {ViewModel.logFile.Path}";
            }

            DispatcherTimerSetup();
        }

        private void updateLog()
        {
            ViewModel.StaticLog = "====================================================================\n" +
                             "|   ID   |   D0 D1 D2 D3 D4 D5 D6 D7   |   Delta   |   Timestamp   |\n" +
                             "====================================================================";
            foreach (var message in ViewModel.CanMessages)
            {
                ViewModel.StaticLog += $"\n   0x{message.Value.id:X3}     {message.Value.data[0]:X2} {message.Value.data[1]:X2} {message.Value.data[2]:X2} {message.Value.data[3]:X2} {message.Value.data[4]:X2} {message.Value.data[5]:X2} {message.Value.data[6]:X2} {message.Value.data[7]:X2}    {message.Value.deltaTime}   {message.Value.timestamp}";
            }

            ViewModel.StaticLog += "\n====================================================================";

            log.Text = ViewModel.StaticLog;
        }

        private void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            //IsEnabled defaults to false
            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (!ViewModel.IsPaused)
            {
                updateLog();
            }
        }

        private void onPause(object sender, RoutedEventArgs e)
        {
            ViewModel.IsPaused = !ViewModel.IsPaused;
            if (ViewModel.IsPaused)
            {
                pause.Content = "Play";
            }
            else
            {
                pause.Content = "Pause";
            }
        }

        private void onClear(object sender, RoutedEventArgs e)
        {
            ViewModel.StaticLog = "====================================================================\n" +
                             "|   ID   |   D0 D1 D2 D3 D4 D5 D6 D7   |   Delta   |   Timestamp   |\n" +
                             "====================================================================" +
                             "\n====================================================================";

            log.Text = ViewModel.StaticLog;
        }

        private async void chooseLog(object sender, RoutedEventArgs e)
        { 
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.CurrentWindow);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = $"BMS_CAN_Log_{DateTime.Now}";
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            ViewModel.logFile = file;
            if (file != null)
            {
                warning.Text = $"Log writing to: {ViewModel.logFile.Path}";
            }
        }
    }
}
