using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NaviOkt.ViewModel;

public partial class ListaViewModel: ObservableObject
{
    AutoService AutoService { get; set; }

    public ObservableCollection<Auto> Autok { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;
    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    string title;

    public ListaViewModel(AutoService sz)
    {
        AutoService = sz;
        Title = "Autok";
    }

    [RelayCommand]
    async Task GetAutokAsync()
    {
        if (IsBusy)
        {
            return;
        }
        else
        {
            try
            {
                IsBusy = true;
                await AutoService.GetAutokAsync();
                while (!AutoService.Go)
                {
                    Thread.Sleep(10);
                }
                Console.WriteLine("Alvás vége!!!!");
                if (Autok.Count != 0)
                    Autok.Clear();

                foreach (var m in AutoService.Autok.OrderByDescending(x => x.Kor))
                    Autok.Add(m);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Hiba", "Nem sikerült betölteni az Autokat!!!!", "OK");
            }
            finally
            {
                IsBusy = false;
                Console.WriteLine("Busy vége!!!!");
            }
        }
    }
}