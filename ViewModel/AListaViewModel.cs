using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NaviOkt.ViewModel;

public partial class AListaViewModel : ObservableObject
{
    AutoService AutoService { get; set; }

    public ObservableCollection<Auto> Autok { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;
    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    string title;

    public AListaViewModel(AutoService sz)
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
                    Thread.Sleep(1);
                }
                Console.WriteLine("Alv�s v�ge!!!!");
                if (Autok.Count != 0)
                    Autok.Clear();

                foreach (var m in AutoService.Autok.OrderByDescending(x => x.Kor))
                    Autok.Add(m);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Hiba", "Nem siker�lt bet�lteni az Autokat!!!!", "OK");
            }
            finally
            {
                IsBusy = false;
                Console.WriteLine("Busy v�ge!!!!");
            }
        }
    }

    [RelayCommand]
    Task GoToDetails(Auto Kocsi)
    {
        //return Shell.Current.GoToAsync($"{nameof(DetailPage)}?Rendszam={Kocsi.Rendszam}");
        return Shell.Current.GoToAsync($"{nameof(DetailPage)}", 
                new Dictionary<string, object>
                {
                    ["Kocsi"] = Kocsi
                });
    }
}