
using CoreLibrary.Utilities.Result;

namespace CoreLibrary.Utilities.Exceptions;

public static class DataResultErrorException
{
    private static string _message;

    // public static DataResult ThrowE()
    // {
    //     
    // }
    
}

public class DataResultErrorExceptionMethods
{
    public DataResult ThrowError(string message)
    {
        return new DataResult { ErrorMessageList = new List<string> { message } };
    }
}