using ColMg.ViewModels;
using ColMg.Views;
using Microsoft.Extensions.Logging;

namespace ColMg
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
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                })
                .Services
                    .AddSingleton<CollectionRepository>()
                    .AddSingleton<CollectionsViewModel>()
                    .AddSingleton<CollectionsPage>()
                    .AddSingleton<CollectionViewModel>()
                    .AddSingleton<CollectionPage>()
                ;

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}