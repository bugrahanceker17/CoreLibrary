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
}