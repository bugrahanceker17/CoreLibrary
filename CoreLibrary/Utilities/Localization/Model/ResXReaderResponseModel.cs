namespace CoreLibrary.Utilities.Localization.Model;

public class ResXReaderResponseModel
{
    public string Key { get; set; }
    public ResXReaderItemModel Item { get; set; }
    
}

public class ResXReaderItemModel
{
    public string Value1 { get; set; }
    public string Value2 { get; set; }
}