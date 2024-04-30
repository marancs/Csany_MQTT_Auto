namespace NaviOkt.View;

public partial class ViewWindows : ContentView
{
    AListaViewModel viewModel;
    public ViewWindows(AListaViewModel vm)
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