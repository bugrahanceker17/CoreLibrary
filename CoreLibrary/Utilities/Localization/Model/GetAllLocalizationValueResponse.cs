namespace CoreLibrary.Utilities.Localization.Model;

public class GetAllLocalizationValueResponse
{
    public Guid Id { get; set; }
    public string? Key { get; set; }
    public string? ValueEN { get; set; }
    public string? ValueTR { get; set; }
    public string? Description { get; set; }
}