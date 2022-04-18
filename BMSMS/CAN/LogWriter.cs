using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMSMS.CAN
{
    public class LogWriter
    {

        private DateTime prevWriteTime = DateTime.Now;
        private TimeSpan logWriteInterval = new TimeSpan(0, 0, 0, 1);

        public async void WriteHandlerAsync()
        {
            while (true)
            {
                if (MainWindow.CurrentWindow.ViewModel.logFile != null && DateTime.Now.Subtract(prevWriteTime) > logWriteInterval)
                {
                    string logCopy = MainWindow.CurrentWindow.ViewModel.Log;
                    MainWindow.CurrentWindow.ViewModel.Log = "";
                    await Windows.Storage.FileIO.AppendTextAsync(MainWindow.CurrentWindow.ViewModel.logFile, logCopy);
                }
            }
            
        }
    }
}
