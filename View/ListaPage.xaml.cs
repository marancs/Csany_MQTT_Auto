namespace NaviOkt.View;
public partial class ListaPage : ContentPage
{
	ListaViewModel viewModel;
	public ListaPage(ListaViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
        BindingContext = vm;
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            VerticalLayout.Add(new ViewAndroid(new AListaViewModel(new AutoService())));
        }

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            VerticalLayout.Add(new ViewWindows(new AListaViewModel(new AutoService())));
        }
    }
}