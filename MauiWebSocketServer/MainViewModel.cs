using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiWebSocketServer
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _spherePositionY= string.Empty;
        private string _sliderValue= string.Empty;

        public string SpherePositionY
        {
            get => _spherePositionY;
            set
            {
                _spherePositionY = value;
                OnPropertyChanged();
            }
        }

        public string SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
