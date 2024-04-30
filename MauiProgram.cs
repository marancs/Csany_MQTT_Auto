using Microsoft.Extensions.Logging;

namespace NaviOkt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<AutoService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ListaPage>();
            builder.Services.AddSingleton<ViewAndroid>();
            builder.Services.AddSingleton<DetailPage>();

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<ListaViewModel>();
            builder.Services.AddSingleton<AListaViewModel>();
            builder.Services.AddSingleton<DetailViewModel>();
            return builder.Build();
        }
    }
}
