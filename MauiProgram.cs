﻿using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Firebase;
using AppNotes.Models;
using System.Reactive.Concurrency;
using AppNotes.Services;

namespace AppNotes
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddMudServices();
            builder.Services.AddSingleton<ConexionBBDD>();
            builder.Services.AddSingleton<SynchronizationService>();
            builder.Services.AddSingleton<LibraryService>();
            builder.Services.AddSingleton<EventService>();
            builder.Services.AddSingleton<ToDoService>();
            builder.Services.AddSingleton<RoutineService>();

            return builder.Build();
        }
    }
}
