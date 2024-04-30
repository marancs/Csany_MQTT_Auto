namespace NaviOkt.View;

public partial class ViewAndroid : ContentView
{
    AListaViewModel viewModel;
    public ViewAndroid(AListaViewModel vm)
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