using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiscordImagesArchiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            logError.Tag = LogLevel.Error;
            logInfo.Tag = LogLevel.Info;
            logDebug.Tag = LogLevel.Debug;
            logDebug.IsChecked = true;
        }

        public void AddLogLine(string txt)
        {
            logsBox.Text += txt + "\n";
        }

        private void ConnectionStateChangeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Log(LogLevel.Info, "connect");
        }

        private void RootDirBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            App.Log(LogLevel.Debug, "browse");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton chosenRadio = (sender as RadioButton);
            App.CurrentLogLevel = (LogLevel)chosenRadio.Tag;
        }
    }
}
