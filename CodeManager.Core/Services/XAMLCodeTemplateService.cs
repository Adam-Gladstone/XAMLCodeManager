using CodeManager.Core.Contracts.Services;
using CodeManager.Core.Models;


namespace CodeManager.Core.Services;

public class XAMLCodeTemplateService : IXAMLCodeTemplateService
{
    private readonly List<XAMLCodeTemplate> _allTemplates = new();

    public void InitializeData(string path)
    {
        _allTemplates.Clear();

        var files = Directory.GetFiles(path, "*.xaml", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);

            var template = new XAMLCodeTemplate
            {
                Name = fileInfo.Name,
                Type = "Control",
                Path = file
            };

            _allTemplates.Add(template);
        }
    }

    public List<XAMLCodeTemplate> GetData()
    {
        return _allTemplates;
    }
}
