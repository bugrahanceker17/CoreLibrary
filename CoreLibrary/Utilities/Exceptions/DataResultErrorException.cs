
using CoreLibrary.Utilities.Result;

namespace CoreLibrary.Utilities.Exceptions;

public static class DataResultErrorException
{
  
}

public static class DataResultError
{
    public static DataResult ThrowError(string message)
    {
        return new DataResult { ErrorMessageList = new List<string> { message } };
    }
}