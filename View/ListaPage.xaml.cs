namespace NaviOkt.View;

public partial class ListaPage : ContentPage
{
	ListaViewModel viewModel;
	public ListaPage(ListaViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
        BindingContext = vm;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        viewModel.GetAutokCommand.Execute(null);
    }
}