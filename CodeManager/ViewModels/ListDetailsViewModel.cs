using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CodeManager.Contracts.Services;
using CodeManager.Contracts.ViewModels;
using CodeManager.Core.Contracts.Services;
using CodeManager.Core.Models;
using CodeManager.Dialogs;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace CodeManager.ViewModels;

public partial class ListDetailsViewModel : MasterDetailViewModel<XAMLCodeTemplate>, INavigationAware
{
    private readonly IXAMLCodeTemplateService templateDataService;

    public ListDetailsViewModel(IXAMLCodeTemplateService templateDataService)
    {
        this.templateDataService = templateDataService;

        var settings = App.GetService<SettingsViewModel>();

        if (!string.IsNullOrEmpty(settings.TemplateFolder))
        {
            this.templateDataService.InitializeData(settings.TemplateFolder);
        }

        Items.CollectionChanged += Items_CollectionChanged;
    }

    public ICommand CopyCommand => new RelayCommand<string>(CopyCommand_Executed);

    public override bool ApplyFilter(XAMLCodeTemplate item, string filter)
    {
        return item.ApplyFilter(filter);
    }

    public async void TemplateParams()
    {
        try
        {
            var contents = string.Empty;

            var controlParam = await GetControlParamsAsync();
            if (!string.IsNullOrEmpty(controlParam))
            {
                if (Current != null)
                {
                    contents = Current.Contents;

                    var newContents = contents.Replace("x:Name=\"\"", $"x:Name=\"{controlParam}\"");

                    CopyToClipboard(newContents);
                }
            }
        }
        catch (Exception e)
        {
            App.ReportException(e);
        }
    }

    private void CopyCommand_Executed(string? parm)
    {
        TemplateParams();
    }

    private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Debug.WriteLine($"Collection changed: {e.Action}.");
    }

    public void OnNavigatedTo(object? parameter)
    {
        Items.Clear();

        var data = templateDataService.GetData();

        if (data.Count > 0)
        {
            data.OrderBy(c => c.Name).ToList().ForEach(c => Items.Add(c));
        }
        else
        {
            var template = new XAMLCodeTemplate
            {
                Name = "<Empty>",
                Type = "Unknown",
                Path = "Use Settings to select the XAML templates folder."
            };

            Items.Add(template);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task<string> GetControlParamsAsync()
    {
        if (App.MainRoot == null)
        {
            return string.Empty;
        }

        var themeSelectorService = App.GetService<IThemeSelectorService>();

        ControlParamsDialog dlg = new();
        ContentDialog dialog = new()
        {
            XamlRoot = App.MainRoot.XamlRoot,
            RequestedTheme = themeSelectorService.Theme,
            Title = "Control Parameters",
            PrimaryButtonText = "OK",
            IsPrimaryButtonEnabled = false,
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = dlg
        };

        var templateName = string.Empty;
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            templateName = dlg.GetParam();
        }

        return templateName;
    }

    private static void CopyToClipboard(string contents)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{contents}");

        var dataPackage = new DataPackage
        {
            RequestedOperation = DataPackageOperation.Copy
        };
        dataPackage.SetText(sb.ToString());
        Clipboard.SetContent(dataPackage);
    }
}
