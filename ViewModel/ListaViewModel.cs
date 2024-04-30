using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NaviOkt.ViewModel;

public partial class ListaViewModel: ObservableObject
{
    [ObservableProperty]
    string title;

    public ListaViewModel()
    {
        Title = "Autok";
    }

}