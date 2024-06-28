using CodeManager.Core.Models;

namespace CodeManager.Core.Contracts.Services;
public interface IXAMLCodeTemplateService
{
    public void InitializeData(string path);

    List<XAMLCodeTemplate> GetData();
}
