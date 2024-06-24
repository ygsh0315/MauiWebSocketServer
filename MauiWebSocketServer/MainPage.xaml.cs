using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiWebSocketServer
{
    public partial class MainPage : ContentPage
    {
        private WebSocketServer _webSocketServer = new WebSocketServer("http://localhost:8080/");
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public MainPage()
        {
            InitializeComponent();
            _webSocketServer.OnMessageReceived += OnMessageReceived;
            _ = _webSocketServer.StartAsync();
        }

 
       
        private void OnMessageReceived(string message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (message.StartsWith("SphereY:"))
                {
                    spherePositionLabel.Text = "Height: " + message.Substring("SphereY:".Length);
                }
                else if (message.StartsWith("SliderValue:"))
                {
                    sliderValueLabel.Text = "Rebound Factor: " + message.Substring("SliderValue:".Length);
                }
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _cancellationTokenSource.Cancel();
            _webSocketServer.Stop();
        }
    }
}
