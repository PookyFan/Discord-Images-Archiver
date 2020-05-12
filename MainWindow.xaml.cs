using Discord;
using Discord.WebSocket;
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
        private bool logInSuccess;
        private DiscordSocketClient client;
        private static SolidColorBrush redBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
        private static SolidColorBrush greenBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

        public MainWindow()
        {
            logInSuccess = false;
            client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = Discord.LogSeverity.Debug, MessageCacheSize = 0 });
            client.LoggedIn += OnDiscordLoggedIn;
            client.LoggedOut += OnDiscordLoggedOut;
            client.Ready += OnDiscordReady;
            client.Log += OnDiscordLog;
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

        private List<TreeViewModel> CreateChannelsList()
        {
            List<TreeViewModel> channelsNodes = new List<TreeViewModel>();
            foreach(var server in client.Guilds)
            {
                TreeViewModel node = new TreeViewModel(server.Name);
                foreach(var channel in server.TextChannels)
                {
                    TreeViewModel subnode = new TreeViewModel($"#{channel.Name}");
                    subnode.Tag = channel.Id;
                    node.Children.Add(subnode);
                }

                channelsNodes.Add(node);
            }

            return channelsNodes;
        }

        private Task OnDiscordLoggedIn()
        {
            logInSuccess = true;
            Dispatcher.Invoke(new Action(() =>
            {
                connectionStatusLabel.Content = "connected";
                connectionStatusLabel.Foreground = greenBrush;
                connectionButton.Content = "DISCONNECT";
                connectionButton.IsEnabled = true;
            }));

            client.StartAsync();
            App.Log(LogLevel.Info, "Successfully logged in to Discord");
            return Task.CompletedTask;
        }

        private Task OnDiscordLoggedOut()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                channelsTreeView.ItemsSource = new List<TreeViewModel>();
                connectionStatusLabel.Content = "not connected";
                connectionStatusLabel.Foreground = redBrush;
                connectionButton.Content = "CONNECT";
                connectionButton.IsEnabled = true;
            }));

            if(logInSuccess)
                App.Log(LogLevel.Info, "Successfully logged off from Discord");

            logInSuccess = false;
            return Task.CompletedTask;
        }

        private Task OnDiscordReady()
        {
            Dispatcher.Invoke(new Action(() => { channelsTreeView.ItemsSource = CreateChannelsList(); }));
            return Task.CompletedTask;
        }

        private Task OnDiscordLog(LogMessage message)
        {
            string log = $"[DC] [{message.Severity}] {message.Source}: {message.Message} {message.Exception}";
            App.Log(LogLevel.Debug, log);
            return Task.CompletedTask;
        }

        private void ConnectionStateChangeButton_Click(object sender, RoutedEventArgs e)
        {
            connectionButton.IsEnabled = false;

            if(client.LoginState == LoginState.LoggedOut)
                client.LoginAsync(TokenType.Bot, authTokenTextbox.Text);
            else if(client.LoginState == LoginState.LoggedIn)
                client.LogoutAsync();
        }

        private void RootDirBrowseButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton chosenRadio = (sender as RadioButton);
            App.CurrentLogLevel = (LogLevel)chosenRadio.Tag;
        }
    }
}
