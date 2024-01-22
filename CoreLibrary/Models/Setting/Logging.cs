namespace CoreLibrary.Models.Setting;

public class Logging
{
    public LogLevel? LogLevel { get; set; }   
}

public class LogLevel
{
    public string? Default { get; set; }
    public string? Microsoft { get; set; }
    public string? Lifetime { get; set; }
}