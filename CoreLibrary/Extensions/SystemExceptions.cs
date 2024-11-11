using System.Text;

namespace CoreLibrary.Extensions;

public static class SystemExceptions
{
    public static bool GuidIsNullOrEmpty(this Guid? input)
    {
        if (input == null || input.Equals(Guid.Empty))
            return true;

        return false;
    }

    public static bool GreaterThanZero(this int input)
    {
        if (input > 0)
            return true;

        return false;
    }

    public static bool LessOrEqualToZero(this int input)
    {
        if (input <= 0)
            return true;

        return false;
    }
    
    public static List<T> AsPaginationList<T>(this IQueryable<T> data, int page, int pageSize)
    {
        if (page < 1)
        {
            throw new ArgumentException("Sayfa numarası 1'den küçük olamaz.", nameof(page));
        }

        if (pageSize < 1)
        {
            throw new ArgumentException("Sayfa boyutu 1'den küçük olamaz.", nameof(pageSize));
        }

        return data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }
    
    public static string ConvertTurkishCharsToEnglish(this string input, bool toLowerCase = true)
    {
        StringBuilder sb = new StringBuilder(input);

        if (toLowerCase)
        {
            sb.Replace("Ç", "c");
            sb.Replace("ç", "c");
            sb.Replace("Ğ", "g");
            sb.Replace("ğ", "g");
            sb.Replace("İ", "i");
            sb.Replace("ı", "i");
            sb.Replace("Ö", "o");
            sb.Replace("ö", "o");
            sb.Replace("Ş", "s");
            sb.Replace("ş", "s");
            sb.Replace("Ü", "u");
            sb.Replace("ü", "u");
        }
        else
        {
            sb.Replace("Ç", "C");
            sb.Replace("ç", "c");
            sb.Replace("Ğ", "G");
            sb.Replace("ğ", "g");
            sb.Replace("İ", "I");
            sb.Replace("ı", "i");
            sb.Replace("Ö", "O");
            sb.Replace("ö", "o");
            sb.Replace("Ş", "S");
            sb.Replace("ş", "s");
            sb.Replace("Ü", "U");
            sb.Replace("ü", "u");
        }
       

        return sb.ToString();
    }
}