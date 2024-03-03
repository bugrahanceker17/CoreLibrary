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
}