using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var templateDirectory = @"D:\Development\Projects\C#\XAMLCodeManager\Templates\TEMP";

        var items = new string[]
        {
            "AppBar",
            "AppBarToggleButton",
            "AutoSuggestBox",
            "Border",
            "Button",
            "CalendarDatePicker",
            "CalendarView",
            "Canvas",
            "CheckBox",
            "ComboBox",
            "CommandBar",
            "ContentDialog",
            "DatePicker",
            "DatePickerFlyout",
            "Ellipse",
            "FlipView",
            "Flyout",
            "Grid",
            "GridView",
            "Hub",
            "HyperlinkButton",
            "Image",
            "InkCanvas",
            "InkToolbar",
            "ItemsControl",
            "ListBox",
            "ListView",
            "MapControl",
            "MediaElement",
            "MediaTransportControls",
            "MenuFlyout",
            "PasswordBox",
            "Path",
            "Pivot",
            "ProgressRing",
            "RadioButton",
            "Rectangle",
            "RelativePanel",
            "RepeatButton",
            "RichEditBox",
            "ScrollView",
            "SemanticZoom",
            "SplitView",
            "StackPanel",
            "SymbolIcon",
            "TextBlock",
            "TextBox",
            "TimePicker",
            "TimePickerFlyout",
            "ToggleButton",
            "ToggleSwitch",
            "ToolTip",
            "VariableSizedWrapGrid",
            "ViewBox",
            "WebView"
        };

        foreach (var item in items)
        {
            var filename = Path.Combine(templateDirectory, $"{item}.xaml");

            var contents = BuildContents(item);

            Console.WriteLine($"Writing file: {filename}");

            File.WriteAllText(filename, contents);
        }

        Console.WriteLine("Completed.");
    }

    private static string BuildContents(string name)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"<!-- {name} -->");
        sb.AppendLine($"<{name} x:Name=\"\" Margin=\"0,0,0,0\"></{name}>");

        return sb.ToString();
    }

}