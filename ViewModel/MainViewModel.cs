using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NaviOkt.ViewModel;
public partial class MainViewModel : ObservableObject
{
    AutoService AutoSz;
    [ObservableProperty]
    string title;
    [ObservableProperty]
    string welcome;
    public MainViewModel(AutoService service) {
        AutoSz = service;
        Title = "Maui teszt program";
        Welcome = "MAUI - MQTT - NODJS - MYSQL";
    }
}