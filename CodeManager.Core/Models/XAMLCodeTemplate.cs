using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeManager.Core.Models;

public partial class XAMLCodeTemplate : ObservableObject
{
    public string Name { get; set; }

    public string Type { get; set; }

    public string Path { get; set; }

    public bool ApplyFilter(string filter)
    {
        return Name.Contains(
                filter, StringComparison.InvariantCultureIgnoreCase)
            || (Type is not null && Type.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
            ;
    }

    private string helpString;

    public string HelpString
    {
        get
        {
            if (File.Exists(Path))
            {
                var text = File.ReadAllText(Path);

                if (!string.IsNullOrEmpty(text))
                {
                    helpString = ExtractHelpString(text, out _);
                }
            }

            return helpString;
        }
    }

    private string contents;
    public string Contents
    {
        get
        {
            if (File.Exists(Path))
            {
                var text = File.ReadAllText(Path);

                if (!string.IsNullOrEmpty(text))
                {
                    int endPos;
                    helpString = ExtractHelpString(text, out endPos);

                    contents = ExtractContents(text, endPos);
                }
                else
                {
                    contents = text;
                }
            }
            else 
            {
                contents = Path;
            }

            return contents;
        }
    }

    public override string ToString() => $"{Name}";

    private static string ExtractHelpString(string text, out int end)
    {
        string helpString;

        var start = text.IndexOf("<!--");

        if (start != -1)
        {
            end = text.IndexOf("-->");
            if (end != -1)
            {
                helpString = text.Substring(start + 4, end - start - 4).Trim();
            }
            else
            {
                end = 0;
                helpString = "";
            }
        }
        else
        {
            end = 0;
            helpString = "";
        }

        return helpString;
    }

    private static string ExtractContents(string text, int startPos)
    {
        return text.Substring(startPos + 4, text.Length - startPos - 4).Trim();
    }
}
