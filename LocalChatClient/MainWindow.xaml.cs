using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace LocalChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSignalR();
        }

        private async Task InitializeSignalR()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5000/chatHub") // Application 2'nin URL'si
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    OutputTextBlock.Text += $"{user}: {message}\n";
                });
            });

            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendMessage", UserTextBox.Text, InputTextBox.Text);
                InputTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Send error: {ex.Message}");
            }
        }
    }
}
