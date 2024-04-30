namespace NaviOkt.View;

public partial class DetailPage : ContentPage
{
	DetailViewModel viewModel;
	public DetailPage(DetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		viewModel = vm;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}