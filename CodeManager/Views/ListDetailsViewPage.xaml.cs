using System.Windows.Input;
using CodeManager.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace CodeManager.Views;

public sealed partial class ListDetailsViewPage : Page
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public ListDetailsViewPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();

        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        base.OnNavigatedTo(e);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

        base.OnNavigatingFrom(e);
    }

    public ICommand CopyToClipboardCommand => new RelayCommand(CopyToClipboard);

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Current" && ViewModel.HasCurrent)
        {
            TemplateListView.ScrollIntoView(ViewModel.Current);
        }
    }

    private void ListViewItem_PointerEntered(object? sender, PointerRoutedEventArgs e)
    {
        if (e.Pointer.PointerDeviceType is PointerDeviceType.Mouse or PointerDeviceType.Pen)
        {
            VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
        }
    }

    private void ListViewItem_PointerExited(object? sender, PointerRoutedEventArgs e)
    {
        VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
    }

    private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        ViewModel.Filter = args.QueryText;
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            ViewModel.Filter = sender.Text;
        }
    }

    private void CopyToClipboard()
    {
        ViewModel?.TemplateParams();
    }

    private void TemplateListView_DoubleTapped(object? sender, DoubleTappedRoutedEventArgs e)
    {
        ViewModel?.TemplateParams();
    }
}
