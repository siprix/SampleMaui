using Microsoft.Extensions.Logging;

namespace SampleMaui
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
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var services = builder.Services;

            //Stub service for test purposes (check UI appearance and behaviour)
            //services.AddSingleton<Siprix.ICoreService, Siprix.StubCoreService>();

            //Siprix core service for each platform
            services.AddSingleton<Siprix.ICoreService, Siprix.CoreService>();

            //Obj model (requires service)
            services.AddSingleton<Siprix.ObjModel>();

            //Android call notification service
#if ANDROID
            services.AddTransient<Siprix.ICallNotifService, SampleMaui.Platforms.Android.CallNotifService>();
#else
            services.AddTransient<Siprix.ICallNotifService, Siprix.StubCallNotifService>();
#endif
            //Routes
            Routing.RegisterRoute("AccountsListPage", typeof(Pages.AccountsListPage));
            Routing.RegisterRoute("CallsListPage", typeof(Pages.CallsListPage));
            Routing.RegisterRoute("MessagesPage", typeof(Pages.MessagesPage));
            Routing.RegisterRoute("LogsPage", typeof(Pages.LogsPage));

            //Pages
            services.AddTransient<App>();
            services.AddTransient<Pages.AccountsListPage>();
            services.AddTransient<Pages.CallsListPage>();            
            services.AddTransient<Pages.MessagesPage>();
            services.AddTransient<Pages.LogsPage>();
            return builder.Build();
        }
    }
}
