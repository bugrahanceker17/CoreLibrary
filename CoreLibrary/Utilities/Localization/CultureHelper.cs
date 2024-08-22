using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Utilities.Localization;

public interface ICultureHelper
{
    string GetCurrentCulture();
    void SetCurrentCulture(string cultureName);
}

public class CultureHelper : ICultureHelper
{
    private string _currentCulture;

    public string GetCurrentCulture()
    {
        return _currentCulture;
    }

    public void SetCurrentCulture(string cultureName)
    {
        _currentCulture = cultureName;
    }
}