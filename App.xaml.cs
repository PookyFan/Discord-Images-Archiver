using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordImagesArchiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public enum LogLevel { Debug, Info, Error };

    public partial class App : Application
    {
        private static App appInstance;

        public static LogLevel CurrentLogLevel { get; set; } = LogLevel.Error;

        public App()
        {
            appInstance = this;
        }

        public static void Log(LogLevel severity, string message)
        {
            if(severity < CurrentLogLevel)
                return;

            string log = $"[{DateTime.Now,-19}] [{severity.ToString().ToUpper()}] {message}";
            Current.Dispatcher.Invoke(new Action(() => { (appInstance.MainWindow as MainWindow).AddLogLine(log); }));
        }
    }
}
