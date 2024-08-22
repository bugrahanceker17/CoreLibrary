using CoreLibrary.Models.Concrete.DataTransferObjects.Base;

namespace CoreLibrary.Utilities.Localization.Model;

public class CreateLocalizationValueRequest : BaseRequest
{
    public string Key { get; set; }
    public string ValueEN { get; set; }
    public string ValueTR { get; set; }
    public string Description { get; set; }
}