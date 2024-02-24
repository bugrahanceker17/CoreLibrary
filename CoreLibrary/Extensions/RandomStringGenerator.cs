using System.Text;

namespace CoreLibrary.Extensions;

public static class RandomStringGenerator
{
    public static string Generate(bool includeSymbols, int length)
    {
        string val = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        if(includeSymbols)
            val += "!?&%$";

        Random random = new Random();
        
        var stringBuilder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(val[random.Next(val.Length)]);
        }
        
        return stringBuilder.ToString();
    }
}