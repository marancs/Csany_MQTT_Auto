namespace NaviOkt.ViewModel;
[QueryProperty(nameof(Kocsi), nameof(Kocsi))]
public partial class DetailViewModel : ObservableObject
{
    AutoService AutoSz;

    [ObservableProperty]
    string title;


    [ObservableProperty]
    Auto kocsi;

    public DetailViewModel(AutoService service)
	{
		AutoSz = service;      
	}
}