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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

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

            DispatcherTimerSetup();
        }

        private void updateLog()
        {
            log.Text = ViewModel.Log;
            myScrollViewer.ScrollToVerticalOffset(myScrollViewer.ScrollableHeight);
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
            ViewModel.Log = "";
            log.Text = "";
        }

        private async void onExport(object sender, RoutedEventArgs e)
        { 
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.CurrentWindow);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = $"BMS_CAN_Log_{DateTime.Now}";
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            await Windows.Storage.FileIO.WriteTextAsync(file, ViewModel.Log);
        }
    }
}
