using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeManager.Core.Models;

public class XAMLCodeTemplate : ObservableObject
{
    public string Name
    {
        get; set;
    }

    public string Type
    {
        get; set;
    }

    public string Path
    {
        get; set;
    }

    public bool ApplyFilter(string filter)
    {
        return Name.Contains(
                filter, StringComparison.InvariantCultureIgnoreCase)
            || (Type is not null && Type.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
            ;
    }

    public string Contents => File.Exists(Path) ? File.ReadAllText(Path) : Path;

    public override string ToString() => $"{Name}";
}
