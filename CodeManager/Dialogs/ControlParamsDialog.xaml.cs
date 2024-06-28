using Microsoft.UI.Xaml.Controls;

namespace CodeManager.Dialogs;

public sealed partial class ControlParamsDialog : Page
{
    public ControlParamsDialog()
    {
        InitializeComponent();
    }

    public string GetParam()
    {
        return TextBoxName.Text;
    }

    private void TextName_TextChanged(object sender, TextChangedEventArgs e)
    {
        var parent = Parent as ContentDialog;
        if (parent != null)
        {
            parent.IsPrimaryButtonEnabled = !string.IsNullOrEmpty(TextBoxName.Text);
        }
    }
}
