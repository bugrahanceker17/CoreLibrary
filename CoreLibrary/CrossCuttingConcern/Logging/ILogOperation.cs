namespace CoreLibrary.CrossCuttingConcern.Logging;

public interface ILogOperation
{
    void Information<T>(string logParameter);
    void Error<T>(string logParameter);
}