using System.Diagnostics;

using CodeManager.Activation;
using CodeManager.Contracts.Services;
using CodeManager.Core.Contracts.Services;
using CodeManager.Core.Services;
using CodeManager.Dialogs;
using CodeManager.Models;
using CodeManager.Services;
using CodeManager.ViewModels;
using CodeManager.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CodeManager;


public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static FrameworkElement? MainRoot { get; private set; }

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IFolderSelectorService, FolderSelectorService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IXAMLCodeTemplateService, XAMLCodeTemplateService>();
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<ListDetailsViewModel>();
            services.AddTransient<ListDetailsViewPage>();

            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.

        if (e.Exception != null)
        {
            Debug.WriteLine(e.Exception.ToString());
        }
    }

    // Generic exception reporting
    public static async void ReportException(Exception e)
    {
        var themeSelectorService = App.GetService<IThemeSelectorService>();

        ContentDialog dialog = new()
        {
            XamlRoot = MainWindow.Content.XamlRoot,
            RequestedTheme = themeSelectorService.Theme,
            Title = "Exception Report",
            PrimaryButtonText = "OK",
            DefaultButton = ContentDialogButton.Primary,

            Content = new ExceptionDialog(e)
        };

        _ = await dialog.ShowAsync();
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);

        MainRoot = MainWindow.Content as FrameworkElement;
    }
}
