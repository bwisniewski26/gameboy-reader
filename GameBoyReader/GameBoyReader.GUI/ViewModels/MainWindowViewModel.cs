using CommunityToolkit.Mvvm.ComponentModel;
using GameBoyReader.Core.Services;
using System.Collections.ObjectModel;

namespace GameBoyReader.GUI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public static ArduinoSerialClient SerialClient { get; } = new();

        [ObservableProperty]
        private string? selectedComPort;

        public ObservableCollection<string> ComPorts { get; }
            = new ObservableCollection<string>(SerialClient.RetrieveAvailableCOMPorts());
    }
}
