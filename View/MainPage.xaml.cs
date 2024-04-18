namespace NaviOkt.View;
public partial class MainPage : ContentPage
{
    MainViewModel viewmodel;
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        viewmodel = vm;

    }

}