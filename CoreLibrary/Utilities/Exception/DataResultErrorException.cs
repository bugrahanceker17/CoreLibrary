
using CoreLibrary.Utilities.Result;

namespace CoreLibrary.Utilities.Exception;

public class DataResultErrorException
{
    private readonly string _errorMessage = string.Empty;
    
    public DataResultErrorException(string errorMessage)
    {
        _errorMessage = errorMessage;
    }

    public DataResult Error()
    {
        return new DataResult { ErrorMessageList = new List<string> { _errorMessage } };
    }
}