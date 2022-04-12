using BMSMS.CAN;
using BMSMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BMSMS
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel = new MainViewModel();
        public static MainWindow CurrentWindow;

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("monitoring", typeof(Pages.Monitoring))
        };

        public MainWindow()
        {
            this.InitializeComponent();

            CurrentWindow = this;

            CANListener listener = new CANListener() { };

            Thread t1 = new(listener.ListenAsync);
            t1.IsBackground = true;
            t1.Start();

            mainFrame.Navigate(typeof(Pages.Monitoring));
        }
    }
}
