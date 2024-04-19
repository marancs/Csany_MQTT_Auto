namespace NaviOkt.View;

public partial class ViewWindows : ContentView
{
    ListaViewModel viewModel;
    public ViewWindows(ListaViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        viewModel.GetAutokCommand.Execute(null);
    }
}