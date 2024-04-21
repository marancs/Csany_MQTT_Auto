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
		    VerticalLayout.Add(new ViewAndroid(vm));
		}
	
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
		    VerticalLayout.Add(new ViewWindows(vm));
		}	
	}

	private void ContentPage_Loaded(object sender, EventArgs e)
	{
	viewModel.GetAutokCommand.Execute(null);
	}
}
